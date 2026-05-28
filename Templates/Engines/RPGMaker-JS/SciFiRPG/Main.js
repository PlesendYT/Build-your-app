/*:
 * @target MZ
 * @plugindesc Sci-Fi RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Sci-fi RPG with spaceship combat and tech tree.
 *
 * Plugin Commands:
 *   SciFiRPG shipBattle enemy_type
 *   SciFiRPG techUnlock warp_drive
 *   SciFiRPG explorePlanet planet_1
 *   SciFiRPG hireCrew member_name
 *   SciFiRPG alienEncounter race_name
 *
 * @command shipBattle
 * @text Ship Battle
 * @desc Start a spaceship battle
 * @arg enemyType
 * @text Enemy Type
 * @type string
 */

(function() {
    const pluginName = 'SciFiRPG';

    // ---- Ship Stats ----
    $gameParty._ship = $gameParty._ship || {
        hull: 100,
        maxHull: 100,
        shields: 50,
        maxShields: 50,
        weapons: 10,
        engine: 5,
        crew: []
    };

    // ---- Tech Tree ----
    const techTree = {
        warpDrive: { name: 'Warp Drive', cost: 3, prerequisite: null, effect: 'unlock_fast_travel' },
        plasmaCannons: { name: 'Plasma Cannons', cost: 4, prerequisite: null, effect: 'weapon_boost' },
        shieldMatrix: { name: 'Shield Matrix', cost: 3, prerequisite: null, effect: 'shield_boost' },
        cloakingDevice: { name: 'Cloaking Device', cost: 5, prerequisite: 'shieldMatrix', effect: 'stealth' },
        hyperdrive: { name: 'Hyperdrive', cost: 6, prerequisite: 'warpDrive', effect: 'unlock_galaxy_map' },
    };

    $gameParty._unlockedTech = $gameParty._unlockedTech || [];

    Game_Party.prototype.unlockTech = function(techId) {
        const tech = techTree[techId];
        if (!tech || this._unlockedTech.includes(techId)) return false;
        if (tech.prerequisite && !this._unlockedTech.includes(tech.prerequisite)) return false;

        this._unlockedTech.push(techId);
        switch (tech.effect) {
            case 'weapon_boost':
                this._ship.weapons += 5;
                break;
            case 'shield_boost':
                this._ship.maxShields += 25;
                this._ship.shields = this._ship.maxShields;
                break;
        }
        return true;
    };

    // ---- Alien Races ----
    const alienRaces = {
        zorblax: { name: 'Zorblax', hostility: 0.7, techLevel: 8 },
        xandarians: { name: 'Xandarians', hostility: 0.2, techLevel: 12 },
        krynn: { name: 'Krynn', hostility: 0.5, techLevel: 6 },
        voidWalkers: { name: 'Void Walkers', hostility: 0.9, techLevel: 15 },
    };

    $gameParty._alienRelations = $gameParty._alienRelations || {};

    // ---- Ship Combat ----
    function resolveShipCombat(enemyPower) {
        const ship = $gameParty._ship;
        const playerPower = ship.weapons + ship.crew.length * 2;

        const combatPower = (playerPower / enemyPower) * 100 + Math.random() * 30;

        if (combatPower > 60) {
            const loot = Math.floor(combatPower / 10);
            $gameParty._money = ($gameParty._money || 0) + loot * 10;
            return 'victory';
        } else if (combatPower > 30) {
            ship.hull -= 20;
            return 'retreat';
        } else {
            ship.hull -= 50;
            return 'defeat';
        }
    }

    // ---- Planet Exploration ----
    const planets = {
        planet_1: { name: 'Aetheria', danger: 0.3, resources: ['crystal', 'ore'], discovery: 'Ancient Ruins' },
        planet_2: { name: 'Nova Prime', danger: 0.7, resources: ['plasma', 'fuel'], discovery: 'Alien Outpost' },
        planet_3: { name: 'Xylos', danger: 0.5, resources: ['minerals', 'data'], discovery: 'Abandoned Lab' },
    };

    function explorePlanet(planetId) {
        const planet = planets[planetId];
        if (!planet) return;

        const dangerRoll = Math.random();
        if (dangerRoll < planet.danger) {
            $gameMessage.add(`Danger on ${planet.name}!`);
            $gameParty._ship.hull -= 15;
            return 'danger';
        }

        $gameMessage.add(`Explored ${planet.name}. Found: ${planet.discovery}`);
        planet.resources.forEach(res => {
            $gameParty._modernItems = $gameParty._modernItems || [];
            $gameParty._modernItems.push(res);
        });
        return 'success';
    }

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'shipBattle': {
                    const enemyPower = alienRaces[args[1]] ? alienRaces[args[1]].techLevel : 5;
                    const result = resolveShipCombat(enemyPower);
                    $gameMessage.add(`Battle result: ${result}`);
                    break;
                }
                case 'techUnlock':
                    $gameParty.unlockTech(args[1]);
                    break;
                case 'explorePlanet': {
                    const res = explorePlanet(args[1]);
                    if (res) $gameSwitches.setValue(4, res === 'success');
                    break;
                }
                case 'hireCrew':
                    $gameParty._ship.crew.push(args[1]);
                    break;
                case 'alienEncounter': {
                    const race = alienRaces[args[1]];
                    if (race) {
                        $gameMessage.add(`Encountered ${race.name} (Hostility: ${race.hostility})`);
                    }
                    break;
                }
            }
        }
    };
})();
