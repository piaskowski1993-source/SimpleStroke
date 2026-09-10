# Simple Stroke — Design Notes
Draft · August 2026 · Concept, core loop, identity, affirmation mechanic, feed, moderation, monetization, infra

---

## 1. Philosophy — Locked

A website (not an app) built around daily drawing practice. Explicitly framed as a **proof-of-concept
and learning project, not a cash cow.** Piotr's own words: a good design-and-operations learning
experience even if it never makes real money.

This framing came out of cost/revenue modeling (§7): the site stays a cheap out-of-pocket hobby cost
through roughly 100 users, becomes a real but modest monthly cost around 1,000 users that revenue may
or may not cover, and at 10,000 users the actual budget-breaker is moderation staffing, not servers —
not ad income. Given that, the honest framing is "build it because it's interesting and educational,"
not "build it to make money."

---

## 2. Core Loop — Locked

- Login required to participate.
- On signup, the user is assigned one random simple shape.
- Daily task: draw that shape and upload a photo of it. Artistic embellishment is fine — the only
  requirement is that the assigned shape is present in the drawing.
- **One upload per day.** If it gets flagged/rejected by moderation, there is no retry that day —
  next chance is the following day. (Flags this as a real fairness dependency on moderation accuracy —
  AI content-moderation models trained on photos are known to false-positive on illustrations/line
  art; since a flag now has a real, unrecoverable cost, moderation vendor quality matters more than
  it would otherwise.)
- **No formal penalty for missing a day. No formal reward for uploading.** Instead, every profile
  shows a public **"uploads / days active" ratio** — a passive, always-visible consistency stat rather
  than a streak counter or points system.
- 10-word max caption under each photo. No other post metadata.

---

## 3. Identity — Locked

- **No real usernames.** Each account is assigned a permanent sequential number at signup (issued
  chronologically), functioning as a stable pseudonym — not a one-shot-anonymous post.
- Content/profiles are **searchable by number.**
- **No follow, favorite, or "observe" feature — by design.** If a user wants to keep track of someone
  and find them again later, they have to note the number themselves, outside the site.
- This is an explicit, acknowledged tradeoff: it gives up the single strongest normal retention
  mechanic (following creators you like) in exchange for no persistent social graph and no
  algorithmic amplification — the same stance BeReal took. It also keeps the site from needing
  follower counts, notifications, or a "who to follow" feed.
- Later validated during the monetization discussion (§7): the proposed book-store preview feature
  would give people a reason to browse and informally track interesting users anyway, just through
  manual/external note-taking rather than an in-app feature — consistent with, not contradicting,
  this design.

---

## 4. Shape-Change / Affirmation Mechanic — Locked (core), open (anti-gaming detail)

- A user can opt in to an **indefinite "wants to change shape" flag**, visible under every one of
  their posts, alongside a running count of affirmations received.
- Other users decide for themselves whether to affirm, using full context — the requester's post
  history and upload ratio are visible, not just a blind up/down vote.
- **At 100 affirmations, the user may change to a new shape.**
- Explicit purpose, in Piotr's words: *"I understand but this is what you got and now you have to at
  least master this shape, and if you still do not enjoy it you can change it later."* — this is a
  mastery/patience gate, not a popularity contest. The 100-affirmation threshold exists so people sit
  with a disliked shape rather than escaping it instantly.

**Open / not yet designed:** anti-gaming safeguards. Piotr's stated principle: *"anything that can be
implemented to restrict users should and will be applied."* Needs a concrete rule before build —
candidates discussed but not decided: requiring an affirmer to have their own credible account
history before their affirmation counts, and/or rate-limiting how often one account can affirm.
Without some floor, affirmations degrade into a plain like-button the moment anyone wants to game
them.

---

## 5. Feed — Locked (core mechanic), open (interval)

- Global feed, not per-user/following-based (consistent with §3 — no social graph).
- Updates in short batches (interval discussed as both ~10 minutes and ~1 minute — **not yet
  decided**).
- **Within each batch, posts are sorted by streak length** — the most consistent uploader currently
  posting gets top placement for that window.
