VAR debug = true
VAR activationNumber = 0

EXTERNAL XT_DamagePlayer(x, y)

==start
The beginning!
->DONE

==testString
Hahahahha!

Many lines.
->DONE

==testPreOptionsText
So there will be options soon.
Get ready lol.
->DONE
==testOptions
You have reached a place of options! Aaa!

+ [Pick 1]

+ [Pick 2]

+ [Pick 3]

+ [Pick 4]

+ [Pick 5]

+ [Pick 6]

- Well that was fun. Now die.
{DamagePlayer(1)}
+ [OOps]
->DONE

==function DamagePlayer (amount)
{XT_DamagePlayer(amount, activationNumber)}
~activationNumber++
{debug: [Damaging player for {amount}!]}

==function XT_DamagePlayer(amount, actNr)
[Damage player for {amount}!]

