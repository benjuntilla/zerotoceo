# jessica
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{
    - gameFlags ? talkedToJessica:
        Did you talk to Jeremy yet? -> END
}
Hey, I'm Jessica.
    *   [Hello!]
- Nice pin. You're in FBLA, huh?
    *   [Yep.]
- Figures. I was in that club as well.
- (question) Um, which award are you wearing again?
    *   [Business.]
        ~ xp -= 10
        No, I don't think that's quite right. (Lost 10 XP)
        -> question
    *   [Future.]
        ~ xp += 20
        Oh yeah, I remember now! (Gained 20 XP)
    *   [Leader.]
        ~ xp -= 10
        No, I don't think that's quite right. (Lost 10 XP)
        -> question
- Very cool. Seems like your next step for BAA is the Business award.
- Hm, well since you're wearing your pin so brazenly, it seems like you're looking for work.
- Am I correct?
    *   [Yes.]
~ gameFlags += talkedToJessica
- Well, then. There's work for you. Go see Jeremy. He's looking for someone to do his dirty work.
-> END
