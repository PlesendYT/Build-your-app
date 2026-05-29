## Script for Mystery Visual Novel
# Detective mystery: gather clues, interview four suspects, and make the correct accusation

label start:
    scene bg manor
    detective "The Sterling Manor case. A wealthy collector found dead in his study."
    detective "The only thing stolen: a priceless jade statue."
    detective "Four suspects, one killer. Let's find the truth."
    jump investigation

label investigation:
    scene bg study
    detective "The study is preserved exactly as it was found."

    $ clues_found.append("Broken glass near the window")
    $ clues_found.append("A muddy footprint - size 8 shoe")

    detective "I can see broken glass near the window, and a muddy footprint underneath."
    menu:
        "Examine the desk":
            $ clues_found.append("A hidden safe - slightly ajar, empty")
            detective "The safe behind the painting is open. Empty."
        "Examine the body position":
            $ clues_found.append("Body position suggests victim knew attacker")
            detective "The victim fell backwards - he was facing whoever did this."

    narrator "Time to interview the suspects."
    jump interview_blackwood

label interview_blackwood:
    scene bg parlor
    show suspect1
    suspect1 "I had nothing to do with this! Henry and I were... close."
    detective "Mrs. Blackwood, where were you at 9 PM?"

    call screen interrogation_choices
    $ choice = _return

    if choice == "alibi":
        suspect1 "I was in the garden. The roses needed tending."
        $ clues_found.append("Mrs. Blackwood's alibi: garden, no witness")
        detective "The garden is on the opposite side of the manor."
    elif choice == "motive":
        suspect1 "Henry was going to cut me out of his will. But I didn't kill him!"
        $ clues_found.append("Mrs. Blackwood: financial motive")
    elif choice == "clue":
        detective "Does this footprint look familiar?"
        suspect1 "I wear size 6, detective. Not a match."
    else:
        $ interviewed.append("Mrs. Blackwood")

    if "Mrs. Blackwood" not in interviewed:
        jump interview_blackwood

    jump interview_sterling

label interview_sterling:
    scene bg library
    show suspect2
    suspect2 "Look, I'm his business partner, not his murderer."
    detective "Mr. Sterling. The jade statue - you wanted it, didn't you?"

    call screen interrogation_choices
    $ choice = _return

    if choice == "alibi":
        suspect2 "Dinner at Chez Pierre. I have the receipt."
        $ clues_found.append("Mr. Sterling's alibi: restaurant receipt")
    elif choice == "motive":
        suspect2 "The statue was insured for millions. I'd be a fool to steal it."
        detective "Unless the insurance fraud was the real motive..."
    elif choice == "clue":
        detective "The safe was opened with a combination. Who knew it?"
        suspect2 "I did. But so did Ms. Chen - she was his assistant."
    else:
        $ interviewed.append("Mr. Sterling")

    if "Mr. Sterling" not in interviewed:
        jump interview_sterling

    jump interview_chen

label interview_chen:
    scene bg office
    show suspect3
    suspect3 "I've been expecting you, detective."
    detective "Ms. Chen. You were his assistant for how long?"

    call screen interrogation_choices
    $ choice = _return

    if choice == "alibi":
        suspect3 "I was working late. Alone. No witnesses."
        detective "Convenient."
    elif choice == "motive":
        suspect3 "He was going to fire me. I needed the severance, but not enough to kill."
        $ clues_found.append("Ms. Chen: about to be fired")
    elif choice == "clue":
        detective "The muddy footprint outside the window. It leads toward your apartment."
        suspect3 "...I took a walk that night. Through the garden."
    else:
        $ interviewed.append("Ms. Chen")

    if "Ms. Chen" not in interviewed:
        jump interview_chen

    jump interview_reeves

label interview_reeves:
    scene bg lab
    show suspect4
    suspect4 "Ah, the detective. I've been analyzing the evidence myself."
    detective "Dr. Reeves. You were the family physician."

    call screen interrogation_choices
    $ choice = _return

    if choice == "alibi":
        suspect4 "I was at the hospital. Emergency surgery. Dozens of witnesses."
        $ clues_found.append("Dr. Reeves has a solid alibi")
    elif choice == "motive":
        suspect4 "Henry owed me money. A significant sum. But dead men pay no debts."
    elif choice == "clue":
        suspect4 "The glass wasn't broken from outside. Look at the angle of the cracks."
        detective "He's right. The glass was broken from inside."
    else:
        $ interviewed.append("Dr. Reeves")

    if "Dr. Reeves" not in interviewed:
        jump interview_reeves

    jump deduction

label deduction:
    detective "I've gathered all the evidence. Time to make an accusation."
    call screen accusation_screen
    $ accusation = _return

    if accusation == "Ms. Chen":
        detective "Ms. Chen... the glass was broken from inside to fake a break-in."
        detective "You knew the combination to the safe. And your 'walk' that night..."
        detective "You took the statue, broke the glass from within, and left through the window."
        suspect3 "...I needed the money. He was going to fire me."
        suspect3 "I'm sorry."
        narrator "Case solved. Ms. Chen was arrested for the theft of the jade statue."
        narrator "Murder charges were added when the autopsy revealed poisoning."
        narrator "Justice prevails once more."
    else:
        suspect3 "I'm afraid you're wrong, detective."
        suspect3 "I have proof I was elsewhere the entire night."
        detective "...I need to re-examine the evidence."
        narrator "The wrong accusation was made. The case remains open."
        narrator "Perhaps the truth will come out someday."

    return
