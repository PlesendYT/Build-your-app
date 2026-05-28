## Script for Choice Game
## Every choice matters

label start:
    scene bg crossroads
    show screen stats_panel
    narrator "You stand at a crossroads. Not a physical one, but a moment."
    narrator "Four doors stand before you. Each leads to a different destiny."
    narrator "Choose wisely. Your path begins now."

    call screen multi_choice(
        "Which door do you enter?",
        "The Red Door (Courage)",
        "The Blue Door (Kindness)",
        "The Green Door (Cunning)",
        "The Gray Door (Unknown)"
    )
    $ choice = _return

    if choice == "a":
        $ courage += 10
        $ path = "warrior"
        jump warrior_path
    elif choice == "b":
        $ kindness += 10
        $ path = "healer"
        jump healer_path
    elif choice == "c":
        $ cunning += 10
        $ path = "rogue"
        jump rogue_path
    else:
        $ path = "mystery"
        jump mystery_path

label warrior_path:
    scene bg forest
    narrator "The red door opens to a vast forest."
    narrator "A beast blocks the path ahead."

    call screen multi_choice(
        "The beast snarls. What do you do?",
        "Draw your sword and fight",
        "Try to tame the beast",
        "Find a way around",
        "Stand your ground silently"
    )
    $ choice = _return

    if choice == "a":
        $ courage += 15
        narrator "You charge. The beast is fierce, but your will is fiercer."
        narrator "After a long battle, you emerge victorious."
        narrator "The forest recognizes your strength."
    elif choice == "b":
        $ kindness += 10
        $ courage += 5
        narrator "You extend your hand. The beast sniffs, then settles."
        narrator "It becomes your companion."
    elif choice == "c":
        $ cunning += 10
        narrator "You find a hidden path through the caves."
        narrator "It's dark, but you make it through."
    else:
        $ courage += 5
        narrator "You don't flinch. The beast senses your resolve and leaves."

    narrator "Your path continues..."
    jump final_test

label healer_path:
    scene bg village
    narrator "The blue door opens to a village in need."
    narrator "The people are suffering from a strange illness."

    call screen multi_choice(
        "How do you help?",
        "Use your healing knowledge",
        "Gather rare herbs from the mountains",
        "Organize the villagers to work together",
        "Seek help from a nearby city"
    )
    $ choice = _return

    if choice == "a":
        $ kindness += 15
        narrator "Your knowledge saves many lives."
        narrator "The village elder blesses you."
    elif choice == "b":
        $ courage += 10
        $ kindness += 5
        narrator "The mountains are dangerous, but you find the herbs."
        narrator "You return as a hero."
    elif choice == "c":
        $ cunning += 10
        $ kindness += 5
        narrator "Together, the villagers overcome the crisis."
    else:
        $ cunning += 10
        narrator "The city sends aid. The village recovers."

    narrator "Your path continues..."
    jump final_test

label rogue_path:
    scene bg city
    narrator "The green door opens to a bustling city."
    narrator "A shadowy figure approaches with a proposal."

    call screen multi_choice(
        "The figure offers you a job. What do you do?",
        "Accept the heist",
        "Refuse and report them",
        "Negotiate for a better deal",
        "Follow them to learn more"
    )
    $ choice = _return

    if choice == "a":
        $ cunning += 15
        narrator "The heist is risky, but your planning is immaculate."
        narrator "You succeed and earn a fortune."
    elif choice == "b":
        $ kindness += 10
        narrator "The guards arrest the figure. You did the right thing."
    elif choice == "c":
        $ cunning += 10
        narrator "You negotiate a share. The job pays well."
    else:
        $ courage += 10
        narrator "You discover a larger conspiracy. The figure is just a pawn."

    narrator "Your path continues..."
    jump final_test

label mystery_path:
    scene bg cave
    narrator "The gray door opens to darkness."
    narrator "A voice echoes: 'You chose the unknown. Why?'"

    call screen multi_choice(
        "How do you answer?",
        "Because I seek truth",
        "Because I fear nothing",
        "Because I trust the journey",
        "Because I had no other choice"
    )
    $ choice = _return

    if choice == "a":
        $ wisdom_points = 10
        narrator "Truth is the rarest treasure. The voice is pleased."
    elif choice == "b":
        $ courage += 10
        narrator "Fearlessness is a gift and a curse."
    elif choice == "c":
        $ kindness += 10
        narrator "Trust is the foundation of all great journeys."
    else:
        $ cunning += 10
        narrator "Honesty. Unexpected, but respected."

    narrator "A path forward illuminates."
    narrator "Your path continues..."
    jump final_test

label final_test:
    scene bg temple
    narrator "All paths converge at the Temple of Decisions."
    narrator "A final challenge awaits."

    if courage >= 25:
        narrator "Your courage has forged a warrior's spirit."
        narrator "The temple recognizes your strength."
        narrator "You are granted the title: The Brave."
    elif kindness >= 25:
        narrator "Your kindness has touched many lives."
        narrator "The temple recognizes your compassion."
        narrator "You are granted the title: The Compassionate."
    elif cunning >= 25:
        narrator "Your cunning has opened many doors."
        narrator "The temple recognizes your wisdom."
        narrator "You are granted the title: The Clever."
    else:
        narrator "You walked your own path, guided by intuition."
        narrator "The temple recognizes your uniqueness."
        narrator "You are granted the title: The Free."

    narrator "Every choice led you here. This is your story."
    narrator "And your story is just beginning."
    narrator "Thank you for playing."

    call screen stats_panel
    $ renpy.pause(2.0)
    return
