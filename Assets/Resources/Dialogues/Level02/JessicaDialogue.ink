# jessica
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
Hey, I'm Jessica.
    *   [Hello!]
-   Nice pin. You're in FBLA, huh?
    *   [Yep.]
-   Figures. I was in that club as well.
-   (question1) Um, which award are you wearing again?
    *   [Business.]
        ~ xp -= 10
        No, I don't think that's quite right. (Lost 10 XP)
        -> question1
    *   [Future.]
        ~ xp += 20
        Oh yeah, I remember now! (Gained 20 XP)
    *   [Leader.]
        ~ xp -= 10
        No, I don't think that's quite right. (Lost 10 XP)
        -> question1
-   Hm, well since you're wearing your pin so brazenly, it seems like you're looking for work.
-   Am I correct?
    *   [Yes.]
~ gameFlags += talkedToJessica
-   Well, then. There's work for you. Go see Jeremy. He's looking for someone to do his dirty work.
-> END
