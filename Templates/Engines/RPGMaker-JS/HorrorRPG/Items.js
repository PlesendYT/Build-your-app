/*:
 * @target MZ
 * @plugindesc Horror RPG Items v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Horror-themed items: battery, medicine, keys, notes.
 */

(function() {
    const pluginName = 'HorrorRPG_Items';

    const horrorItemTypes = {
        battery: {
            name: 'Battery',
            effect: function() {
                $gameParty._flashlightBattery = Math.min(100, $gameParty._flashlightBattery + 30);
                return true;
            }
        },
        medicine: {
            name: 'Medicine',
            effect: function() {
                $gameParty.changeSanity(15);
                return true;
            }
        },
        note: {
            name: 'Research Note',
            effect: function() {
                $gameParty._notesFound = ($gameParty._notesFound || 0) + 1;
                $gameMessage.add(`Note #${$gameParty._notesFound} added to journal.`);
                return true;
            }
        },
        key: {
            name: 'Rusty Key',
            effect: function() {
                $gameSwitches.setValue(9, true);
                $gameMessage.add('You picked up a rusty key.');
                return true;
            }
        }
    };

    $gameParty._horrorItems = $gameParty._horrorItems || [];
    $gameParty._notesFound = $gameParty._notesFound || 0;

    Game_Party.prototype.useHorrorItem = function(itemType) {
        const item = horrorItemTypes[itemType];
        if (!item) return false;
        return item.effect();
    };

    // ---- Horror Events (randomized) ----
    const horrorEvents = [
        'A door slams shut behind you.',
        'You hear footsteps in the distance.',
        'The lights flicker and die.',
        'A whisper calls your name.',
        'You see a shadow move.',
        'Something touches your shoulder.',
    ];

    function triggerRandomHorrorEvent() {
        const event = horrorEvents[Math.floor(Math.random() * horrorEvents.length)];
        $gameMessage.add(event);
        $gameParty.changeSanity(-5);
        $gameScreen.startShake(3, 3, 30);

        if (Math.random() < 0.3) {
            $gameScreen.startFlash([255, 255, 255, 128], 15);
        }
    }

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'useHorrorItem':
                    $gameParty.useHorrorItem(args[1]);
                    break;
                case 'randomEvent':
                    triggerRandomHorrorEvent();
                    break;
            }
        }
    };
})();
