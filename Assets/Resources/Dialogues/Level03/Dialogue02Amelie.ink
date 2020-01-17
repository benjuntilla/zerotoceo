# amelie
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - gameFlags ? talkedToAmelie:
        Did you talk to Joey yet?
    - GetMinigameProgression() >= 1:
        ~ gameFlags += talkedToAmelie
        blah blah blah joey
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
So
Are you up to it?
    *   (accepted) [Yes.]
            Cool, great! Get on it.
            ~ pendingMinigame = "Minigame_Coin_Easy"
            -> END
    *   (declined) [No.]
        Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
