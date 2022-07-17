// Story!

==introtalk
{StartNarrative()}
->GetNextDiceTarget->
~diceKnotTarget = "firstRollResult"
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

==RandomBully
// Some random barks from random people I guess. Barks for days.
{!~You’re such a nerd.|Why are you always in my way?|Get out of my face.|Dweeb.|Dork.|Geek.|Watch out, dork.|Ugh.|Cringe.|Why can’t you just like…disappear or something.|Oh my god, what is wrong with you.|You’ve got something on…oh no, that’s just your face.}
->DONE
==RandomStranger
// Some random barks from random people I guess. Barks for days.
{!~Oh…hey.|Have we met?|What was your name again?|Hi, I guess.|Can I help you?|Did you need something?|Are you always this quiet?|So…do you come here often?|What are you looking at?|You must be new.|Sorry, I’m in a rush.|You look lost.|I hate math. Do you hate math? Of course you do, everyone hates math.}
->DONE
==RandomFriendly
// Some random barks from random people I guess. Barks for days.
{!~Oh hey Barbara!|Yoooo, sick costume! It’s a costume, right?|Hey, how are you?|What’s up?|Catch you at the gym later!|Radical game last night, thanks for the invite.|My liege.|Hi Barb. Still on for study group?|Watch out, I hear Jennifer is on the prowl.}
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

==skaterBoi
Heya.

Man, it's hard to get anything done in time.

I'm always so late with everything.

Oh well.
->DONE

==BullyChat
{StartNarrative()}
->GetNextDiceTarget->
Where exactly do you think you’re going, Nerd?

The gym's occupied, there's nothing for you there.

Why don't you go back to rolling dice or whatever, nerd?

~diceKnotTarget = "bullyRoll"

->DONE

==bullyRoll
Jennifer. My nemesis. You picked the wrong day to get on my nerves. Why do you always have to be such an ogre? If I get a three or a six, I will tell her a piece of my mind!

+ [{diceRollResult!=3&&diceRollResult!=6: {DisableButton()}}Fight back!]
// We might be able to do this later.
+ [{diceRollResult!=2&&diceRollResult!=5 : {DisableButton()}}Get away!]
 // Maybe later?
+ [{diceRollResult!=1&&diceRollResult!=4 : {DisableButton()}}Play dead!]
For some inexplicable reason, you decide to play dead. Within moments, as Jennifer’s laughter echoes through the hall, you find yourself...literally dying.
{DamagePlayer(1)}
- 

* [Umm, what just happened?]
->DONE

==heroicChat
The ogres have taken the castle - and the Prince!

The fates have given me the strength to retake it!

I will save you, Prince!

(Space to attack)
->DONE

==useDiceMainDoorPre
{StartNarrative()}
Hmph. You will never prevail. Only those blessed by the fates can pass.
~diceKnotTarget = "useDiceMainDoor"
Let's goooo!
->DONE

==useDiceMainDoor
{QueryDice()}
Only a Holy Six will grant me passage. The others may...grant me boons.

+ [{not hasDieNr1: {DisableButton()}} A one!]
{DestroyDie(1)}
Nothing.
+ [{not hasDieNr2: {DisableButton()}} A two.]
{DestroyDie(2)}
Nothing.
+ [{not hasDieNr3: {DisableButton()}} A three.]
{DestroyDie(3)}
Nothing.
+ [{not hasDieNr4: {DisableButton()}} A four..]
{DestroyDie(4)}
Nothing.
+ [{not hasDieNr5: {DisableButton()}} A five?]
{DestroyDie(5)}
Nothing.
+ [{not hasDieNr6: {DisableButton()}} A six!]
{DestroyDie(6)}
->success
+ [I have nothing.]
It pains me.
{DamagePlayer(1)}
->DONE
- 
+ [I must try again.]
->useDiceMainDoor

=success
#openMainDoor
Yes! I knew I could do it! I am coming, Prince!
+ [Onwards!]
->DONE

==getMoreHealth
#returnToLightWorld
I feel...stronger. It is time to return.
+ [Return to your world.]
->DONE


