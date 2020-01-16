EXTERNAL GetGameLevel()
EXTERNAL GetPlayerXP()
EXTERNAL GetRequiredPoints()
EXTERNAL GetMinigameProgression()
VAR xp = 0
VAR pendingMinigame = ""
LIST gameFlags = minigameDone, talkedToAndy, talkedToJessica, talkedToJeremy

== function GetGameLevel() ==
~ return 1

== function GetPlayerXP() ==
~ return 0

== function GetRequiredPoints() ==
~ return -1

== function GetMinigameProgression() ==
~ return 0
