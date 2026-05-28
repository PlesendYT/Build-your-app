## Script for Comedy Visual Novel

label start:
    scene bg campus
    show screen chaos_meter
    narrator "Welcome to Middleton University - where the coffee is terrible and the students are questionable."
    show ben
    ben "Charlie! You won't believe what happened!"
    player "You finally realized your 'miracle hair growth formula' was just mayonnaise?"
    ben "...that explains a lot actually."
    $ chaos_level += 5
    narrator "And so begins another ridiculous day."
    jump class

label class:
    scene bg classroom
    show professor
    professor "Today we will discuss... the meaning of existence."
    professor "Mr. Charlie. What do you believe is the meaning of life?"

    call screen comedy_choice
    $ choice = _return

    if choice == "joke":
        player "42, professor. Obviously."
        $ joke_counter += 1
        professor "...That's not original, Mr. Charlie."
        player "The original answer wasn't original either."
        $ chaos_level += 10
    elif choice == "absurd":
        player "I believe we're all just characters in a visual novel made by someone named RenPy."
        professor "That's... disturbingly meta."
        $ chaos_level += 15
    else:
        player "To be kind, to learn, and to find happiness."
        professor "An unexpectedly mature answer, Mr. Charlie."
        $ chaos_level -= 5

    narrator "Class is interrupted by a loud crash."
    jump hallway

label hallway:
    scene bg hallway
    show mia
    mia "CHARLIE! You have to see this!"
    player "Mia, what now?"
    mia "Someone replaced all the cafeteria food with rubber replicas!"
    player "...who has the time and skill to do that?"
    mia "I'm not saying it was me, but it was definitely me."

    menu:
        "Help Mia with her prank":
            mia "YES! I knew I could count on you!"
            narrator "Together, you replace the dean's office furniture with inflatable versions."
            $ chaos_level += 20
            narrator "The dean sits down. The chair deflates. Comedy gold."
        "Report her to the professor":
            player "Mia, I can't be part of this."
            mia "Traitor! I'll remember this!"
            $ chaos_level -= 5
            narrator "Mia is sentenced to write 'I will not cause chaos' 100 times."
            narrator "She writes it in invisible ink. Of course."

    jump cafeteria

label cafeteria:
    scene bg cafeteria
    narrator "The cafeteria is in shambles. Rubber food everywhere."
    show ben
    ben "Charlie! Mia! Over here!"
    ben "I've discovered something amazing."
    player "What now?"
    ben "If you microwave these rubber steaks, they smell EXACTLY like real steak!"
    ben "But you can't eat them! It's the perfect crime!"

    if chaos_level >= 50:
        narrator "The dean storms in, furious about the inflatable furniture."
        narrator "But when he tries to yell, his voice comes out as a squeak."
        narrator "Someone replaced his microphone with a squeaky toy."
        narrator "Everyone loses it. Absolute chaos."
        narrator "The dean eventually laughs too. It's that kind of school."
        narrator "Ending: Maximum Chaos Achieved"
    elif joke_counter >= 3:
        narrator "Ben's rubber steak experiment works a little too well."
        narrator "A dog runs in, grabs the 'steak', and a chase ensues."
        narrator "The dog is the dean's. The dean is not amused."
        narrator "But secretly, even he cracks a smile."
        narrator "Ending: Comedy Gold"
    else:
        narrator "The day ends quietly. Relatively speaking."
        narrator "Mia is planning her next prank. Ben is microwaving things."
        narrator "And me? I wouldn't have it any other way."
        narrator "Ending: Another Day at Middleton"

    return
