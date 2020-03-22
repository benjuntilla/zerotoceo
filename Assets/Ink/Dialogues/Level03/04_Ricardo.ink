# ricardo
# 0
# Minigame_Grandma_Hard
INCLUDE ../Helper.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - GetMinigameProgression() >= 3:
        Thank you for helping. It is greatly appreciated. I will surely write you any letters of recommendation.
        -> END
    - quest.accepted:
        Please address the problem at hand.
    - quest.declined:
        Are you willing to help me now?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Grandma_Hard"
                -> END
            *   [No.]
                Goodbye, then. -> END
    - gameFlags ? talkedToJoey:
        -> quest
    - else:
        {~Um, hello.|Please don't bother me.|I'm doing work at the moment.} -> END
}
= quest
I saw you talking to that meathead Joey. He probably yapped about the issue I'm having at the moment.
I apologize if he put you off. He isn't at all representative of the standards of this company.
I'm going to assume that you are aware of my problem.
My grandmother called, saying that she needs someone to escort her across some street.
She refuses to use public transportation, instead walking everywhere she goes.
Frankly, it's frustrating, but she's family, so I need someone to help her.
I am currently dealing with pesky clients at the moment, so I don't have any time to do so.
Will you help?
    *   (accepted) [Yes.]
        Thank you very much. Come see me when she's across that street.
        ~ pendingMinigame = "Minigame_Grandma_Hard"
        -> END
    *   (declined) [No.]
        Understandable. Goodbye. -> END
