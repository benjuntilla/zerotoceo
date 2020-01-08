# trump
EXTERNAL GetGameLevel()
EXTERNAL GetPlayerXP()
VAR xp = 0
VAR pendingMinigame = ""

-> start

== function GetGameLevel() ==
~ return 1

== function GetPlayerXP() ==
~ return 0

=== start ===
~ xp = GetPlayerXP()
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- else: -> END
}

=== level_one ===
{ 
    - not greeting:
        -> greeting
    - not quest:
        -> quest
    - else:
        -> dialogue
}
= greeting
    I am trump.
    *   (correct) [Hello!]
        Hello to you, too. (Gained 10xp)
        ~ xp += 10
    *   (incorrect) [Fuck you.]
        Okay. (Lost 10xp)
        ~ xp -= 10
- -> END
= quest
    { greeting.correct:
        You seem like a nice guy. My grandma needs help crossing the street. Will you help?
        *   [Yes.]
            Thanks.
            ~ pendingMinigame = "Minigame_Grandma_Easy"
            -> END
        *   [No.]
            That's all right.
            -> END
    - else:
        I hate you. -> END
    }
= dialogue
    { greeting.correct:
        Hey! -> END
    - else:
        I hate you. -> END
    }

=== level_two ===
Welcome to level two! -> END
