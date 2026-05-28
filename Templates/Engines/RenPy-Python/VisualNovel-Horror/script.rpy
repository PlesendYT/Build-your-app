## Script for Horror Visual Novel

label start:
    scene bg dark_room
    show screen survival_status
    narrator "I wake up in darkness. My head throbs."
    narrator "The last thing I remember... the hospital. Morgan was there."
    voice "Alex... Alex, are you awake?"
    protagonist "Morgan? Is that you?"
    voice "Stay quiet. Something is in the building with us."
    jump hallway

label hallway:
    scene bg hallway
    narrator "The hallway stretches into darkness. Flickering lights."
    $ flashlight_battery -= 5

    menu:
        "Go left toward the sound":
            $ sanity -= 5
            narrator "A door creaks. I should not have come this way."
            jump monster_encounter
        "Go right toward the exit sign":
            narrator "The exit sign flickers. Hope."
            jump safe_room
        "Call out for Morgan":
            $ sanity -= 10
            voice "SHHH! I said be quiet!"
            jump monster_encounter

label monster_encounter:
    scene bg hallway_dark
    narrator "Something moves in the shadows."
    $ monster_encountered += 1
    $ sanity -= 20
    narrator "A grotesque figure. Too tall. Limbs at wrong angles."
    narrator "I run. I don't look back."

    menu:
        "Hide in the closet":
            narrator "I squeeze into the dark closet. Holding my breath."
            narrator "Footsteps pass. Heavy. Wrong."
            narrator "...it's gone."
            $ sanity += 10
        "Fight back":
            narrator "I grab a pipe and swing."
            narrator "It shrieks and disappears into the wall."
            narrator "Did I hurt it? I don't know."
            $ sanity += 5
        "Run for the exit":
            narrator "I sprint. The door is right there!"

    jump safe_room

label safe_room:
    scene bg room
    narrator "I find a room. Lockable. Safe. For now."
    voice "Alex? Are you in there?"
    protagonist "Morgan! Where are you?"
    voice "I'm in the room next door. I found something important."
    voice "The thing... it's not a monster. It's a experiment."
    voice "We need to reach the basement and destroy the core."

    call screen safe_room_choices
    $ choice = _return

    if choice == "search":
        narrator "I search the room."
        if not has_key:
            $ has_key = True
            narrator "I find a key! It looks like it fits the basement door."
        else:
            narrator "Nothing useful here."
    elif choice == "door":
        narrator "I check the door. Locked. Something scratches on the other side."
        $ sanity -= 10
    elif choice == "rest":
        narrator "I close my eyes for a moment."
        $ sanity += 15
        narrator "Just a moment... I feel a bit better."
    elif choice == "listen":
        narrator "I press my ear to the wall."
        narrator "I hear Morgan's breathing. And... something else."
        voice "Alex. When we get out of here..."
        voice "I need to tell you something. About why we're here."
        narrator "The truth will have to wait."

    if sanity <= 0:
        jump madness_ending
    elif has_key and door_opened:
        jump basement
    else:
        jump hallway

label madness_ending:
    narrator "The darkness surrounds me. I can't tell what's real anymore."
    narrator "The voices... they're everywhere."
    narrator "I see Morgan's face, but it twists into something terrible."
    narrator "I close my eyes and let the void take me."
    narrator "..."
    narrator "Ending: Lost to the Darkness"
    return

label basement:
    scene bg basement
    narrator "The basement is the heart of the nightmare."
    narrator "Massive machinery pulses with unnatural energy."
    show friend
    friend "Alex! You made it. The core - we need to overload it."
    friend "But it will release everything trapped here."
    friend "Including... me."
    protagonist "What do you mean?"
    friend "I'm not Morgan. Not anymore. The experiment bonded us."
    friend "If you destroy the core, I go with it."
    friend "It's the only way to end this."

    menu:
        "Destroy the core - save everyone else":
            narrator "I pull the lever. The machinery screams."
            narrator "Morgan... my friend... smiles at me one last time."
            narrator "Light consumes everything."
            narrator "I wake up in a hospital bed. The nurses say I was in a coma."
            narrator "There was no monster. No experiment."
            narrator "But I know the truth."
            narrator "Ending: Freedom"
        "Refuse - find another way":
            narrator "I can't do it. I won't lose Morgan."
            narrator "The machinery continues to pulse."
            narrator "The nightmare continues. Forever."
            narrator "Ending: The Loop"
        "Destroy myself too":
            narrator "If Morgan goes, I go with them."
            narrator "I throw myself into the core."
            narrator "The last thing I feel is Morgan's hand in mine."
            narrator "Ending: Together"

    return
