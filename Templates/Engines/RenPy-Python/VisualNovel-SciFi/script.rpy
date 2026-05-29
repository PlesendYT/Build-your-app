## Script for Sci-Fi Visual Novel
# Sci-fi exploration story: choices affect ship stats, tech level, and alien trust

label start:
    scene bg bridge
    show screen ship_status
    ai "All systems nominal, Captain. We have arrived at the Zeta Reticuli system."
    captain "This is what we came for. First contact."
    narrator "The year is 2347. Humanity's first interstellar voyage."

    jump exploration

label exploration:
    call screen exploration_choice
    $ choice = _return

    if choice == "station":
        jump station_scene
    elif choice == "planet":
        jump planet_scene
    elif choice == "asteroids":
        jump asteroid_scene

label station_scene:
    scene bg station
    narrator "The derelict station drifts silently."
    ai "Life support offline for 200 years. No life signs."

    menu:
        "Send a boarding party":
            show officer
            officer "I'll lead the team. We'll find valuable technology."
            $ tech_level += 1
            narrator "They recover advanced power cells."
            $ ship_hull += 10
            $ fuel += 15
        "Scan from a distance":
            show officer
            officer "Sensors detected something moving inside..."
            narrator "A signal! A distress beacon from a trapped alien."
            $ alien_trust += 10

    jump encounter

label planet_scene:
    scene bg planet
    narrator "The planet teems with alien life."
    show alien
    alien "You have entered the territory of the Xyl'thar."
    alien "State your intentions."

    menu:
        "We come in peace":
            captain "We mean no harm. We seek knowledge."
            alien "Knowledge can be shared... for a price."
            $ alien_trust += 15
            $ tech_level += 2
        "We claim this planet":
            captain "Under the authority of the United Earth Federation."
            alien "Then you will be met with force."
            $ ship_hull -= 20
            narrator "The ship takes damage as you flee."

    jump encounter

label asteroid_scene:
    scene bg asteroids
    narrator "The asteroid field is treacherous but rich in minerals."
    ai "Scanning for resources..."

    menu:
        "Mine the asteroids":
            show engineer
            engineer "I can extract fuel from those ice asteroids."
            $ fuel += 30
            $ crew_morale += 5
            narrator "A successful mining operation."
        "Search for wreckage":
            narrator "You find the wreck of an ancient alien vessel."
            $ tech_level += 3
            narrator "Technology far beyond human understanding."

    jump encounter

label encounter:
    narrator "A massive alien vessel appears from hyperspace."
    show alien
    alien "Captain Nova. We have observed your journey."
    alien "Humanity has potential. But you face a choice."

    menu:
        "Request an alliance":
            captain "We wish to learn from you. To coexist."
            alien "Then an alliance it shall be. Welcome to the Galactic Council."
            if alien_trust >= 10:
                narrator "Your peaceful approach has earned humanity a place among the stars."
                narrator "A new era of cooperation begins."
            else:
                narrator "The Xyl'thar are skeptical but agree to an observer status."
        "Declare humanity's independence":
            captain "We stand alone. We need no masters."
            alien "So be it. But you will face the coming darkness without allies."
            narrator "Humanity chooses solitude. The path ahead will be difficult."
            narrator "But your spirit remains unbroken."

    narrator "Thus ends the first chapter of humanity's journey into the cosmos."
    narrator "The future is unwritten..."

    return
