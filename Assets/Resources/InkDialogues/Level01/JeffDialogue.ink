# jeff
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
-> level_one

=== level_one ===
{ 
    - GetMinigameProgression() >= 1:
        Thank you for helping my grandma!
    - quest.accepted:
        Are you on it?
    - gameFlags ? talkedToAndy:
        -> quest
    - else:
        {~Do you belong here?|You don't look like a businessman.|Please stop talking to me.} -> END
}
= quest
    I see you've talked to Andy about the work I need finished.
    It isn't at all formal work, seeing as you're only a recruit, so here's the task.
    I live with my grandparents right across the street from here.
    My grandma called, saying she wants to visit me at my workplace.
    Obviously, I can't help her across the street myself with all the work I need to do.
    That's where you come in.
    Wait, hold on. Side thought. You were in FBLA, right?
        *   [Yes.]
- (question) {Prove it. How many BAA awards are there?|How many BAA awards are there?}
    *   [Five.]
        ~ xp -= 10
        Not quite right. (Lost 10 XP)
        -> question
    *   [Three.]
        ~ xp -= 10
        Not quite right. (Lost 10 XP)
        -> question
    *   [Four.]
        ~ xp += 10
        Correct. (Gained 10 XP)
    -   All right, you'll help my Grandma cross the street, right?
        *   (accepted) [Yes.]
            All right, great! Get on it.
            ~ pendingMinigame = "Minigame_Grandma_Easy"
            -> END
        *   [No.]
            That's all right. Maybe later.
            -> END
