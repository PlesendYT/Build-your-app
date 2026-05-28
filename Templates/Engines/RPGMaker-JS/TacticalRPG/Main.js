/*:
 * @target MZ
 * @plugindesc Tactical RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Tactical RPG with grid-based combat, AP system, and cover mechanics.
 *
 * Plugin Commands:
 *   TacticalRPG startBattle map_id
 *   TacticalRPG moveUnit x y
 *   TacticalRPG attackUnit target_x target_y
 *   TacticalRPG endTurn
 *
 * @command startBattle
 * @text Start Battle
 * @desc Begin a tactical battle on a map
 * @arg mapId
 * @text Map ID
 * @type string
 */

(function() {
    const pluginName = 'TacticalRPG';

    // ---- Grid System ----
    const TILE = { EMPTY: 0, WALL: 1, COVER_LOW: 2, COVER_HIGH: 3, OBSTACLE: 4 };

    $gameParty._tacticalMap = $gameParty._tacticalMap || [];
    $gameParty._tacticalUnits = $gameParty._tacticalUnits || [];
    $gameParty._tacticalTurn = 0;
    $gameParty._tacticalPhase = 'idle'; // 'idle', 'deploy', 'battle', 'result'
    $gameParty._selectedUnit = null;
    $gameParty._currentUnitIdx = 0;

    // ---- Unit Class ----
    function TacticalUnit(name, x, y, team, unitClass) {
        this.name = name;
        this.x = x;
        this.y = y;
        this.team = team; // 'player' or 'enemy'
        this.hp = 100;
        this.maxHp = 100;
        this.ap = 6;
        this.maxAp = 6;
        this.atk = 10;
        this.def = 5;
        this.range = 2;
        this.moveRange = 4;
        this.unitClass = unitClass;
        this.hasActed = false;
    }

    TacticalUnit.prototype.canMoveTo = function(tx, ty, map) {
        const dist = Math.abs(this.x - tx) + Math.abs(this.y - ty);
        if (dist > this.moveRange) return false;
        if (tx < 0 || ty < 0 || tx >= map[0].length || ty >= map.length) return false;
        if (map[ty][tx] === TILE.WALL || map[ty][tx] === TILE.OBSTACLE) return false;

        // Check if another unit occupies the tile
        for (const u of $gameParty._tacticalUnits) {
            if (u !== this && u.x === tx && u.y === ty && u.hp > 0) return false;
        }
        return true;
    };

    TacticalUnit.prototype.canAttack = function(tx, ty) {
        const dist = Math.abs(this.x - tx) + Math.abs(this.y - ty);
        return dist <= this.range && dist > 0;
    };

    TacticalUnit.prototype.takeDamage = function(amount) {
        const actual = Math.max(1, amount - this.def);
        this.hp -= actual;
        return actual;
    };

    // ---- Battle Maps ----
    const battleMaps = {
        map_1: {
            name: 'Training Grounds',
            grid: [
                [1, 1, 1, 1, 1, 1, 1],
                [1, 0, 0, 0, 0, 0, 1],
                [1, 0, 2, 0, 2, 0, 1],
                [1, 0, 0, 0, 0, 0, 1],
                [1, 0, 2, 0, 2, 0, 1],
                [1, 0, 0, 0, 0, 0, 1],
                [1, 1, 1, 1, 1, 1, 1],
            ],
            playerUnits: [
                { name: 'Knight', x: 1, y: 3, unitClass: 'knight' },
                { name: 'Archer', x: 2, y: 3, unitClass: 'archer' },
            ],
            enemyUnits: [
                { name: 'Bandit A', x: 5, y: 2, unitClass: 'bandit' },
                { name: 'Bandit B', x: 5, y: 4, unitClass: 'bandit' },
            ]
        },
        map_2: {
            name: 'Forest Clearing',
            grid: [
                [1, 1, 1, 1, 1, 1, 1, 1],
                [1, 0, 0, 3, 0, 0, 0, 1],
                [1, 0, 2, 0, 2, 0, 0, 1],
                [1, 0, 0, 0, 0, 3, 0, 1],
                [1, 0, 2, 0, 2, 0, 0, 1],
                [1, 0, 0, 0, 0, 0, 0, 1],
                [1, 1, 1, 1, 1, 1, 1, 1],
            ],
            playerUnits: [
                { name: 'Mage', x: 1, y: 3, unitClass: 'mage' },
                { name: 'Rogue', x: 2, y: 2, unitClass: 'rogue' },
            ],
            enemyUnits: [
                { name: 'Goblin A', x: 6, y: 2, unitClass: 'goblin' },
                { name: 'Goblin B', x: 6, y: 3, unitClass: 'goblin' },
                { name: 'Goblin C', x: 6, y: 4, unitClass: 'goblin' },
            ]
        }
    };

    // ---- Battle Initialization ----
    function startBattle(mapId) {
        const mapData = battleMaps[mapId];
        if (!mapData) return false;

        $gameParty._tacticalMap = mapData.grid;
        $gameParty._tacticalUnits = [];
        $gameParty._tacticalTurn = 1;
        $gameParty._tacticalPhase = 'battle';
        $gameParty._currentUnitIdx = 0;

        // Deploy player units
        mapData.playerUnits.forEach(u => {
            $gameParty._tacticalUnits.push(
                new TacticalUnit(u.name, u.x, u.y, 'player', u.unitClass)
            );
        });

        // Deploy enemy units
        mapData.enemyUnits.forEach(u => {
            $gameParty._tacticalUnits.push(
                new TacticalUnit(u.name, u.x, u.y, 'enemy', u.unitClass)
            );
        });

        return true;
    }

    // ---- Turn Management ----
    function nextUnit() {
        const units = $gameParty._tacticalUnits.filter(u => u.hp > 0 && u.team === 'player');
        if ($gameParty._currentUnitIdx < units.length - 1) {
            $gameParty._currentUnitIdx++;
            return units[$gameParty._currentUnitIdx];
        }
        return null; // No more player units
    }

    function endPlayerTurn() {
        // Reset all player units
        $gameParty._tacticalUnits.forEach(u => {
            if (u.team === 'player') {
                u.ap = u.maxAp;
                u.hasActed = false;
            }
        });
        $gameParty._currentUnitIdx = 0;

        // Enemy turn
        enemyTurn();
        $gameParty._tacticalTurn++;
    }

    function enemyTurn() {
        const enemies = $gameParty._tacticalUnits.filter(u => u.hp > 0 && u.team === 'enemy');
        enemies.forEach(enemy => {
            // Simple AI: find closest player unit
            const players = $gameParty._tacticalUnits.filter(u => u.hp > 0 && u.team === 'player');
            if (players.length === 0) return;

            let closest = players[0];
            let minDist = Infinity;
            players.forEach(p => {
                const dist = Math.abs(enemy.x - p.x) + Math.abs(enemy.y - p.y);
                if (dist < minDist) { minDist = dist; closest = p; }
            });

            // Move toward player
            if (minDist > enemy.range) {
                const dx = Math.sign(closest.x - enemy.x);
                const dy = Math.sign(closest.y - enemy.y);
                if (enemy.canMoveTo(enemy.x + dx, enemy.y, $gameParty._tacticalMap)) {
                    enemy.x += dx;
                } else if (enemy.canMoveTo(enemy.x, enemy.y + dy, $gameParty._tacticalMap)) {
                    enemy.y += dy;
                }
            }

            // Attack if in range
            if (minDist <= enemy.range) {
                const dmg = enemy.takeDamage(enemy.atk);
                $gameMessage.add(`${enemy.name} attacks ${closest.name} for ${dmg} damage!`);
            }
        });
    }

    // ---- Combat Resolution ----
    function resolveAttack(attacker, target) {
        if (!attacker || !target || target.hp <= 0) return null;

        const dist = Math.abs(attacker.x - target.x) + Math.abs(attacker.y - target.y);
        if (dist > attacker.range) return null;

        // Check cover
        const map = $gameParty._tacticalMap;
        let coverBonus = 1.0;
        if (map[target.y] && map[target.y][target.x] === TILE.COVER_HIGH) coverBonus = 0.3;
        else if (map[target.y] && map[target.y][target.x] === TILE.COVER_LOW) coverBonus = 0.5;

        const rawDamage = attacker.atk + Math.floor(Math.random() * 5);
        const actualDamage = Math.floor(rawDamage * coverBonus);
        const dealt = target.takeDamage(actualDamage);

        attacker.hasActed = true;
        attacker.ap -= 2;

        return { damage: dealt, killed: target.hp <= 0, cover: coverBonus < 1.0 };
    }

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'startBattle':
                    const success = startBattle(args[1]);
                    $gameSwitches.setValue(14, success);
                    break;
                case 'moveUnit': {
                    const unit = $gameParty._tacticalUnits[$gameParty._currentUnitIdx];
                    if (unit) unit.x = parseInt(args[1]); unit.y = parseInt(args[2]);
                    break;
                }
                case 'attackUnit': {
                    const atkUnit = $gameParty._tacticalUnits[$gameParty._currentUnitIdx];
                    const target = $gameParty._tacticalUnits.find(u =>
                        u.x === parseInt(args[1]) && u.y === parseInt(args[2]) && u.team === 'enemy'
                    );
                    if (atkUnit && target) resolveAttack(atkUnit, target);
                    break;
                }
                case 'endTurn':
                    if ($gameParty._tacticalPhase === 'battle') endPlayerTurn();
                    break;
            }
        }
    };
})();
