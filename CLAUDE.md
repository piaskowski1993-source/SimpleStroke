# SimpleStroke — Project Status

Living status doc for whoever (whichever Claude session) picks this project up next. For the
original product concept/brainstorm, see `design/Design_Notes.md` and `design/CLAUDE.md` — those
are design-phase notes and are now partially superseded by real build decisions below.

## What this is

Piotr's graded project for **BE03 - Backend Advanced** (kh_vestfold/telemark), a 4-week course
module: TDD Web API → Docker Compose → IAM/JWT → declarative infra. One project is built up
across all four weeks rather than four disconnected assignments (confirmed by the course's own
Uke 1 assignment text: "Prosjektet dere starter på nå vil videreutvikles gjennom resten av
modulen"). Uke 1's actual graded submission is already banked under the prior project
(WroomWroom, pre-pivot) — everything in `SS.Core`/`SS.Api` here is a from-scratch rebuild of that
same MVP-core step for SimpleStroke, since Uke 2+ require an "existing system" to build on and
WroomWroom isn't it anymore.

Full pivot history, budget/hosting constraints, and course-scope decisions live in Claude's memory
(`project-simplestroke-course`), not repeated here.

## Conventions

- **Naming: `SS` prefix, not `SimpleStroke` spelled out** — solution/project names, namespaces
  (e.g. `SS.Core`, `SS.Api`, `SSDbContext`). Deliberate choice after WroomWroom made the full name
  a chore to type everywhere.
- **Git commit style**: TDD work follows literal `red: ...` / `green: ...` commit messages,
  matching the actual red→green→refactor cycle, per the course's own rubric expectation of visible
  TDD commit history.
- **Piotr hand-types all C#/project code himself** — Claude gives code as chat blocks to type in,
  never edits project source files directly (memory/status files like this one are the exception).
- **Piotr runs and reports back nearly all terminal commands himself**, including verification/
  checks (build output, git status, file contents) — Claude tells him what to check and how, he
  runs it. This is deliberate (learning the whole process, not just the code), not a workflow
  inefflciency. Exception made only for Docker commands with several repeated flags that were
  breaking on paste (shell line-splitting) — those Claude ran directly.

## Architecture (current)

- **`SS.Core`** — plain domain logic, no infrastructure dependencies. `Shape` enum, `User`
  (Id/Shape/UploadCount + `Upload()`), `ShapeAssigner`. Built TDD-first, 3 tests, red→green
  commits per class.
- **`SS.Core.Tests`** — xUnit tests for the above.
- **`SS.Api`** — ASP.NET Core Web API, **controller-based** (not minimal APIs — deliberately
  regenerated with `-controllers` flag, since Uke 3's assignment requires an `AuthController`
  class, matching what the course itself teaches in Modul 2).
  - `Controllers/UsersCOntroller.cs` — POST/GET `api/users` (create, get by id). Note the typo in
    the filename (capital O) — harmless, C# doesn't care, just a known wart if searching for it.
  - `Data/SSDbContext.cs` — EF Core DbContext, `DbSet<User> Users`. Needed an explicit
    `OnModelCreating` → `HasKey(u => u.Id)` override — `User.Id` is a get-only property with no
    setter, and EF's convention-based PK discovery didn't reliably pick it up from that alone.
  - `UserStore.cs` — EF-backed, async, replaces an earlier in-memory-`Dictionary` version (deleted
    from `SS.Core`, since it depends on `SSDbContext` which lives in `SS.Api` — keeping it in
    `SS.Core` would've created a circular project reference).
  - `Program.cs` — registers `SSDbContext` (Npgsql), health checks (`/health`), runs
    `db.Database.Migrate()` on startup. Connection string: config (`appsettings.json`, local dev)
    with env var fallback (`ConnectionStrings__DefaultConnection`, for Docker later).
  - First EF Core migration (`InitialCreate`) generated and committed.

## Current status (Uke 2 — Docker week, in progress)

Done: full API + EF Core + Postgres wiring described above, migration generated, local Postgres
running in Docker (`ss-postgres` container, mapped to host port **5433** — port 5432 is already
taken on this machine by an unrelated WroomWroom Postgres container, `wroomwroom-db-1`, left
running from that project; don't touch it, it's not part of SimpleStroke).

Not yet done, in order:
1. Run the API locally (`dotnet run --project SS.Api`) against the `ss-postgres` container and
   confirm migration + create/get actually persist data (in progress as of this note).
2. Write the API's own `Dockerfile`, test it standalone first (per the assignment's own
   recommended order — build/run the API container alone before wiring up compose).
3. Write `docker-compose.yaml` — API + Postgres + pgAdmin + healthcheck. Reference pattern already
   pulled from the course's linked example repo (`Joviank/ToDo-prosjekt`) — ask Claude for it
   rather than re-researching from scratch.
4. Verify pgAdmin can see the data, and that data survives an API container restart (the actual
   rubric requirement — not the DB container restarting, the API one).
5. Grab the commit URL for submission.

Then Uke 3 (JWT/IAM — `AuthController`, Excalidraw auth-flow diagram, user data isolation) and
Uke 4 (declarative infra — assignment wasn't posted on Canvas as of last check).

## Checking in on start

If you're a fresh Claude session opening this project: read this file, then check
`git log --oneline` for anything more recent than what's listed above (this file may lag slightly
behind the actual repo state) before assuming anything here is still accurate.
