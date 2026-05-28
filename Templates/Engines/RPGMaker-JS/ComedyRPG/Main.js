/*:
 * @target MZ
 * @plugindesc Comedy RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Comedy RPG with joke system, absurd events, and humor tracking.
 *
 * Plugin Commands:
 *   ComedyRPG tellJoke joke_id
 *   ComedyRPG absurdEvent event_id
 *   ComedyRPG humorStat 10
 *   ComedyRPG fourthWall break
 *
 * @command tellJoke
 * @text Tell Joke
 * @desc Trigger a specific joke
 * @arg jokeId
 * @text Joke ID
 * @type string
 */

(function() {
    const pluginName = 'ComedyRPG';

    $gameParty._humorLevel = $gameParty._humorLevel || 0;
    $gameParty._jokesTold = $gameParty._jokesTold || [];
    $gameParty._absurdity = $gameParty._absurdity || 0;

    // ---- Joke Database ----
    const jokes = {
        rpg: "Why did the RPG hero break up with his inventory? Too much baggage!",
        debug: "I'd tell you a UDP joke, but you might not get it.",
        programmer: "Why do programmers prefer dark mode? Because light attracts bugs!",
        meta: "Hey, you're playing an RPGMaker game. Inside an RPGMaker plugin. That I wrote. Meta enough?",
        pun: "I'm reading a book on anti-gravity. It's impossible to put down!",
    };

    Game_Party.prototype.tellJoke = function(jokeId) {
        const joke = jokes[jokeId];
        if (!joke || this._jokesTold.includes(jokeId)) return false;

        this._jokesTold.push(jokeId);
        this._humorLevel += 10;
        $gameMessage.add(joke);
        $gameMessage.add('...');
        $gameMessage.add('Everyone laughs! Humor +10');
        return true;
    };

    // ---- Absurd Events ----
    const absurdEvents = [
        { text: 'A chicken crosses the road. Then crosses back. Then asks YOU for directions.', effect: 'humor' },
        { text: 'The save point starts giving you life advice.', effect: 'meta' },
        { text: 'A random NPC gives you a quest to find their lost sock.', effect: 'quest' },
        { text: 'The final boss is just a really angry accountant.', effect: 'boss' },
        { text: 'Gravity takes a coffee break. You float.', effect: 'physics' },
    ];

    function triggerAbsurdEvent() {
        if ($gameParty._absurdity >= absurdEvents.length) {
            $gameMessage.add('Reality has run out of absurdity. For now.');
            return;
        }

        const event = absurdEvents[$gameParty._absurdity];
        $gameParty._absurdity++;
        $gameParty._humorLevel += 5;

        $gameMessage.add(`[ABSURD EVENT #${$gameParty._absurdity}]`);
        $gameMessage.add(event.text);

        if (event.effect === 'humor') {
            $gameParty._humorLevel += 5;
        } else if (event.effect === 'meta') {
            $gameMessage.add('(Seriously, who writes this stuff?)');
        } else if (event.effect === 'physics') {
            $gameParty.leader()._modernSkills = $gameParty.leader()._modernSkills || {};
            $gameParty.leader()._modernSkills['agility'] =
                ($gameParty.leader()._modernSkills['agility'] || 1) + 2;
        }
    }

    // ---- Fourth Wall Breaking ----
    function breakFourthWall(type) {
        switch (type) {
            case 'break':
                $gameMessage.add('Hey. You. Yes, YOU. The person behind the screen.');
                $gameMessage.add('Thanks for playing this comedy RPG!');
                $gameMessage.add(`You've accumulated ${$gameParty._humorLevel} humor points.`);
                break;
            case 'controls':
                $gameMessage.add('PSST: Press F5 to save. Or don\'t. Live dangerously.');
                break;
            case 'credits':
                $gameMessage.add('This joke brought to you by... a person typing JavaScript at 2 AM.');
                break;
        }
    }

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'tellJoke':
                    $gameParty.tellJoke(args[1]);
                    break;
                case 'absurdEvent':
                    triggerAbsurdEvent();
                    break;
                case 'humorStat':
                    $gameParty._humorLevel += parseInt(args[1]);
                    break;
                case 'fourthWall':
                    breakFourthWall(args[1]);
                    break;
            }
        }
    };
})();
