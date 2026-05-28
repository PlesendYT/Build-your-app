/*:
 * @target MZ
 * @plugindesc Modern RPG Items v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Modern items: smartphones, laptops, energy drinks, etc.
 *
 * @command useItem
 * @text Use Modern Item
 * @desc Use a modern item with special effects
 * @arg itemType
 * @text Item Type
 * @type string
 */

(function() {
    const pluginName = 'ModernRPG_Items';

    const modernItemEffects = {
        energyDrink: {
            name: 'Energy Drink',
            effect: function() {
                $gameParty.leader()._modernSkills['tech'] =
                    ($gameParty.leader()._modernSkills['tech'] || 1) + 2;
                $gameMessage.add('Tech skill temporarily boosted!');
            }
        },
        smartphone: {
            name: 'Smartphone',
            effect: function() {
                SceneManager.push(Scene_Smartphone);
            }
        },
        laptop: {
            name: 'Laptop',
            effect: function() {
                const hackLevel = $gameParty.leader().getModernSkillLevel('hack');
                $gameMessage.add(`Current hacking level: ${hackLevel}`);
            }
        },
        coffee: {
            name: 'Coffee',
            effect: function() {
                $gameParty.leader()._modernSkills['endurance'] =
                    ($gameParty.leader()._modernSkills['endurance'] || 1) + 1;
                $gameMessage.add('Feeling refreshed! Endurance up.');
            }
        }
    };

    // Track modern items in inventory
    $gameParty._modernItems = $gameParty._modernItems || [];

    Game_Party.prototype.addModernItem = function(itemId, count) {
        const existing = this._modernItems.find(i => i.id === itemId);
        if (existing) {
            existing.count += count;
        } else {
            this._modernItems.push({ id: itemId, count });
        }
    };

    Game_Party.prototype.useModernItem = function(itemId) {
        const item = this._modernItems.find(i => i.id === itemId);
        if (!item || item.count <= 0) return false;

        const effect = modernItemEffects[itemId];
        if (effect) {
            effect.effect();
            item.count--;
            return true;
        }
        return false;
    };

    // Apartment decoration system
    $gameParty._apartmentItems = $gameParty._apartmentItems || [];

    Game_Party.prototype.addApartmentItem = function(itemName) {
        this._apartmentItems.push(itemName);
    };

    Game_Party.prototype.getApartmentItems = function() {
        return this._apartmentItems;
    };

    // Plugin Commands
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'useItem':
                    $gameParty.useModernItem(args[1]);
                    break;
                case 'addItem':
                    $gameParty.addModernItem(args[1], parseInt(args[2]));
                    break;
                case 'decorate':
                    $gameParty.addApartmentItem(args[1]);
                    break;
            }
        }
    };
})();
