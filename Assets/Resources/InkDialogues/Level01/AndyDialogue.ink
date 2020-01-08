# andy
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
-> level_one

=== level_one ===
-> greeting
= greeting
Hello, I'm Andy.
    *   [Hello!]
        Hello to you, too.
-   Are you the new recruit?
    *   [Yes.]
        ~ xp += 10
        Ah, you're here! (Gained 10xp)
    *   [No.]
        Well, too bad.
-   Before I can trust you with this important task, I need you to answer some questions.
    *   [Okay?]
        -> question
= question
-   [Question]
    *   (correct) [Correct]
        ~ xp += 10
        All right, that was correct. (Gained 10 xp)
        ~ talkedToAndy = true
        I don't actually have your task, but go speak to Jeff. He has one for you. -> END
    *   (wrong) [Wrong]
        ~ xp -= 10
        Okay, that's not quite right. (Lost 10xp)
        -> question
