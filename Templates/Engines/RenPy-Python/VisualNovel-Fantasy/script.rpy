## Script for Fantasy Visual Novel

label start:
    scene bg forest
    narrator "The kingdom of Aetheria is under threat. The Shadow King rises again."
    show sage
    sage "Kaelen, you are the last hope of our people."
    sage "The ancient prophecy speaks of one who will unite the realms."
    narrator "Your journey begins now."

    call screen companion_select
    $ companion = _return

    if companion == "luna":
        show luna
        luna "I've been waiting for you, Kaelen. The stars have aligned."
        $ luna_trust += 10
    else:
        show thorn
        thorn "Ready to spill some blood? Let's move."
        $ thorn_trust += 10

    jump chapter1

label chapter1:
    scene bg mountain
    narrator "Your first challenge: the Serpent's Pass."

    menu:
        "Fight the giant serpent head-on":
            $ bravery += 5
            if companion == "thorn":
                thorn "Now that's what I like to hear!"
                narrator "Together, you and Thorn defeat the serpent."
            else:
                narrator "Luna uses a barrier spell while you strike."
                $ luna_trust += 5
        "Find a clever way around":
            $ wisdom += 5
            if companion == "luna":
                luna "There's an ancient path only visible by moonlight."
                narrator "Luna's knowledge guides you safely through."
                $ luna_trust += 5
            else:
                thorn "Hmph. Smart, I guess."
                $ thorn_trust += 3
        "Use magic to freeze the serpent":
            $ magic += 5
            narrator "Your ice spell holds the serpent long enough."

    narrator "The pass is cleared. But ahead lies the Enchanted Forest."
    jump chapter2

label chapter2:
    scene bg forest_magic
    narrator "The forest is alive with ancient magic."
    narrator "A mysterious figure blocks your path."

    menu:
        "Challenge them to a duel of wits":
            $ wisdom += 5
            narrator "You solve their riddle and earn passage."
        "Attack without hesitation":
            $ bravery += 5
            narrator "The figure dissolves into mist. The path is clear."
        "Attempt to communicate peacefully":
            $ magic += 5
            narrator "You sense they are a guardian, not an enemy."

    narrator "You grow stronger with each challenge."
    jump chapter3

label chapter3:
    scene bg castle
    narrator "The Shadow King's fortress looms before you."

    if bravery >= 15 and wisdom >= 15 and magic >= 15:
        narrator "You have mastered all three virtues."
        narrator "Kaelen, the prophesied hero, faces the Shadow King."
        narrator "With your companions at your side, you unleash the power of Aetheria."
        narrator "Light banishes shadow. The kingdom is saved!"
        narrator "A new era of peace begins."
    elif bravery >= 10:
        narrator "Your courage is unmatched. You charge the fortress."
        narrator "The Shadow King falls to your blade, but at great cost."
        narrator "You are victorious, but scarred."
    elif wisdom >= 10:
        narrator "You outsmart the Shadow King's defenses."
        narrator "A clever trap seals him away for another thousand years."
        narrator "Wisdom prevails over brute force."
    elif magic >= 10:
        narrator "Your magical power erupts, shattering the fortress walls."
        narrator "The Shadow King is consumed by his own darkness."
        narrator "Magic has saved the realm."
    else:
        narrator "You fight bravely, but you are not yet ready."
        narrator "The Shadow King overpowers you."
        narrator "Kaelen... the realm will remember your sacrifice."
        narrator "Another hero will rise."

    return
