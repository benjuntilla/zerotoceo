# andy
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
-> level_one

=== level_one ===
Hello, I'm Andy.
    *   [Hello!]
        Hello to you, too.
-   You're the new recruit, right?
    *   [Yes.]
        ~ xp += 10
        Ah, you're here! (Gained 10xp)
    *   [No.]
        Haha, that's funny, recruit.
-   Anyway, I know your dad!
-   He told me about you once.
-   He mentioned that you're in FBLA!
-   Well, you're still in it, right?
    *   [Yes.]
-   Ok, cool!
-   I was in the club when I was your age.
-   Are you in the process of achieving your Business Achievement Awards at the moment?
    *   [Yeah.]
-   All right. I have a task for you. This can help with your community service hours.
-   But, before I can trust you with it, I need you to answer some questions.
    *   [Okay?]
-   (question) You have a 
    *   (correct) [Yes.]
        ~ xp += 10
        All right, that was correct. (Gained 10 xp)
        ~ gameFlags += talkedToAndy
        I don't actually have your task, but go speak to Jeff. He has one for you. -> END
    *   (wrong) [No.]
        ~ xp -= 10
        Okay, that's not quite right. (Lost 10xp)
        -> question
