## Script for Kinetic Novel
## A linear story - no choices, pure narrative

label start:
    scene bg night_sky
    show screen kinetic_ui
    narrator "Some stories don't need choices."
    narrator "They just need to be told."
    narrator "This is one of them."
    narrator "..."
    narrator "Her name was Iris. She painted the stars."
    narrator "His name was Leo. He built bridges."
    narrator "They met on a train that was going nowhere special."
    narrator "But that's where the special things always happen."
    $ renpy.pause(1.0)
    jump chapter1

label chapter1:
    scene bg train
    show io
    io "Is this seat taken?"
    narrator "She asked, even though the train was empty."
    narrator "Leo looked up from his book."
    show leo
    leo "No, go ahead."
    narrator "Iris sat down, and the world shifted slightly."
    narrator "Not in a way you'd notice. But in a way you'd feel."
    narrator "They didn't speak for the first hour."
    narrator "Two strangers, sharing silence on a moving train."
    narrator "Then Iris laughed at something in her own head."
    io "Sorry, I just... thought of something funny."
    leo "What?"
    io "I don't remember now. That's the tragedy of good jokes."
    narrator "Leo smiled. It was a small smile."
    narrator "But it was real."
    $ renpy.pause(1.0)
    jump chapter2

label chapter2:
    scene bg station
    narrator "They got off at the same station by coincidence."
    narrator "Or maybe not. The universe has a sense of humor."
    io "This is my stop too."
    leo "Where are you headed?"
    io "Nowhere. Just... walking."
    leo "Me too. Want to walk together?"
    narrator "Iris considered this for exactly half a second."
    io "Okay."
    narrator "They walked through the old town."
    narrator "Past bakeries that smelled like childhood."
    narrator "Past bookstores that promised adventure."
    narrator "Past a fountain where children made wishes."
    io "What would you wish for?"
    leo "If I told you, it wouldn't come true."
    io "I think some wishes come true precisely because you say them out loud."
    narrator "Leo thought about this."
    leo "I'd wish for more days like this one."
    $ renpy.pause(1.0)
    jump chapter3

label chapter3:
    scene bg sunset
    narrator "They reached the hill at sunset."
    narrator "The sky was on fire with colors that don't have names."
    io "I paint the stars, but I could never paint this."
    leo "You paint stars?"
    io "It's what I do. I'm an artist."
    leo "I build bridges. Real ones, not metaphorical."
    io "That's beautiful. Connecting things."
    leo "I've never thought of it that way."
    narrator "They sat in comfortable silence."
    narrator "The kind of silence that feels like conversation."
    io "Leo?"
    leo "Yes?"
    io "Thank you for today."
    leo "Thank you for sitting next to me."
    narrator "And that was that."
    narrator "Some stories don't need grand endings."
    narrator "Some stories just need a beginning."
    narrator "And two people brave enough to say hello."
    narrator "..."
    narrator "The End"
    narrator "Thank you for reading."

    return
