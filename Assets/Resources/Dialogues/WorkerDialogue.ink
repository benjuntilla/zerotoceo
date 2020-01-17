# worker
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
{~Welcome to BizTek, recruit! -> END|Did you know that we are the third biggest business technology producer in the country? -> END} 

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

=== level_three ===
{~I like your shirt. -> END|Wanna get some refreshments after work? -> END|Looking good. -> END}
