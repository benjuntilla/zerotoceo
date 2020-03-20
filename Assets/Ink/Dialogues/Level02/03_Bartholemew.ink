# bartholemew
INCLUDE ../Helper.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - GetMinigameProgression() >= 2:
        Thanks you for your help.
        Unfortunately, I have no monetary compensation for you; however, I am willing to write any good words for you.
        As I can see from your pin, you are in FBLA; therefore, you need service hours.
        Thanks to me, you have them. You're welcome, and good day. -> END
        -> END
    - quest.accepted:
        Are you working on it?
    - quest.declined:
        Have you changed your mind?
            *   [Yes.]
                Many thanks. Return to me once the task is done.
                ~ pendingMinigame = "Minigame_Grandma_Medium"
                -> END
            *   [No.]
                Please. Only return once you are willing to complete the task. -> END
    - gameFlags ? talkedToJeremy:
        -> quest
    - else:
        {~Go away.|Do not bother me.|I am in the middle of deep thought at the moment.} -> END
}
= quest
I am on the verge of a breakthrough right now! For what purpose are you distracting me?
Did one of my colleagues make you do this?
    *   [No.]
        I appreciate your honesty. Now, if you will, please leave me. -> END
    *   [Don.]
        There is no one named Don working in this facility. I can see through your lies.
        Get out of my sight. -> END
    *   [Jeremy.]
        Oh, I see. It's probably important then.
- Hmm, I wonder. Why would Jeremy direct you to me?
Oh, I remember now! My grandmother!
I apologize. The initial reason I was looking out this window was to observe my grandmother.
As you can see, I fell into the routine trap of deep thought.
Anyway, it seems like she's having a hard time crossing the street.
Will you help her?
    *   (accepted) [Yes.]
            Many thanks. Return to me once the task is done.
            ~ pendingMinigame = "Minigame_Grandma_Medium"
            -> END
    *   (declined) [No.]
        Perhaps another time. Goodbye. -> END
