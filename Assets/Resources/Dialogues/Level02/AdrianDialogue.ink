# adrian
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - GetMinigameProgression() >= 2:
        Thanks for helping out!
        I'll see you around.
        -> END
    - quest.accepted:
        Are you on it?
    - quest.declined:
        Are you up to the task now?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Grandma_Medium"
                -> END
            *   [No.]
                Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
    - gameFlags ? talkedToJeremy:
        -> quest
    - else:
        {~Um, hello.|Please don't bother me.|I'm doing work at the moment.} -> END
}
= quest
something something something
Are you up to it?
    *   (accepted) [Yes.]
            Cool, great! Get on it.
            ~ pendingMinigame = "Minigame_Grandma_Medium"
            -> END
    *   (declined) [No.]
        Perhaps another time. Goodbye. -> END
