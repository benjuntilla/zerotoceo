# exit door
EXTERNAL GetGameLevel()

=== start ===
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- 3: -> level_three
- 4: -> level_four
- else: -> END
}

=== level_one ===
You approach the door to leave but suddenly realize that you shouldn't abandon your journey so soon. -> END

=== level_two ===
You've already made it this far. Why would you leave now? -> END

=== level_three ===
You've already made it this far. Why would you leave now? -> END

=== level_four ===
Are you really going to abandon your father? -> END
