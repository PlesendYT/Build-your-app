/*:
 * @target MZ
 * @plugindesc Fantasy RPG Core Plugin v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Fantasy RPG Core Plugin
 * Adds custom spells, classes, and quest tracking.
 *
 * Plugin Commands:
 *   FantasyRPG learnSpell fireball
 *   FantasyRPG questStart saveTheVillage
 *   FantasyRPG questComplete saveTheVillage
 *   FantasyRPG setClass mage
 *   FantasyRPG dayNightCycle on
 *
 * @command learnSpell
 * @text Learn Spell
 * @desc Teach the party a new spell
 *
 * @arg spellId
 * @text Spell ID
 * @type string
 *
 * @command questStart
 * @text Start Quest
 * @desc Begin a new quest
 *
 * @arg questId
 * @text Quest ID
 * @type string
 *
 * @command questComplete
 * @text Complete Quest
 * @desc Mark a quest as complete
 *
 * @arg questId
 * @text Quest ID
 * @type string
 */

(function() {
    const pluginName = 'FantasyRPG';

    // ---- Spells ----
    const spells = {
        fireball: { name: 'Fireball', mpCost: 12, power: 80, element: 3 },
        heal: { name: 'Heal', mpCost: 8, power: 60, element: 0 },
        lightning: { name: 'Lightning', mpCost: 18, power: 110, element: 5 },
        shield: { name: 'Shield', mpCost: 6, power: 40, element: 0 },
    };

    $gameParty._learnedSpells = $gameParty._learnedSpells || [];
    $gameParty._quests = $gameParty._quests || [];

    Game_Party.prototype.learnSpell = function(spellId) {
        if (spells[spellId] && !this._learnedSpells.includes(spellId)) {
            this._learnedSpells.push(spellId);
            return true;
        }
        return false;
    };

    Game_Party.prototype.hasSpell = function(spellId) {
        return this._learnedSpells.includes(spellId);
    };

    Game_Party.prototype.getLearnedSpells = function() {
        return this._learnedSpells.map(id => ({ id, ...spells[id] }));
    };

    // ---- Quests ----
    Game_Party.prototype.startQuest = function(questId) {
        if (!this._quests.some(q => q.id === questId)) {
            this._quests.push({ id: questId, status: 'active' });
            return true;
        }
        return false;
    };

    Game_Party.prototype.completeQuest = function(questId) {
        const quest = this._quests.find(q => q.id === questId);
        if (quest) {
            quest.status = 'completed';
            return true;
        }
        return false;
    };

    Game_Party.prototype.getActiveQuests = function() {
        return this._quests.filter(q => q.status === 'active');
    };

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            const subCommand = args[0];
            const arg1 = args[1];
            switch (subCommand) {
                case 'learnSpell':
                    $gameParty.learnSpell(arg1);
                    break;
                case 'questStart':
                    $gameParty.startQuest(arg1);
                    break;
                case 'questComplete':
                    $gameParty.completeQuest(arg1);
                    break;
                case 'setClass':
                    $gameParty.leader().setClass(arg1);
                    break;
            }
        }
    };

    // ---- Custom Class ----
    Game_Actor.prototype.setClass = function(classId) {
        this._classId = classId;
        this._setupEquipment();
    };

    // ---- Day/Night Cycle ----
    let _dayNightEnabled = false;
    let _gameTime = 0;

    const _Scene_Map_update = Scene_Map.prototype.update;
    Scene_Map.prototype.update = function() {
        _Scene_Map_update.call(this);
        if (_dayNightEnabled) {
            _gameTime += 1 / 60;
            const hour = Math.floor(_gameTime / 3600) % 24;
            const brightness = (hour >= 6 && hour < 18) ? 255 : 80;
            $gameScreen.setTone([brightness - 255, brightness - 255, brightness - 255, 0], 60);
        }
    };

    PluginManager.registerCommand(pluginName, 'dayNightCycle', function(args) {
        _dayNightEnabled = args[0] === 'on';
    });

    // ---- Window: Quest Log ----
    function Window_QuestLog() {
        this.initialize.apply(this, arguments);
    }
    Window_QuestLog.prototype = Object.create(Window_Selectable.prototype);
    Window_QuestLog.prototype.constructor = Window_QuestLog;

    Window_QuestLog.prototype.initialize = function(x, y, width, height) {
        Window_Selectable.prototype.initialize.call(this, x, y, width, height);
        this.refresh();
    };

    Window_QuestLog.prototype.refresh = function() {
        this.clear();
        const quests = $gameParty.getActiveQuests();
        if (quests.length === 0) {
            this.drawText('No active quests', 0, 0, this.width, 'center');
            return;
        }
        quests.forEach((quest, i) => {
            this.drawText(quest.id, 0, i * this.lineHeight(), this.width, 'left');
        });
    };
})();
