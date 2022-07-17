// Story!

==introtalk
{StartNarrative()}
->GetNextDiceTarget->
Today is the day, Avery. I’m going to ask them out.

I’m going to roll the dice, and I’m going to stop being such a coward.
->DONE
=t2_d1
//Friend is t2, player is t1
Wait a second. Are those actual dice in your pocket?

Oh my god.

No, Barbara. Just…no..
->DONE
=t1_d2
What? I said I was going to roll the dice!

If I don’t do it this way, I’ll never be brave enough.

With the dice, I’ll be forced to ask.
->DONE
=t2_d2
Have you even decided which one to ask yet? 

It’s all “Charlie this, Quinn that.” I honestly can’t keep up.
->DONE

=t1_d3
That’s the beauty of it my friend. Fate will decide. Actually, let me show you…
~diceKnotTarget = "firstRollResult"
// I will hook it up here so it’ll roll a die once this is finished > it will then know to go to “firstRollResult” after it’s finished
->DONE

==firstRollResult
Fates of old, I beseech you in my quest. Who will be my date to the school dance this weekend? If odd: I will ask Quinn. If even, I will ask Charlie.

+ [{diceRollResult==2||diceRollResult==4||diceRollResult==6: {DisableButton()}}Quinn!]
//Literally unreachable, unless we change the roll. Guess we’re going with Charlie then?

+ [{diceRollResult==1||diceRollResult==3||diceRollResult==5: : {DisableButton()}}Charlie!]
 
- The fates have decided. Now, I have a date with destiny. Wish me luck!
* [Begin thy quest.]
->DONE

==RandomPerson
// Some random barks from random people I guess
{!~Oh hey Barbara!|Yoooo, sick costume! It’s a costume, right?|You’re such a nerd.|Why are you always in my way?|Get out of my face, Baa-bara.|Dweeb.|Dork.|Geek.|Baaaaa!|Watch out, dork.|Where’s your lunch money?|I’m watching you.|I’m coming to get you, Barbara.|What’s that smell?|Do you hear that high-pitched whining noise? Oh, it’s you, nevermind.|I hear your mother is DTF.|Was that your dad hitting on the teacher in Home Room last week? Gross.|Oh…hey.|Have we met?|What was your name again?|Hi, I guess.|Can I help you?|Did you need something?|Are you always this quiet?|So…do you come here often?|What are you looking at?|You must be new.|Sorry, I’m in a rush.|You look lost.|Hey, how are you?}
->DONE

==averyChat
// You can talk to friend to get info about stuff
{averyChat<2:
What are you still doing here? Charlie’s probably in the gym.

End of the hall, ya dweeb.
->DONE
}
// to be updated as we go, if needed
{~Hey Barb.|Yo yo.|Maan I just wanna go play some DnD…}
->DONE

