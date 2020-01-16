# andy
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
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
-   (introquestion) You're the new recruit, right?
    *   [Yes.]
        ~ xp += 20
        Ah, you're here! (Gained 20 xp)
    *   [No.]
        Really? <> -> introquestion
-   Anyway, I know your dad! He told me about you once.
-   He mentioned that you're in FBLA!
-   Well, you're still in it, right?
    *   [Yes.]
-   Ok, cool! I was in the club when I was your age.
-   Are you in the process of achieving your Business Achievement Awards at the moment?
    *   [What?]
        You don't know what BAA is?
        -   (info) OK, well, BAA is a program designed to help FBLA members enhance their business and leadership skills.
        BAA has 4 levels, Future, Business, Leader, and America.
        Completing each involves doing projects that develop yourself and your community.
        The difficulty of the projects increase as you progress through the 4 levels.
- Now do you understand what BAA is?
    +   [Yes.]
    +   [No.]
        -> info
-   All right. I have a task for you. This can help with your community service hours.
-   But, before I can trust you with it, I need to know one thing.
    *   [Okay?]
-   (question) Are you willing to put in the work necessary to achieve your ambitions?
-   Your ambitions being receiving all of your four BAAs?
    *   (correct) [Yes.]
        ~ xp += 20
        All right, nice! (Gained 20 xp)
        ~ gameFlags += talkedToAndy
        You're on your way to receiving your first achievement, the Future award.
        One problem, though. I don't actually have your task, but Jeff probably has one for you.
        He's by the water cooler over there. Go talk to him about it.  -> END
    *   [No.]
        Well, it doesn't seem like you're the right person for the job then.
        Goodbye. -> END
