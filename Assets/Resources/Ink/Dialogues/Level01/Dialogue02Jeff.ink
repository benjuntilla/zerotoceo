# jeff
INCLUDE ../Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ 
    - GetMinigameProgression() >= 1:
        Thank you for helping my grandma! This is definitely going into your service hours.
    - quest.accepted:
        Are you on it?
    - gameFlags ? talkedToAndy:
        -> quest
    - else:
        {~Do you belong here?|You don't look like a businessman.|Hello and goodbye.} -> END
}
= quest
    I see you've talked to Andy about the work I need finished.
    It isn't at all formal work, seeing as you're only a recruit, so here's the task.
    I live with my grandparents right across the street from here.
    My grandma called, saying she wants to visit me at my workplace.
    Obviously, I can't help her across the street myself with all the work I need to do.
    That's where you come in.
    Plus, you're in FBLA, right?
        *   [Yes.]
- (question) {Prove it, then. How many BAA awards are there?|How many BAA awards are there?}
    *   [Five.]
        ~ xp -= 10
        Not quite right. (Lost 10 XP)
        -> question
    *   [Three.]
        ~ xp -= 10
        Not quite right. (Lost 10 XP)
        -> question
    *   [Four.]
        ~ xp += 20
        Correct. Now I know I can probably trust you. (Gained 20 XP)
    -   All right, you'll help my Grandma cross the street, right?
        *   (accepted) [Yes.]
            Cool, great! Get on it. See me when you've finished.
            ~ pendingMinigame = "Minigame_Grandma_Easy"
            -> END
        *   [No.]
            That's all right. Maybe later.
            -> END
