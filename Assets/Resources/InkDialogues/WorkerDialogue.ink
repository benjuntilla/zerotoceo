# worker
INCLUDE Generic.ink

=== start ===
~ xp = GetPlayerXP()
{ GetGameLevel():
- 1: -> level_one
- 2: -> level_two
- else: -> END
}

=== level_one ===
{~Welcome to BusinessTek, recruit! -> END|Did you know that <fact> -> END} 

=== level_two ===
{~I like your shirt. -> END|Wanna get some refreshments after work? -> END|Looking good. -> END|-> question}
= question
{
    - !question1:
        -> question1
    - !question2:
        -> question2
}
- (question1) question1
    * [correct]
        ~ xp += 10
        ok.
    * [incorrect]
        ~ xp -= 10
        ok.
    - -> END
- (question2) question2
    * [correct]
        ~ xp += 10
        ok.
    * [incorrect]
        ~ xp -= 10
        ok.
    - -> END
