/*:
 * @target MZ
 * @plugindesc Modern RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Modern RPG setting with smartphone interface, job system, and relationships.
 *
 * Plugin Commands:
 *   ModernRPG jobTake waiter
 *   ModernRPG jobProgress waiter 10
 *   ModernRPG relationshipMeet npc_1
 *   ModernRPG relationshipGain npc_1 5
 *   ModernRPG timeSet 14 30
 *
 * @command jobTake
 * @text Take Job
 * @desc Assign a job to the party leader
 * @arg jobId
 * @text Job ID
 * @type string
 */

(function() {
    const pluginName = 'ModernRPG';

    // ---- Job System ----
    const jobs = {
        waiter: { name: 'Waiter', basePay: 15, skill: 'charm' },
        barista: { name: 'Barista', basePay: 18, skill: 'speed' },
        tutor: { name: 'Tutor', basePay: 20, skill: 'intelligence' },
        driver: { name: 'Driver', basePay: 22, skill: 'endurance' },
        programmer: { name: 'Programmer', basePay: 30, skill: 'tech' },
    };

    $gameParty._job = $gameParty._job || null;
    $gameParty._jobProgress = $gameParty._jobProgress || 0;
    $gameParty._money = $gameParty._money || 500;

    Game_Party.prototype.takeJob = function(jobId) {
        if (jobs[jobId]) {
            this._job = jobId;
            this._jobProgress = 0;
            return true;
        }
        return false;
    };

    Game_Party.prototype.advanceJob = function(amount) {
        if (!this._job) return false;
        this._jobProgress += amount;
        const job = jobs[this._job];
        if (this._jobProgress >= 100) {
            this._money += job.basePay * 2;
            this._jobProgress = 0;
            // Level up notification
            $gameMessage.add(`Job complete! Earned bonus pay!`);
        }
        return true;
    };

    Game_Party.prototype.getJobPay = function() {
        if (!this._job) return 0;
        return jobs[this._job].basePay + Math.floor(this._jobProgress / 10);
    };

    // ---- Relationship System ----
    $gameParty._relationships = $gameParty._relationships || {};

    Game_Party.prototype.meetCharacter = function(npcId) {
        if (!this._relationships[npcId]) {
            this._relationships[npcId] = { affinity: 0, met: true };
            return true;
        }
        return false;
    };

    Game_Party.prototype.changeAffinity = function(npcId, amount) {
        if (this._relationships[npcId]) {
            this._relationships[npcId].affinity =
                Math.max(-100, Math.min(100, this._relationships[npcId].affinity + amount));
            return true;
        }
        return false;
    };

    Game_Party.prototype.getAffinity = function(npcId) {
        return this._relationships[npcId] ? this._relationships[npcId].affinity : 0;
    };

    // ---- Time System ----
    let _gameHour = 8;
    let _gameMinute = 0;

    function advanceTime(minutes) {
        _gameMinute += minutes;
        while (_gameMinute >= 60) {
            _gameMinute -= 60;
            _gameHour++;
        }
        if (_gameHour >= 24) _gameHour = 0;
    }

    function getTimeString() {
        const ampm = _gameHour >= 12 ? 'PM' : 'AM';
        const h = _gameHour % 12 || 12;
        const m = String(_gameMinute).padStart(2, '0');
        return `${h}:${m} ${ampm}`;
    }

    // ---- Smartphone Screen ----
    function Scene_Smartphone() {
        this.initialize.apply(this, arguments);
    }
    Scene_Smartphone.prototype = Object.create(Scene_MenuBase.prototype);
    Scene_Smartphone.prototype.constructor = Scene_Smartphone;

    Scene_Smartphone.prototype.initialize = function() {
        Scene_MenuBase.prototype.initialize.call(this);
    };

    Scene_Smartphone.prototype.create = function() {
        Scene_MenuBase.prototype.create.call(this);
        this._helpWindow = new Window_Help(1);
        this._helpWindow.setText(`Time: ${getTimeString()} | Money: $${$gameParty._money}`);
        this.addWindow(this._helpWindow);

        const commands = ['Jobs', 'Relationships', 'Messages', 'Map', 'Close'];
        this._commandWindow = new Window_Command(commands);
        this._commandWindow.setHandler('Jobs', () => this.openJobs());
        this._commandWindow.setHandler('Relationships', () => this.openRelationships());
        this._commandWindow.setHandler('Close', () => this.popScene());
        this.addWindow(this._commandWindow);
    };

    Scene_Smartphone.prototype.openJobs = function() {
        const job = $gameParty._job;
        if (job) {
            const j = jobs[job];
            $gameMessage.add(`Current Job: ${j.name}`);
            $gameMessage.add(`Progress: ${$gameParty._jobProgress}/100`);
            $gameMessage.add(`Pay: $${$gameParty.getJobPay()}/hr`);
        } else {
            $gameMessage.add('No job currently.');
        }
    };

    Scene_Smartphone.prototype.openRelationships = function() {
        const rels = $gameParty._relationships;
        const keys = Object.keys(rels);
        if (keys.length === 0) {
            $gameMessage.add('No relationships yet.');
            return;
        }
        keys.forEach(key => {
            $gameMessage.add(`${key}: Affinity ${rels[key].affinity}`);
        });
    };

    // Plugin Commands
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'jobTake':
                    $gameParty.takeJob(args[1]);
                    break;
                case 'jobProgress':
                    $gameParty.advanceJob(parseInt(args[2]));
                    break;
                case 'relationshipMeet':
                    $gameParty.meetCharacter(args[1]);
                    break;
                case 'relationshipGain':
                    $gameParty.changeAffinity(args[1], parseInt(args[2]));
                    break;
                case 'timeSet':
                    _gameHour = parseInt(args[1]);
                    _gameMinute = parseInt(args[2]);
                    break;
            }
        }
    };

    // Register Smartphone scene call
    PluginManager.registerCommand(pluginName, 'openSmartphone', function() {
        SceneManager.push(Scene_Smartphone);
    });
})();
