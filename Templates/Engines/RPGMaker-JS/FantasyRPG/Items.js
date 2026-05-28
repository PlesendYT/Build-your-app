/*:
 * @target MZ
 * @plugindesc Fantasy RPG Item & Crafting v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Adds crafting recipes, item rarity, and custom item effects.
 *
 * @command craftItem
 * @text Craft Item
 * @desc Craft an item from materials
 * @arg recipeId
 * @text Recipe ID
 * @type string
 *
 * @command addRecipe
 * @text Add Recipe
 * @desc Add a crafting recipe to the player's known recipes
 * @arg recipeId
 * @text Recipe ID
 * @type string
 */

(function() {
    const pluginName = 'FantasyRPG_Items';

    const recipes = {
        healthPotion: {
            name: 'Health Potion',
            result: { type: 'item', id: 1, count: 1 },
            materials: [
                { type: 'item', id: 2, count: 3 },
                { type: 'item', id: 3, count: 1 }
            ]
        },
        manaPotion: {
            name: 'Mana Potion',
            result: { type: 'item', id: 4, count: 1 },
            materials: [
                { type: 'item', id: 5, count: 2 },
                { type: 'item', id: 6, count: 1 }
            ]
        },
        steelSword: {
            name: 'Steel Sword',
            result: { type: 'weapon', id: 7, count: 1 },
            materials: [
                { type: 'item', id: 8, count: 4 },
                { type: 'item', id: 9, count: 2 }
            ]
        }
    };

    $gameParty._knownRecipes = $gameParty._knownRecipes || [];

    Game_Party.prototype.learnRecipe = function(recipeId) {
        if (!this._knownRecipes.includes(recipeId) && recipes[recipeId]) {
            this._knownRecipes.push(recipeId);
            return true;
        }
        return false;
    };

    Game_Party.prototype.hasRecipe = function(recipeId) {
        return this._knownRecipes.includes(recipeId);
    };

    Game_Party.prototype.canCraft = function(recipeId) {
        const recipe = recipes[recipeId];
        if (!recipe) return false;
        return recipe.materials.every(mat => {
            const count = mat.type === 'item' ?
                $gameParty.numItems($dataItems[mat.id]) :
                $gameParty.numItems($dataWeapons[mat.id]);
            return count >= mat.count;
        });
    };

    Game_Party.prototype.craftItem = function(recipeId) {
        if (!this.canCraft(recipeId)) return false;
        const recipe = recipes[recipeId];

        // Remove materials
        recipe.materials.forEach(mat => {
            for (let i = 0; i < mat.count; i++) {
                if (mat.type === 'item') {
                    $gameParty.loseItem($dataItems[mat.id], 1);
                } else {
                    $gameParty.loseItem($dataWeapons[mat.id], 1);
                }
            }
        });

        // Add result
        const result = recipe.result;
        for (let i = 0; i < result.count; i++) {
            if (result.type === 'item') {
                $gameParty.gainItem($dataItems[result.id], 1);
            } else {
                $gameParty.gainItem($dataWeapons[result.id], 1);
            }
        }
        return true;
    };

    // Item rarity system
    const rarityColors = {
        common: '#ffffff',
        uncommon: '#1eff00',
        rare: '#0070dd',
        epic: '#a335ee',
        legendary: '#ff8000'
    };

    const _Window_ItemList_drawItem = Window_ItemList.prototype.drawItem;
    Window_ItemList.prototype.drawItem = function(index) {
        const item = this.itemAt(index);
        if (item && item.meta && item.meta.rarity) {
            const color = rarityColors[item.meta.rarity] || '#ffffff';
            this.changeTextColor(ColorManager.textColorFromHex(color));
        }
        _Window_ItemList_drawItem.call(this, index);
        this.resetTextColor();
    };

    // Plugin commands
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'craftItem':
                    $gameParty.craftItem(args[1]);
                    break;
                case 'addRecipe':
                    $gameParty.learnRecipe(args[1]);
                    break;
            }
        }
    };
})();
