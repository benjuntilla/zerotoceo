# jeremy
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - gameFlags ? talkedToJeremy:
        Did you talk to Adrian yet?
    - GetMinigameProgression() >= 1:
        ~ gameFlags += talkedToJeremy
        Thanks for helping out! If you want more work, go see Adrian.
        He's staring out of that window over there.
        Anyway, I'll see you around.
        -> END
    - quest.accepted:
        Are you on it?
    - quest.declined:
        Are you up to the task now?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Trash_Easy"
                -> END
            *   [No.]
                Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
    - gameFlags ? talkedToJessica:
        -> quest
    - else:
        {~Um, hello.|Please don't bother me.|I'm doing work at the moment.} -> END
}
= quest
I saw you talking to Jessica over there. Was it about me?
    *   [Yes.]
    *   [No.]
        Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
- Well, surely it was about the work I need done?
    *   [Yes.]
    *   [No.]
        Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
- OK, cool. I see you're wearing some sort of fancy pin, so I believe you are trustworthy.
The front of this building is absolutely covered in trash, and it looks disgusting.
Someone needs to do it, and it isn't going to be me.
Are you up to it?
    *   (accepted) [Yes.]
            Cool, great! Get on it.
            ~ pendingMinigame = "Minigame_Trash_Easy"
            -> END
    *   (declined) [No.]
        Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
