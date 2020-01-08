# worker
EXTERNAL GetGameLevel()

=== start ===
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- else: -> END
}

=== level_one ===
{~Do you belong here?|You don't look like a businessman.|Please stop talking to me.} -> END

=== level_two ===
{~Nice shirt.|Wanna get some refreshments after work?|Looking good.} -> END
