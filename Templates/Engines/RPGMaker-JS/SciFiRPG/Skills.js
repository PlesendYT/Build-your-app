/*:
 * @target MZ
 * @plugindesc Sci-Fi Skills & Hacking v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Sci-fi themed skills: hacking, piloting, engineering.
 *
 * @command hackSystem
 * @text Hack System
 * @desc Hack an alien computer system
 * @arg systemLevel
 * @text System Level
 * @type number
 */

(function() {
    const pluginName = 'SciFiRPG_Skills';

    const sciFiSkills = {
        hacking: { name: 'Hacking', baseLevel: 1 },
        piloting: { name: 'Piloting', baseLevel: 1 },
        engineering: { name: 'Engineering', baseLevel: 1 },
        xenobiology: { name: 'Xenobiology', baseLevel: 1 },
    };

    Game_Actor.prototype.getSciFiSkill = function(skillId) {
        return this._sciFiSkills ? (this._sciFiSkills[skillId] || 1) : 1;
    };

    Game_Actor.prototype.setupSciFiSkills = function() {
        this._sciFiSkills = this._sciFiSkills || {};
        Object.keys(sciFiSkills).forEach(s => {
            this._sciFiSkills[s] = this._sciFiSkills[s] || 1;
        });
    };

    const _Game_Actor_setup = Game_Actor.prototype.setup;
    Game_Actor.prototype.setup = function(actorId) {
        _Game_Actor_setup.call(this, actorId);
        this.setupSciFiSkills();
    };

    // Hacking minigame
    function hackSystem(targetLevel) {
        const leader = $gameParty.leader();
        const hackLevel = leader.getSciFiSkill('hacking');
        const required = targetLevel * 2;

        if (hackLevel >= required) {
            return 'auto_success';
        }

        // Skill check
        const roll = Math.random() * 10 + hackLevel;
        if (roll >= targetLevel) {
            leader._sciFiSkills['hacking'] += 0.5;
            return 'success';
        }
        return 'failed';
    }

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'hackSystem': {
                    const result = hackSystem(parseInt(args[1]));
                    $gameVariables.setValue(1, result === 'success' || result === 'auto_success' ? 1 : 0);
                    break;
                }
                case 'pilotCheck': {
                    const pilot = $gameParty.leader().getSciFiSkill('piloting');
                    $gameVariables.setValue(2, pilot);
                    break;
                }
            }
        }
    };
})();
