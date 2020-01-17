# joey
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - gameFlags ? talkedToJoey:
        Did you talk to Adrian yet?
    - GetMinigameProgression() >= 2:
        ~ gameFlags += talkedToJoey
        blah blah blah ricardo
        -> END
    - quest.accepted:
        Are you on it?
    - quest.declined:
        Are you up to the task now?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Trash_Medium"
                -> END
            *   [No.]
                Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
    - gameFlags ? talkedToAmelie:
        -> quest
    - else:
        {~Um, hello.|Please don't bother me.|I'm doing work at the moment.} -> END
}
= quest
blah blah blah
Are you up to it?
    *   (accepted) [Yes.]
            Cool, great! Get on it.
            ~ pendingMinigame = "Minigame_Trash_Medium"
            -> END
    *   (declined) [No.]
        Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
