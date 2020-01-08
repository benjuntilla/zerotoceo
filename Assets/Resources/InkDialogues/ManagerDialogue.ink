# manager
EXTERNAL GetGameLevel()
EXTERNAL GetPlayerXP()
VAR xp = 0
~ xp = GetPlayerXP()

=== start ===
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- else: -> END
}

=== level_one ===
= introduction
Welcome to BusinessTek! I am the head manager here--your father's right hand man.
My duties include assisting the recruits, so here I am.
I will provide you advice whenever it seems necessary. Otherwise, I will attend to my other various tasks.
I know that your father wants you to acquire business skills that will help you later in your life.
You can begin by helping one of our senior members, Zander. You will find him by his cubicle.
Good luck. I hope you will find your place in this company!
-> END

=== level_two ===
= introduction
Welcome to the second floor!
When management sees potential in you, they move you up here.
You should be proud for having moved up the ladder so soon! Your father must be very proud.
-> END
