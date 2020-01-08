EXTERNAL GetGameLevel()
EXTERNAL GetPlayerXP()
VAR xp = 0
VAR pendingMinigame = ""
// Set by the dialogue manager script
VAR minigameDone = false
VAR talkedToAndy = false

== function GetGameLevel() ==
~ return 1

== function GetPlayerXP() ==
~ return 0
