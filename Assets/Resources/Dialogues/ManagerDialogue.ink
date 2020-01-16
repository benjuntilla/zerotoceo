# manager
INCLUDE Generic.ink

-> start

=== start ===
~ xp = GetPlayerXP()
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- 3: -> level_three
- else: -> END
}

=== level_one ===
Welcome to BizTek! I am the head manager here--your father's right hand man.
My duties include assisting the recruits, so here I am.
I will provide you advice whenever it seems necessary. Otherwise, I will attend to my other various tasks.
I know that your father wants you to acquire business skills that will help you later in your life.
You can begin by helping one of our senior members, Andy. You will find him by his cubicle.
Good luck. I hope you will find your place in this company!
-> END

=== level_two ===
Welcome to the second floor!
When management sees potential in you, they move you up here.
You should feel accomplished for having moved up the ladder so soon!
Enough of this chit-chat, though. Back to work.
-> END

=== level_three ===
You've made it to the third floor!
You've come so far. Did you know that you father is just upstairs?
He'd be very proud to see you!
Anyway, enough of my non-formality. Continue to do your work.
Press onwards.
-> END
