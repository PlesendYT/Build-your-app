## Screens for Mystery Visual Novel

screen notebook():
    frame:
        xalign 0.5
        yalign 0.5
        xsize 600
        ysize 400
        vbox:
            text "Case Notes" size 28
            null height 10
            text "Clues Found: [len(clues_found)]"
            null height 10
            for clue in clues_found:
                text "- [clue]"
            null height 20
            textbutton "Close" action Hide("notebook")

screen interrogation_choices():
    frame:
        xalign 0.5
        yalign 0.9
        vbox:
            text "What do you ask?"
            null height 10
            textbutton "Ask about alibi" action Return("alibi")
            textbutton "Ask about motive" action Return("motive")
            textbutton "Present a clue" action Return("clue")
            textbutton "End interview" action Return("end")

screen accusation_screen():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Who is the culprit?" size 24
            null height 20
            textbutton "Mrs. Blackwood" action Return("Mrs. Blackwood")
            textbutton "Mr. Sterling" action Return("Mr. Sterling")
            textbutton "Ms. Chen" action Return("Ms. Chen")
            textbutton "Dr. Reeves" action Return("Dr. Reeves")
