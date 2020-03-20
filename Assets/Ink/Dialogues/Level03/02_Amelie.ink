# amelie
INCLUDE ../Helper.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - gameFlags ? talkedToAmelie:
        Did you talk to Joey yet? He's by the water cooler.
    - GetMinigameProgression() >= 1:
        ~ gameFlags += talkedToAmelie
        These coins are sorted very well. Good job. I'm sure your chapter will be delighted to know that you're doing all this charity work.
        If you want more to do, go see Joey. He's over there chugging cups by the water cooler.
        -> END
    - quest.accepted:
        Are you on it?
    - quest.declined:
        Are you up to the task now?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Coin_Easy"
                -> END
            *   [No.]
                Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
    - gameFlags ? talkedToJacquise:
        -> quest
    - else:
        {~Um, hello.|Please don't bother me.|I'm doing work at the moment.} -> END
}
= quest
Goodness gracious! You scared me! Do you always sneak up on people like that?
What do you want?
    *   [Money.]
        ~ xp -= 10
        I... don't have any for you. Good day. (Lost 10 XP) -> END
    *   [Work.]
        I see! I definitely have that.
    *   [Vices.]
        ...
        ~ xp -= 10
        Bye. (Lost 10 XP) -> END
- So, as you know, BizTek is partnered with March of Dimes, as is the club you're in, FBLA.
How did I know you're in FBLA? Well, you can't expect anybody not to when you're constantly showing off those pins.
Anyway, you know what March of Dimes is, right?
    *   [Huh?]
- (info) I see. Well, prepare for some learning.
March of Dimes is a charity organization.
Being the altruistic man he is, your father decided to do some good for the world.
Thus, BizTek partnered with March of Dimes.
"What are the dimes for" and "Why is there marching," you're wondering?
Well, the dimes are for funding infant mortality and birth defect prevention.
The money goes towards education, advocacy, and research about the cause.
The marching is just a method for raising money for the organization.
Do you now understand what the purpose of March of Dimes is?
    +   [Yes.]
    +   [No.]
        -> info
- Great. Anyway, BizTek just did a march, and we received a ton of coins.
March of Dimes wants everything sorted before we send them the money, and I don't have enough time to do it myself.
You can see where this is going, right?
    *   [Yes.]
    *   [No.]
        ...I want you to sort the coins.
- Now, are you up to the task?
    *   (accepted) [Yes.]
            Cool, great! See me when you're done.
            ~ pendingMinigame = "Minigame_Coin_Easy"
            -> END
    *   (declined) [No.]
        Well, then goodbye. -> END
