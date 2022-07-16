VAR debug = true
VAR activationNumber = 0
VAR diceKnotTarget = ""
VAR nextDiceTarget = -1
VAR diceRollResult = -1

EXTERNAL XT_DamagePlayer(x,y)
EXTERNAL XT_StartNarrative()
EXTERNAL XT_StopNarrative()
EXTERNAL XT_RollDie(x,y,z)

==start
The beginning!
->DONE

==testString
Hahahahha!

Many lines.
->DONE

==testPreOptionsText
{StartNarrative()}
~diceKnotTarget = "testOptions"
->GetNextDiceTarget->
So there will be options soon.
Get ready lol.
->DONE
==testOptions
You have reached a place of options! Aaa!

+ [{diceRollResult!=1: {DisableButton()}}(Pick 1]

+ [{diceRollResult!=2: {DisableButton()}} Pick 2]

+ [{diceRollResult!=3: {DisableButton()}} Pick 3]

+ [{diceRollResult!=4: {DisableButton()}} Pick 4]

+ [{diceRollResult!=5: {DisableButton()}} Pick 5]

+ [{diceRollResult!=6: {DisableButton()}} Pick 6]

- Well that was fun.
+ [OOps]
->DONE

===GetNextDiceTarget===
{->SetNextDiceTarget(1)->|->SetNextDiceTarget(2)->|->SetNextDiceTarget(3)->|->SetNextDiceTarget(4)->|->SetNextDiceTarget(5)->|->SetNextDiceTarget(6)->|->SetNextDiceTarget(-1)->}
->->

=SetNextDiceTarget(newNumber)
~nextDiceTarget = newNumber
->->

==function DamagePlayer (amount)
{XT_DamagePlayer(amount, activationNumber)}
~activationNumber++
{debug: [Damaging player for {amount}!]}
==function StartNarrative()
{XT_StartNarrative()}
==function StopNarrative()
{XT_StopNarrative()}
==function RollDie(targetNumber, targetKnot)
// Use -1 for fully random. Targetnknot is the string we go to after
~activationNumber++
{XT_RollDie(targetNumber, targetKnot, activationNumber)}

==function XT_DamagePlayer(amount, actNr)
[Damage player for {amount}!]
==function XT_StartNarrative()
[Set to Narrative State]
==function XT_StopNarrative()
[Set to Game State]
==function XT_RollDie(targetNr, targetKnot, actNr)
[Try to roll the die with target number {targetNr}]


// gENERIC FUNCTIONS

===function UseButton(buttonName)===
<>{not debug:
\[useButton.{buttonName}]
}
===function DisableButton()===
<>{not debug:
\[disable\]
}
===function UseText(textName)===
<>{not debug:
\[useText.{textName}]
}

===function ReqS(currentAmount, requiredAmount, customString)===
// can be used for things that aren't resources - to be used in options! [{Req(stuffYouNeed, 10, "Stuffs")}!]
{currentAmount>=requiredAmount:<color=green>|<color=red>}
<>{not debug:
\[{currentAmount}/{requiredAmount}\ {customString}]</color>
- else:
({currentAmount}/{requiredAmount} {customString})</color>
}


=== function print_num(x) ===
// print_num(45) -> forty-five
{ 
    - x >= 1000:
        {print_num(x / 1000)} thousand { x mod 1000 > 0:{print_num(x mod 1000)}}
    - x >= 100:
        {print_num(x / 100)} hundred { x mod 100 > 0:and {print_num(x mod 100)}}
    - x == 0:
        zero
    - else:
        { x >= 20:
            { x / 10:
                - 2: twenty
                - 3: thirty
                - 4: forty
                - 5: fifty
                - 6: sixty
                - 7: seventy
                - 8: eighty
                - 9: ninety
            }
            { x mod 10 > 0:<>-<>}
        }
        { x < 10 || x > 20:
            { x mod 10:
                - 1: one
                - 2: two
                - 3: three
                - 4: four        
                - 5: five
                - 6: six
                - 7: seven
                - 8: eight
                - 9: nine
            }
        - else:     
            { x:
                - 10: ten
                - 11: eleven       
                - 12: twelve
                - 13: thirteen
                - 14: fourteen
                - 15: fifteen
                - 16: sixteen      
                - 17: seventeen
                - 18: eighteen
                - 19: nineteen
            }
        }
}

