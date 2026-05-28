/*:
 * @target MZ
 * @plugindesc Horror RPG Skills v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Horror-themed skills: stealth, perception, lockpicking.
 */

(function() {
    const pluginName = 'HorrorRPG_Skills';

    const horrorSkills = {
        stealth: { name: 'Stealth' },
        perception: { name: 'Perception' },
        lockpick: { name: 'Lockpick' },
        firstAid: { name: 'First Aid' },
    };

    Game_Actor.prototype.getHorrorSkill = function(skillId) {
        return this._horrorSkills ? (this._horrorSkills[skillId] || 1) : 1;
    };

    Game_Actor.prototype.setupHorrorSkills = function() {
        this._horrorSkills = this._horrorSkills || {};
        Object.keys(horrorSkills).forEach(s => {
            this._horrorSkills[s] = this._horrorSkills[s] || 1;
        });
    };

    const _Game_Actor_setup = Game_Actor.prototype.setup;
    Game_Actor.prototype.setup = function(actorId) {
        _Game_Actor_setup.call(this, actorId);
        this.setupHorrorSkills();
    };

    // Lockpick check
    function attemptLockpick(difficulty) {
        const leader = $gameParty.leader();
        const skill = leader.getHorrorSkill('lockpick');
        const roll = Math.random() * 10;
        return (roll + skill) >= difficulty;
    }

    // Perception check (detect monster or items)
    function perceptionCheck(隐藏ObjectDifficulty) {
        const leader = $gameParty.leader();
        const skill = leader.getHorrorSkill('perception');
        const sanity = $gameParty._sanity;
        const modifier = sanity < 30 ? -2 : 0;
        const roll = Math.random() * 10;
        return (roll + skill + modifier) >= 隐藏ObjectDifficulty;
    }

    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'lockpick': {
                    const success = attemptLockpick(parseInt(args[1]));
                    $gameSwitches.setValue(7, success);
                    break;
                }
                case 'perception': {
                    const found = perceptionCheck(parseInt(args[1]));
                    $gameSwitches.setValue(8, found);
                    break;
                }
                case 'healSanity': {
                    const amount = parseInt(args[1]);
                    $gameParty.changeSanity(amount);
                    break;
                }
            }
        }
    };
})();
