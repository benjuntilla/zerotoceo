# jeff
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
-> level_one

=== level_one ===
{ 
    - gameFlags ? talkedToAndy:
        -> quest
    - else:
        {~Do you belong here?|You don't look like a businessman.|Please stop talking to me.} -> END
}
= quest
{ 
- GetMinigameProgression() >= 1:
    Thank you for helping my grandma!
- quest.accepted:
    Are you on it?
- else:
    I see you've talked to Andy about the work I need finished.
    My grandma needs help crossing the street. Will you help?
        *   (accepted) [Yes.]
            Thanks.
            ~ pendingMinigame = "Minigame_Grandma_Easy"
            -> END
        *   [No.]
            That's all right. Maybe later.
            -> END
}
