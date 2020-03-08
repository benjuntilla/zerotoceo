# joey
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - gameFlags ? talkedToJoey:
        Did you talk to Ricardo yet?
    - GetMinigameProgression() >= 2:
        ~ gameFlags += talkedToJoey
        Hey, man. Thanks for helping out. You seem to like doing this stuff.
            *   [Indeed.]
                Since that's the case, you should see Ricardo. I think he's having some sort of domestic issue.
                -> END
    - quest.accepted:
        Are you on it?
    - quest.declined:
        Are you cool now? Do you want to pick up that trash for me?
            *   [Yes.]
                Cool, great! Get on it.
                ~ pendingMinigame = "Minigame_Trash_Medium"
                -> END
            *   [No.]
                Well, then frankly, I don't know why I'm talking to you. Goodbye. -> END
    - gameFlags ? talkedToAmelie:
        -> quest
    - else:
        *Chugs water* -> END
}
= quest
Ayy, what's up, man.
Do you want to join me?
    *   [What?]
- Chugging water. Do you want to chug water with me?
    *   [Yes.]
        Haha, did you really think I'd let you join me? (Lost 10 XP)
    *   [No.]
        ~ xp += 20
        Party pooper. (Gained 20 XP)
    *   [Why?]
        'Cause it's fun. Party pooper.
-  Anyway, I overheard you and Amelie talking about me having work for someone to do.
It's true. The Manager tasked me with cleaning up the garbage in the front of the building.
He said I was slacking off too much and even accused me of littering used plastic cups.
The nerve of that man. Obviously, I have no intention of cleaning up a mess that I didn't even cause.
Therefore, I will shift the responsibility onto you. Are you content with that?
    *   (accepted) [Yes.]
        Nice, man. See me when you're done.
        ~ pendingMinigame = "Minigame_Trash_Medium"
        -> END
    *   (declined) [No.]
        Lame. Hope you get fired. -> END
