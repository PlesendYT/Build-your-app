/*:
 * @target MZ
 * @plugindesc Fantasy RPG Skill Expansion v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Adds custom skill types and elemental interactions.
 *
 * @command addSkillTree
 * @text Add Skill Tree
 * @desc Add a skill tree for an actor
 * @arg actorId
 * @text Actor ID
 * @type number
 */

(function() {
    const pluginName = 'FantasyRPG_Skills';

    // Skill tree structure
    const skillTrees = {};

    function createSkillTree(actorId) {
        skillTrees[actorId] = {
            points: 0,
            nodes: [
                { id: 'power1', name: 'Power Up I', req: null, cost: 1, stat: 'atk', bonus: 5 },
                { id: 'power2', name: 'Power Up II', req: 'power1', cost: 2, stat: 'atk', bonus: 10 },
                { id: 'magic1', name: 'Magic Up I', req: null, cost: 1, stat: 'mat', bonus: 5 },
                { id: 'magic2', name: 'Magic Up II', req: 'magic1', cost: 2, stat: 'mat', bonus: 10 },
                { id: 'defense1', name: 'Defense Up I', req: null, cost: 1, stat: 'def', bonus: 5 },
                { id: 'defense2', name: 'Defense Up II', req: 'defense1', cost: 2, stat: 'def', bonus: 10 },
            ],
            unlocked: []
        };
    }

    function canUnlock(actorId, nodeId) {
        const tree = skillTrees[actorId];
        if (!tree || tree.unlocked.includes(nodeId)) return false;
        const node = tree.nodes.find(n => n.id === nodeId);
        if (!node) return false;
        if (tree.points < node.cost) return false;
        if (node.req && !tree.unlocked.includes(node.req)) return false;
        return true;
    }

    function unlockNode(actorId, nodeId) {
        const tree = skillTrees[actorId];
        if (!tree || !canUnlock(actorId, nodeId)) return false;
        const node = tree.nodes.find(n => n.id === nodeId);
        tree.points -= node.cost;
        tree.unlocked.push(nodeId);

        // Apply stat bonus
        const actor = $gameActors.actor(actorId);
        if (actor) {
            actor.addParam(node.stat === 'atk' ? 2 : node.stat === 'mat' ? 4 : 3, node.bonus);
        }
        return true;
    }

    function addSkillPoints(actorId, points) {
        if (!skillTrees[actorId]) createSkillTree(actorId);
        skillTrees[actorId].points += points;
    }

    // Elemental interaction system
    const elementMatrix = {
        fire: { strong: 'ice', weak: 'water' },
        ice: { strong: 'fire', weak: 'earth' },
        lightning: { strong: 'water', weak: 'earth' },
        water: { strong: 'fire', weak: 'lightning' },
        earth: { strong: 'lightning', weak: 'ice' },
        light: { strong: 'dark', weak: null },
        dark: { strong: 'light', weak: null },
    };

    function calculateElementalBonus(attackElement, targetElement) {
        if (!attackElement || !targetElement) return 1.0;
        const elem = elementMatrix[attackElement];
        if (!elem) return 1.0;
        if (elem.strong === targetElement) return 1.5;
        if (elem.weak === targetElement) return 0.5;
        return 1.0;
    }

    // Override damage calculation for elements
    const _Game_Action_makeDamageValue = Game_Action.prototype.makeDamageValue;
    Game_Action.prototype.makeDamageValue = function(target, critical) {
        const base = _Game_Action_makeDamageValue.call(this, target, critical);
        const attackElem = this.getItemElementType();
        const targetElem = target.elementRate(attackElem);
        return Math.floor(base * targetElem);
    };

    // Plugin commands
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'addPoints':
                    addSkillPoints(parseInt(args[1]), parseInt(args[2]));
                    break;
                case 'unlockNode':
                    unlockNode(parseInt(args[1]), args[2]);
                    break;
            }
        }
    };

    PluginManager.registerCommand(pluginName, 'addSkillTree', function(args) {
        createSkillTree(parseInt(args.actorId));
    });
})();
