EXTERNAL GetGameLevel()
EXTERNAL GetPlayerXP()
EXTERNAL GetRequiredPoints()
EXTERNAL GetMinigameProgression()
EXTERNAL AddToGameFlags()
VAR xp = 0
VAR pendingMinigame = ""
LIST gameFlags = minigameDone, talkedToAndy, talkedToJessica, talkedToJeremy, talkedToJacquise, talkedToAmelie, talkedToJoey

== function GetGameLevel() ==
~ return 1

== function GetPlayerXP() ==
~ return 0

== function GetRequiredPoints() ==
~ return -1

== function GetMinigameProgression() ==
~ return 0

== function AddToGameFlags(flag) ==
~ gameFlags += flag
