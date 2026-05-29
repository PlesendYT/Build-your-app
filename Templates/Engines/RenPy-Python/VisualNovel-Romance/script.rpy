## Script for Romance Visual Novel
# Romance story: affection levels for Elena, Marco, and Lea determine the ending

label start:
    scene bg city
    show screen relationship_status
    narrator "Some meetings change your life forever."
    narrator "It was a rainy Tuesday when I walked into that coffee shop..."
    jump chapter1

label chapter1:
    scene bg coffeeshop
    narrator "The bell above the door chimed. And there she was."
    show elena
    elena "Oh! I'm sorry, I didn't see you there."
    player "No worries. You dropped this."
    narrator "I handed her the book she'd left on the counter."
    elena "My favorite novel! Thank you so much."
    elena "I'm Elena, by the way."
    player "Jamie. Nice to meet you, Elena."

    menu:
        "Ask about the book":
            $ elena_affection += 10
            elena "It's a love story. Set in Paris."
            elena "Do you like romance novels?"
            menu:
                "Love them":
                    $ elena_affection += 5
                    elena "I knew you had good taste!"
                "Not really":
                    $ elena_affection += 0
                    elena "That's okay. Different strokes."
        "Compliment her smile":
            $ elena_affection += 8
            elena "Oh! Thank you, Jamie. That's very sweet."
        "Keep it professional":
            player "Well, enjoy your book."
            elena "Wait - can I get your number? For the book?"

    narrator "We exchanged numbers. I walked home with a smile."
    $ chapter = 2
    jump chapter2

label chapter2:
    scene bg park
    narrator "A few days later, I met Marco at the park."
    show marco
    marco "Jamie! Over here!"
    player "Marco! You look great."
    narrator "Marco and I have been friends for years."
    narrator "But lately... something has changed."

    menu:
        "Hug him warmly":
            $ marco_affection += 10
            marco "Wow, someone's happy to see me."
            player "Of course I am. It's been too long."
        "Keep distance":
            $ marco_affection += 2
            marco "Everything okay? You seem distant."
            player "Yeah, just tired."

    marco "I missed you, Jamie. I mean it."
    player "I missed you too, Marco."

    menu:
        "Tell him how you feel":
            $ marco_affection += 15
            player "Marco... I think I have feelings for you."
            marco "...Jamie. I've wanted to say the same thing."
            narrator "The world stopped. He felt it too."
        "Change the subject":
            player "So, how's work?"
            marco "...It's fine. Hey, are you free this weekend?"
            $ marco_affection += 3

    narrator "My heart is confused. Elena or Marco?"
    $ chapter = 3
    jump chapter3

label chapter3:
    scene bg library
    narrator "Lea wasn't someone I expected to meet."
    show lea
    lea "This seat taken?"
    player "Go for it."
    narrator "She had an intensity that drew me in."

    menu:
        "Start a conversation":
            $ lea_affection += 10
            lea "You're reading Murakami. Interesting choice."
            player "You know Murakami?"
            lea "I wrote my thesis on him."
        "Focus on your book":
            $ lea_affection += 2
            lea "Quiet type. I respect that."

    if lea_affection >= 10:
        lea "Jamie... I don't usually do this, but..."
        lea "Would you like to grab coffee sometime?"
        player "I'd love to."
        narrator "Another possibility. Another path."
    else:
        narrator "Lea went back to her reading. Maybe another time."

    jump chapter4

label chapter4:
    scene bg rooftop
    narrator "The city lights twinkle below. I need to make a choice."
    narrator "Three people. Three possibilities. One heart."

    if elena_affection >= marco_affection and elena_affection >= lea_affection:
        show elena
        elena "Jamie! I was hoping to find you here."
        player "Elena. I've been thinking about you."
        elena "I can't stop thinking about that day in the coffee shop."
        elena "It sounds crazy, but... I think this is the start of something."
        narrator "Elena takes my hand. It feels right."
        narrator "Ending: A New Chapter with Elena"
    elif marco_affection >= elena_affection and marco_affection >= lea_affection:
        show marco
        marco "There you are. I've been looking everywhere."
        player "Marco... I need to tell you something."
        marco "Let me go first. I love you, Jamie. I've always loved you."
        narrator "The words I'd been waiting to hear."
        narrator "Ending: Love That Was Always There"
    elif lea_affection >= elena_affection and lea_affection >= marco_affection:
        show lea
        lea "The city looks beautiful from up here."
        player "Lea. I'm glad you came."
        lea "I don't do this. Open up, I mean."
        lea "But with you, it feels natural."
        narrator "Sometimes love finds you when you least expect it."
        narrator "Ending: An Unexpected Story"
    else:
        narrator "Maybe I'm not ready for love yet."
        narrator "And that's okay. The most important relationship is with myself."
        narrator "Ending: Self-Discovery"

    return
