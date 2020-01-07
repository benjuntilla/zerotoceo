# exit door
EXTERNAL GetGameLevel()

=== start ===
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- else: -> END
}

=== level_one ===
You approach the door to leave but suddenly realize that you shouldn't abandon your journey so soon. -> END

=== level_two ===
You've already made it this far. Why would you leave now? -> END
