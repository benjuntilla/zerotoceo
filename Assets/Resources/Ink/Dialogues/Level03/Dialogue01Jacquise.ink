# jacquise
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{
    - gameFlags ? talkedToJacquise:
        Did you talk to Amelie yet? -> END
}
Oh, my! Look at those two pins! What are those for?
    *   [FBLA.]
        ~ xp += 20
        Ah! Those are for BAA! I remember now!
    *   [DECA.]
        ~ xp -= 10
        We're not looking for marketers here. Goodbye. (Lost 10 XP) -> END
- I remember how hard it was to get my America pin!
- It was worth it though, because I feel like I gained a lot of knowledge about business in the process.
- Now, look where I am! Working for the third largest business technology producer in the country!
- It's incredible, really. You'll feel the same when you get more of your pins.
- Remember: It's not about the physical reward you gain. It's about the self-improvement.
- That is your goal, right? Self-improvement?
    *   [Yes.]
        ~ xp += 20
        Very good. (Gained 10 xp)
    *   [No.]
        ~ xp -= 10
        Hmm. An odd response. (Lost 10 xp)
- Anyway, There's work for you. Go talk to Amelie. She's typing up some report, so be quiet and don't scare her.
~ gameFlags += talkedToJacquise
-> END
