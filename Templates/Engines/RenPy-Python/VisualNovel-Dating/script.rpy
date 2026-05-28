## Script for Dating Visual Novel

label start:
    show screen affection_display
    narrator "It's a new day in the city. Who will you meet?"

label new_day:
    narrator "Day [day] - Where should I go today?"
    call screen location_choice
    $ location = _return

    if location == "cafe":
        call cafe_scene
    elif location == "park":
        call park_scene
    elif location == "library":
        call library_scene
    elif location == "beach":
        call beach_scene

    $ day += 1
    if day > 14:
        call ending_evaluation
    else:
        jump new_day

label cafe_scene:
    scene bg cafe
    narrator "The warm aroma of coffee fills the air."
    show alex happy
    alex "Hey! I was hoping I'd see you here."
    mc "Hi Alex! Mind if I join you?"
    menu:
        "Sit with Alex":
            $ alex_affection += 10
            alex "I'm glad you did. I always love our conversations."
            alex "How have you been?"
            menu:
                "Tell her about your day":
                    $ alex_affection += 5
                    alex "That sounds wonderful!"
                "Ask about her day":
                    $ alex_affection += 8
                    alex "You're so sweet to ask!"
        "Say you need to go":
            $ alex_affection -= 5
            alex "Oh... sure. Maybe next time."
    return

label park_scene:
    scene bg park
    narrator "The sun filters through the trees."
    show sam happy
    sam "Well hello there! Fancy meeting you here."
    mc "Sam! What a coincidence."
    menu:
        "Go for a walk together":
            $ sam_affection += 10
            sam "Great idea! I know a beautiful trail."
            sam "So tell me, what's something you've always wanted to do?"
            menu:
                "Travel the world":
                    $ sam_affection += 5
                    sam "Me too! We should go together sometime."
                "Learn an instrument":
                    $ sam_affection += 3
                    sam "That's cool. I admire that."
        "Just chat for a bit":
            $ sam_affection += 3
            sam "Sure, I have a few minutes."
    return

label library_scene:
    scene bg library
    narrator "The library is quiet and peaceful today."
    show jordan neutral
    jordan "Oh, hi. I didn't expect to see anyone here."
    mc "Hey Jordan. Find anything good?"
    menu:
        "Ask about the book they're reading":
            $ jordan_affection += 12
            jordan "Actually, yes! It's a fascinating novel about..."
            jordan "I could lend it to you when I'm done."
            mc "I'd like that."
        "Recommend a book":
            $ jordan_affection += 5
            jordan "I'll check it out. Thanks for the suggestion."
    return

label beach_scene:
    scene bg beach
    narrator "The waves crash gently against the shore."
    show riley happy
    riley "Hey there! Come to enjoy the view?"
    mc "Riley! I didn't know you liked the beach."
    menu:
        "Sit and watch the sunset":
            $ riley_affection += 10
            riley "Perfect. Sunsets are my favorite."
            riley "You know, moments like this... they're special."
        "Go for a swim":
            $ riley_affection += 8
            riley "Race you to the water!"
    return

label ending_evaluation:
    scene bg home
    narrator "Two weeks have passed. Your relationships have grown..."

    if alex_affection >= 80:
        $ ending = "Alex's Heart"
    elif sam_affection >= 80:
        $ ending = "Sam's Adventure"
    elif jordan_affection >= 80:
        $ ending = "Jordan's Story"
    elif riley_affection >= 80:
        $ ending = "Riley's Sunset"
    elif alex_affection >= 50 or sam_affection >= 50 or jordan_affection >= 50 or riley_affection >= 50:
        $ ending = "Good Friend"
    else:
        $ ending = "Alone"

    if ending == "Alex's Heart":
        show alex happy
        alex "I've been wanting to tell you something..."
        alex "I think I'm falling for you."
        mc "Alex... I feel the same way."
        narrator "You and Alex share a moment that will change everything."
    elif ending == "Sam's Adventure":
        show sam happy
        sam "So... about that trip I mentioned."
        sam "I booked us tickets. What do you say?"
        mc "Sam, that's amazing! Let's go!"
    elif ending == "Jordan's Story":
        show jordan happy
        jordan "I finished that novel I was telling you about."
        jordan "And I realized... the best stories are the ones we write together."
    elif ending == "Riley's Sunset":
        show riley happy
        riley "Every sunset is beautiful, but they're better with company."
        riley "Will you stay with me for the next one?"
    else:
        narrator "Sometimes the most important relationship is the one you build with yourself."
        narrator "And that's perfectly okay."

    call screen ending_screen(ending)

    return
