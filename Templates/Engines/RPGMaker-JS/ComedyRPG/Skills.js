/*:
 * @target MZ
 * @plugindesc Comedy RPG Skills v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Comedy skills: comedy (damage with laughter), improvisation, mockery.
 */

(function() {
    const pluginName = 'ComedyRPG_Skills';

    const comedySkills = {
        comedy: { name: 'Comedy', damageType: 'laughter' },
        improvisation: { name: 'Improvisation', damageType: 'confusion' },
        mockery: { name: 'Mockery', damageType: 'insult' },
        dadJokes: { name: 'Dad Jokes', damageType: 'cringe' },
    };

    Game_Actor.prototype.getComedySkill = function(skillId) {
        return this._comedySkills ? (this._comedySkills[skillId] || 1) : 1;
    };

    Game_Actor.prototype.setupComedySkills = function() {
        this._comedySkills = this._comedySkills || {};
        Object.keys(comedySkills).forEach(s => {
            this._comedySkills[s] = this._comedySkills[s] || 1;
        });
    };

    const _Game_Actor_setup = Game_Actor.prototype.setup;
    Game_Actor.prototype.setup = function(actorId) {
        _Game_Actor_setup.call(this, actorId);
        this.setupComedySkills();
    };

    // Comedy attack with humor modifier
    const _Game_Action_makeDamageValue = Game_Action.prototype.makeDamageValue;
    Game_Action.prototype.makeDamageValue = function(target, critical) {
        const base = _Game_Action_makeDamageValue.call(this, target, critical);

        // If the action has the 'comedy' tag
        if (this.item() && this.item().meta && this.item().meta.comedy) {
            const humorMod = 1 + ($gameParty._humorLevel || 0) / 100;
            return Math.floor(base * humorMod);
        }
        return base;
    };

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            if (args[0] === 'comedyCheck') {
                const skill = $gameParty.leader().getComedySkill(args[1]);
                $gameVariables.setValue(10, skill);
            }
        }
    };
})();
