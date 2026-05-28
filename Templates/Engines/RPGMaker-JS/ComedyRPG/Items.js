/*:
 * @target MZ
 * @plugindesc Comedy RPG Items v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Comedy items: whoopee cushion, rubber chicken, joke book.
 */

(function() {
    const pluginName = 'ComedyRPG_Items';

    const comedyItems = {
        whoopeeCushion: {
            name: 'Whoopee Cushion',
            effect: function() {
                $gameMessage.add('PRRRRRRRT!');
                $gameMessage.add('...');
                $gameMessage.add('Everyone is in stitches!');
                $gameParty._humorLevel += 5;
                return true;
            }
        },
        rubberChicken: {
            name: 'Rubber Chicken',
            effect: function() {
                $gameMessage.add('*SQUEAK* *SQUEAK* *SQUEAK*');
                $gameMessage.add('You wave the rubber chicken menacingly.');
                $gameParty._humorLevel += 3;
                return true;
            }
        },
        jokeBook: {
            name: 'Joke Book',
            effect: function() {
                $gameMessage.add('You open the joke book...');
                $gameMessage.add('"Why did the scarecrow win an award?"');
                $gameMessage.add('"Because he was outstanding in his field!"');
                $gameParty._humorLevel += 8;
                return true;
            }
        },
        bananaPeel: {
            name: 'Banana Peel',
            effect: function() {
                $gameMessage.add('You place the banana peel on the ground.');
                $gameMessage.add('An enemy slips on it! Classic!');
                return true;
            }
        }
    };

    Game_Party.prototype.useComedyItem = function(itemId) {
        const item = comedyItems[itemId];
        if (!item) return false;
        return item.effect();
    };

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            if (args[0] === 'useComedyItem') {
                $gameParty.useComedyItem(args[1]);
            }
        }
    };
})();
