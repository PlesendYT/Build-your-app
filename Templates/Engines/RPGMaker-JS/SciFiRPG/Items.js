/*:
 * @target MZ
 * @plugindesc Sci-Fi Items & Resources v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Sci-fi items, resources, and crafting for space adventures.
 */

(function() {
    const pluginName = 'SciFiRPG_Items';

    const sciFiResources = {
        crystal: { name: 'Energy Crystal', rarity: 'uncommon', value: 50 },
        plasma: { name: 'Plasma Cell', rarity: 'rare', value: 100 },
        fuel: { name: 'Starship Fuel', rarity: 'common', value: 25 },
        minerals: { name: 'Space Minerals', rarity: 'uncommon', value: 60 },
        data: { name: 'Alien Data Core', rarity: 'epic', value: 250 },
        ore: { name: 'Titanium Ore', rarity: 'common', value: 30 },
    };

    $gameParty._resources = $gameParty._resources || {};

    Game_Party.prototype.addResource = function(resourceId, count) {
        if (sciFiResources[resourceId]) {
            this._resources[resourceId] = (this._resources[resourceId] || 0) + count;
            return true;
        }
        return false;
    };

    Game_Party.prototype.removeResource = function(resourceId, count) {
        if (this._resources[resourceId] && this._resources[resourceId] >= count) {
            this._resources[resourceId] -= count;
            return true;
        }
        return false;
    };

    Game_Party.prototype.getResourceCount = function(resourceId) {
        return this._resources[resourceId] || 0;
    };

    Game_Party.prototype.sellResources = function(resourceId, count) {
        const resource = sciFiResources[resourceId];
        if (!resource || !this.removeResource(resourceId, count)) return false;
        this._money = (this._money || 0) + resource.value * count;
        return true;
    };

    // Repair ship using resources
    Game_Party.prototype.repairShip = function() {
        const ship = this._ship;
        if (!ship) return false;

        const cost = ship.maxHull - ship.hull;
        const mineralsNeeded = Math.ceil(cost / 10);
        const fuelNeeded = Math.ceil(cost / 5);

        if ((this._resources['minerals'] || 0) >= mineralsNeeded &&
            (this._resources['fuel'] || 0) >= fuelNeeded) {
            this.removeResource('minerals', mineralsNeeded);
            this.removeResource('fuel', fuelNeeded);
            ship.hull = ship.maxHull;
            return true;
        }
        return false;
    };

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'addResource':
                    $gameParty.addResource(args[1], parseInt(args[2]));
                    break;
                case 'sellResource':
                    $gameParty.sellResources(args[1], parseInt(args[2]));
                    break;
                case 'repairShip':
                    const repaired = $gameParty.repairShip();
                    $gameSwitches.setValue(5, repaired);
                    break;
            }
        }
    };
})();
