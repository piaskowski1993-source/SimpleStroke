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

## Current status (Uke 2 — Docker week, DONE)

Full API + EF Core + Postgres wiring, `Shape` persistence bug fixed (was silently dropped — EF's
convention-based discovery skips get-only properties unless explicitly referenced in
`OnModelCreating`; fixed with an explicit `Property(u => u.Shape)` call, same trick as `HasKey` for
`Id`), standalone `Dockerfile` built and verified, `docker-compose.yaml` (API + Postgres + pgAdmin
+ healthcheck) up and verified — pgAdmin shows the data, and data survives an **API** container
restart (the actual rubric requirement, not the DB container restarting).

Repo pushed to GitHub, public: https://github.com/piaskowski1993-source/SimpleStroke — submission
commit: https://github.com/piaskowski1993-source/SimpleStroke/commit/ec4d49a03311fa0936c144362ba6218dbb823f04

Local dev Postgres still runs as the standalone `ss-postgres` container (host port **5433** — 5432
is taken by an unrelated WroomWroom container, `wroomwroom-db-1`, left running from that project;
don't touch it). Stop `ss-postgres` before running `docker compose up` for the API, since compose's
own `db` service also wants host port 5433.

## Current status (Uke 3 — IAM/JWT, "Who shall pass?", in progress)

Real Ukesoppgave 3 text obtained from Canvas 2026-09-16 (title: "Who shall pass?"). Actual scope,
narrower than the earlier guess:

- **Not** a full login/issuing system — goal is understanding how an API *reads* an
  already-issued token and uses its claims to restrict data access. Test tokens come from jwt.io,
  not a real Identity Provider.
- **Del 1A** — Excalidraw flow diagram: Resource Owner / Client / Identity Provider / Resource
  Server, showing how a token gets to the API.
- **Del 1B** — same Excalidraw file, data-ownership modeling: define 3 rules for how data belongs
  to a user (at least 1 self-formulated, not copied from the assignment's own examples).
- **Del 2** — `AuthController`: reads JWT from the `Authorization` header, decodes with
  `JwtSecurityTokenHandler` (package: `System.IdentityModel.Tokens.Jwt`), returns the claims.
  Verify by pasting a jwt.io token and checking claims come back correctly.
- **Del 3** — Swagger: add `OpenApiSecurityScheme` in `Program.cs` so the Authorize button accepts
  a JWT and sends it with requests.
- **Del 4** — wire the Del 1B design into the real domain: use the JWT's `sub` claim to identify
  the calling user and restrict `SS.Api` endpoints so User A can't reach User B's data. Reference
  tutorial (`Joviank/ToDo-prosjekt`, `Tutorial/Uke 4 (IAM)/JwtOppsett.txt`) does this by adding a
  `UserId` field to the owned entity and filtering queries by the `sub` claim — SimpleStroke's
  analogous shape still needs deciding (`User` currently *is* the top-level entity, no separate
  child entity being owned yet).
- **Del 5** — submit: Excalidraw file goes in a root-level `Excalidraw/` folder (`.png` or
  `.excalidraw`, descriptive filename), commit, grab the commit URL.

Rubric also explicitly checks: domain logic stays in service/domain layer (not scattered auth
logic), scope stays tight (no new features beyond auth + ownership).

Not started yet — next step is talking through the Del 1A/1B design before writing any code, per
the assignment's own instruction to diagram intended understanding before implementing.

## Then: Uke 4

Declarative infra — assignment wasn't posted on Canvas as of last check.

## Checking in on start

If you're a fresh Claude session opening this project: read this file, then check
`git log --oneline` for anything more recent than what's listed above (this file may lag slightly
behind the actual repo state) before assuming anything here is still accurate.
