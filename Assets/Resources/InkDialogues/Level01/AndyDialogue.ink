# andy
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
-> level_one

=== level_one ===
{
    - question && !correct:
        -> question
    - correct:
        Did you talk to Jeff yet? -> END
    - else:
        -> intro
}
= intro
Hello, I'm Andy.
    *   [Hello!]
        Hello to you, too.
-   You're the new recruit, right?
    *   [Yes.]
        ~ xp += 20
        Ah, you're here! (Gained 20 xp)
    *   [No.]
        Haha, that's funny, recruit.
-   Anyway, I know your dad! He told me about you once.
-   He mentioned that you're in FBLA!
-   Well, you're still in it, right?
    *   [Yes.]
-   Ok, cool! I was in the club when I was your age.
-   Are you in the process of achieving your Business Achievement Awards at the moment?
    *   [Yeah.]
-   All right. I have a task for you. This can help with your community service hours.
-   But, before I can trust you with it, I need to know one thing.
    *   [Okay?]
-   (question) Are you willing to put in the work necessary to achieve your ambitions?
-   Your ambitions being receiving all of your four BAAs?
    *   (correct) [Yes.]
        ~ xp += 20
        All right, nice! (Gained 20 xp)
        ~ gameFlags += talkedToAndy
        One problem, though. I don't actually have your task, but Jeff probably has one for you.
        He's by the water cooler over there. Go talk to him about it.  -> END
    *   [No.]
        Well, it doesn't seem like you're the right person for the job then.
        Goodbye. -> END