- Because ranking resets every batch rather than being a persistent global leaderboard, this behaves
  as a **rotating spotlight** (whoever's most consistent right now gets the moment) rather than a
  fixed leaderboard that permanently buries newcomers. Piotr confirmed this resolves the "newcomers
  are invisible forever" concern, since each batch is a fresh contest.
- **This is an explicit, acknowledged philosophy shift.** Ranking by streak means breaking a streak
  costs feed visibility — a real penalty, just an implicit one (lost placement) rather than an
  explicit one (points/ban). Piotr confirmed he wants this competitive element; the site's marketing
  language should stop claiming "no reward/no penalty" without qualification once this ships.
- At low real-world volume, most batch windows will only contain 1–2 uploads, so the streak-sort
  won't meaningfully do anything until there's real daily traffic — expected, not a flaw.

---

## 6. Moderation & Bans — Locked (MVP plan)

- Uploads run through **AI detection for porn, gore, and brand content.**
- **Community flagging** on top of that, reviewed manually by Piotr at current (small) scale.
- Explicit, stated understanding: manual review will not scale — a different solution is needed once
  volume outgrows it, deferred rather than solved now ("when it becomes too much then I find another
  solution").
- **Bans** are the consequence for rule violations: content that doesn't contain the day's shape, or
  flagged/prohibited content.
- Noted dependency: most ad networks require a working moderation system before approving a
  UGC-image site for ads at all — so this isn't just a safety feature, it's a monetization
  prerequisite (§7).
- Real cost line, not just a build task: AI moderation APIs charge per image call (~$0.001–0.0015 per
  call in current market pricing) — a recurring operating cost that scales with upload volume, not a
  one-time build cost.

---

## 7. Monetization — Draft (secondary goal, "pocket money" not primary)

**Cost/revenue modeling (rough estimates, unlaunched product, treat as directional):**

- **10 users:** ~$6–8/mo cost (VPS + free-tier storage/moderation), ~$0–5 one-time revenue. Net
  negative, pure hobby spend.
- **100 users:** ~$15–20/mo cost, ~$0–15 revenue (ad networks unlikely to even approve a site this
  small). Net negative, still trivial in absolute terms.
- **1,000 users:** ~$80–120/mo cost, ~$55–160/mo revenue (roughly breakeven, could land either side
  depending on ad fill rate).
- **10,000 users:** ~$1,200–1,800/mo cost — the big new line here is **paid moderation help
  (~$500+/mo)**, since manual review by Piotr alone stops being feasible at this volume. ~$400–1,200/mo
  revenue. Likely net negative once moderation labor is counted; roughly breakeven on infra alone.
- **Weakest number in the whole model: ad revenue.** The product is once-a-day by design (no
  doomscroll, no infinite feed pressure), so ad impressions per user are structurally much lower than
  a typical social app. Don't plan around ads meaningfully covering costs before very late stage, if
  ever.
- **Storage is the one cost line that never stops growing** — nothing in the design deletes old
  images, so cumulative storage cost climbs every month indefinitely. Worth an archival/cold-storage
  decision eventually (not urgent now).

**Revenue ideas, by how well they fit the product:**

1. **Print-on-demand personal archive book** ("print your year" / "print your shape journey") —
   strongest fit found. The site naturally accumulates a dated personal drawing archive per user as a
   side effect of normal use, which is exactly the raw material print-on-demand book services want.
   Fulfilled through a POD service (e.g. Peecho/Lulu-style), Piotr takes a margin, cost only incurred
   after a sale — no inventory risk. Fairly unique angle since most competitor products don't produce
   a naturally book-worthy archive.
2. **Private timelapse video export** — stitching a user's dated photos of the same shape into a
   short "watch yourself learn to draw X over 6 months" video. Small technical lift, strong emotional
   payoff, natural one-time paid extra.
3. **Bundle, don't microtransact** — combine ad-removal + timelapse export + book-order discount into
   one "supporter" purchase rather than several separate small charges (matches the industry pattern
   that one-time-purchase options alongside/bundled with other paid features lift total conversion).
4. **Hard rule: monetize the record, never the mechanic.** Anything paid must stay cosmetic/output-only
   (books, video, ad-removal) and must never let someone pay to skip toward the 100-affirmation shape
   change — doing so would turn the site's core "earned mastery" premise into pay-to-win and undercut
   the whole concept.
5. **Store extension — floated, explicitly deferred, not decided:** letting users order copies of
   *other* users' already-published books (with a preview), not just their own. Flagged as needing an
   explicit answer before building: does the original creator get a revenue cut, and is a book public/
   orderable-by-others opt-in or automatic? Piotr's call for now: not solving this yet ("enough
   fairness for now") — see §8 for how this is currently handled instead.
6. **Purpose-tied differentiator (lower priority, not a real earner on its own):** precedent is the
   Forest app tying its paid tier to funding real tree-planting via a nonprofit partner — not a
   revenue driver by itself, but a values-aligned hook that generates goodwill/press. An equivalent
   here might be book-sale proceeds partly funding art-supply donations to a kids' program.
7. **Longer-shot, real category if it ever gets there:** institutional/B2B licensing — art teachers,
   art-therapy programs, workplace wellness programs paying flat fees for private/branded cohort
   access. A single institutional client could outearn thousands of ad-supported users, and it
   sidesteps the low-engagement/ad-RPM problem entirely. Not a near-term plan.
8. **Structural point in Piotr's favor:** staying a website rather than wrapping this in a native app
   avoids Apple/Google's 15–30% platform cut on payments — Stripe-style web payment processing runs
   closer to ~3%. Worth remembering if a native app wrapper is ever considered later.

---

## 8. Legal / ToS Stance — Locked (for now)

Piotr's decision: account creation implies agreement that uploaded photographs can be used by him
"as he sees fit." This is how the unresolved book-store fairness question (§7, item 5) is being
handled for now, rather than building consent/revenue-share infrastructure up front.

Feedback given, not a blocker, worth revisiting when writing real terms:
- Broad content-use clauses are actually standard practice on UGC platforms (Instagram, X, etc. all
  have some version of "by uploading, you grant us a license to use/display/reproduce this").
- The vague phrase "as I see fit" is the one soft spot — if someone later discovers their drawing was
  printed and sold to a stranger, the *surprise* itself (not necessarily the underlying right) is
  what tends to generate backlash/chargebacks/bad press. A specific line ("including creating and
  selling printed reproductions of your uploads") costs nothing extra in the ToS and mostly just
  removes that reaction later. Cheap insurance, not urgent — fine to write real terms whenever the
  project gets closer to actually launching.

---

## 9. Infrastructure — Locked (guidance, not yet implemented)

- **Self-hosting the app/server layer is fine** — a cheap VPS (a few dollars/month at small scale) is
  perfectly adequate early on.
- **Do not self-host user-uploaded images on a personal home hard drive.** The real risks aren't
  storage cost (photos are small, a few hundred KB–few MB each) — they're residential ISP bandwidth
  limits/ToS restrictions on running servers, and single-point-of-failure data loss (one drive
  failure or house fire destroys every user's entire drawing history, which for this specific product
  — a record of practice over time — is a much worse failure mode than ordinary downtime).
- **Use cloud object storage instead** (Cloudflare R2 or Backblaze B2 style) — cheap at this scale
  (R2 has no egress fee), resilient, no home-network exposure.
- Storage is the one cost line that compounds indefinitely since nothing in the design deletes old
  images (cross-ref §7) — worth a cold-storage/archival policy decision eventually, not urgent now.

---

## 10. Market Research Notes (Aug 2026)

- **Closest structural comp: BeReal** — daily candid photo ritual, unfiltered, thin social identity.
  Peaked and then declined largely because the daily ritual had no skill payoff — showing up
  accumulated nothing. Simple Stroke's shape-mastery/affirmation system is a direct answer to that
  gap: showing up *does* accumulate toward something (a shape change).
- **"No guilt / no streak" habit trackers are a real, validated 2026 niche**, not an untested idea —
  Kriya, SevenGrid, Ordly, Finch all explicitly market "no streaks, no guilt, no reset" as their hook.
  The site's original "no penalty/no reward" framing sits in this validated lane (before the §5 feed
  ranking decision partially walked that back on purpose).
- No existing direct comp found combining a BeReal-style daily-capture ritual with a skill-mastery/
  drawing-practice angle specifically — this combination appears to be genuinely open territory.
- Print-on-demand as a UGC monetization layer is a proven, low-risk pattern generally (pay-after-sale,
  no inventory) — the novelty here is that this specific product naturally generates the kind of
  personal archive that makes a good book, as a side effect of normal use rather than a bolted-on
  feature.

---

**Status:** Concept/brainstorm stage throughout. Nothing built. See [[feedback_dont_build_unprompted]]
in memory — do not start implementation until Piotr explicitly gives a build go-ahead. Next open
items, not yet touched: tech stack, data model, page-by-page wireframe list.
