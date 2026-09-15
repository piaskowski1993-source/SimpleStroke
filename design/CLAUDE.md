# Simple Stroke — Design

Design-phase notes for a daily shape-drawing habit website (working name "Simple Stroke" /
"Master Stroke" — not locked). These are the original brainstorm/concept notes; implementation has
since started as a course project — see `/CLAUDE.md` at the project root for current build status.
Always check lock status before assuming a design decision below is still final.

| File | Contents |
|------|----------|
| Design_Notes | Core loop, identity, affirmation mechanic, feed, moderation, monetization, infra, ToS, market research |

## Key locked decisions (quick reference)

- **Philosophy:** proof-of-concept / learning project, explicitly **not** a cash cow. Cost/revenue
  modeling shows it stays a cheap hobby cost through ~100 users and doesn't plausibly cover its own
  costs (mainly AI moderation + moderation labor) until very late stage, if ever (§7).
- **Format:** a website, not an app. Login required to participate.
- **Core loop:** on signup, assigned one random simple shape. Draw it daily, upload a photo (shape
  just has to be present, embellishment allowed). One upload/day; a flagged upload has no same-day
  retry. No formal penalty for missing a day, no formal reward for uploading — visible instead via a
  public "uploads / days active" ratio per profile (§2).
- **Identity:** no real usernames — permanent sequential number assigned at signup, persistent
  pseudonym. Searchable by number. **No follow/favorite/observe feature, by design** — discovery is
  manual/external (§3).
- **Shape-change mechanic:** opt-in indefinite "wants to change shape" flag, visible under all posts
  with a running affirmation count from other users judging on full context. 100 affirmations unlocks
  a shape change. Purpose: force sitting with a disliked shape before swapping, not pure popularity (§4).
- **Feed:** global, batched updates (interval undecided, ~1–10 min), sorted within each batch by
  streak length — rotating spotlight per batch, not a permanent leaderboard. Explicit, acknowledged
  philosophy shift away from "no reward" — losing a streak does cost feed visibility, a real if
  implicit penalty (§5).
- **Moderation:** AI detection (porn/gore/brand) on upload + community flagging, manually reviewed by
  Piotr at small scale; explicitly acknowledged this needs a different solution once volume exceeds
  manual review capacity (§6).
- **Monetization:** $5 one-time ad-removal + ads (ads are weak here — usage is once-a-day by design,
  don't rely on them). Strongest angle found: print-on-demand personal archive books ("print your
  year"). Store extension (order copies of *other* users' published books) floated but explicitly
  deferred — needs opt-in + revenue-share design before building (§7).
- **ToS stance:** account creation = agreement Piotr can use uploaded photos "as he sees fit." Flagged
  as workable/standard for UGC platforms; recommended (not required) to make it specific about
  physical resale later to avoid backlash (§8).
- **Infra:** self-host the app/server (cheap VPS) fine; do **not** self-host images on a home HDD
  (ISP bandwidth/ToS + single point of failure, not storage cost). Use cloud object storage
  (Cloudflare R2 / Backblaze B2 style). Storage never stops growing — nothing deletes old images (§9).

## Open questions

- Final name (Simple Stroke vs Master Stroke vs other)
- Exact feed batch interval (1 min vs 10 min)
- Streak-reset behavior on a missed day
- Concrete affirmer-eligibility rule (anti-farming safeguard for the affirmation system)
- Book-store opt-in / revenue-share design (ordering other users' books)
- Tech stack (not yet discussed at all)
- Data model / page list (not yet started)
