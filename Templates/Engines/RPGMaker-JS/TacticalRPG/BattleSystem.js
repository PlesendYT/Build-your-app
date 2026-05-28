/*:
 * @target MZ
 * @plugindesc Tactical Battle UI v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Tactical battle UI grid rendering and unit info.
 */

(function() {
    const pluginName = 'TacticalRPG_UI';

    // ---- Unit Info Window ----
    function Window_UnitInfo() {
        this.initialize.apply(this, arguments);
    }
    Window_UnitInfo.prototype = Object.create(Window_Base.prototype);
    Window_UnitInfo.prototype.constructor = Window_UnitInfo;

    Window_UnitInfo.prototype.initialize = function() {
        Window_Base.prototype.initialize.call(this, 0, 0, 400, 160);
    };

    Window_UnitInfo.prototype.refresh = function() {
        this.contents.clear();
        const units = $gameParty._tacticalUnits;
        const idx = $gameParty._currentUnitIdx;
        if (!units || units.length === 0) return;

        const unit = units[idx];
        if (!unit || unit.team !== 'player') {
            this.drawText('Select a unit to command', 0, 0, 380, 'left');
            return;
        }

        this.drawText(`${unit.name} (${unit.unitClass})`, 0, 0, 380, 'left');
        this.drawText(`HP: ${unit.hp}/${unit.maxHp}`, 0, 30, 180, 'left');
        this.drawText(`AP: ${unit.ap}/${unit.maxAp}`, 200, 30, 180, 'left');
        this.drawText(`ATK: ${unit.atk} DEF: ${unit.def}`, 0, 60, 380, 'left');
        this.drawText(`Move: ${unit.moveRange} Range: ${unit.range}`, 0, 90, 380, 'left');
    };

    // ---- Battle Status Window ----
    function Window_BattleStatus() {
        this.initialize.apply(this, arguments);
    }
    Window_BattleStatus.prototype = Object.create(Window_Base.prototype);
    Window_BattleStatus.prototype.constructor = Window_BattleStatus;

    Window_BattleStatus.prototype.initialize = function() {
        Window_Base.prototype.initialize.call(this, 0, 440, 400, 120);
    };

    Window_BattleStatus.prototype.refresh = function() {
        this.contents.clear();
        this.drawText(`Turn: ${$gameParty._tacticalTurn}`, 0, 0, 200, 'left');

        const playerUnits = $gameParty._tacticalUnits.filter(u => u.team === 'player' && u.hp > 0);
        const enemyUnits = $gameParty._tacticalUnits.filter(u => u.team === 'enemy' && u.hp > 0);

        this.drawText(`Allies: ${playerUnits.length}`, 0, 30, 200, 'left');
        this.drawText(`Enemies: ${enemyUnits.length}`, 200, 30, 200, 'left');

        if (enemyUnits.length === 0) {
            this.drawText('VICTORY!', 150, 60, 100, 'center');
            $gameParty._tacticalPhase = 'result';
        } else if (playerUnits.length === 0) {
            this.drawText('DEFEAT', 150, 60, 100, 'center');
            $gameParty._tacticalPhase = 'result';
        }
    };

    // ---- Class abilities ----
    const classAbilities = {
        knight: { name: 'Knight', bonusAtk: 3, bonusDef: 5, bonusHp: 30 },
        archer: { name: 'Archer', bonusAtk: 2, rangeBonus: 2 },
        mage: { name: 'Mage', bonusAtk: 5, rangeBonus: 1 },
        rogue: { name: 'Rogue', bonusMove: 2, bonusAtk: 1 },
        bandit: { name: 'Bandit', bonusAtk: 1 },
        goblin: { name: 'Goblin', bonusMove: 1 },
    };

    // Apply class bonuses when creating units
    const _TacticalUnit_create = TacticalUnit;
    TacticalUnit = function(name, x, y, team, unitClass) {
        _TacticalUnit_create.call(this, name, x, y, team, unitClass);
        const bonuses = classAbilities[unitClass];
        if (bonuses) {
            this.maxHp += bonuses.bonusHp || 0;
            this.hp = this.maxHp;
            this.atk += bonuses.bonusAtk || 0;
            this.def += bonuses.bonusDef || 0;
            this.range += bonuses.rangeBonus || 0;
            this.moveRange += bonuses.bonusMove || 0;
        }
    };
})();
