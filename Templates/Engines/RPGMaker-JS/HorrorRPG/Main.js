/*:
 * @target MZ
 * @plugindesc Horror RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Horror RPG with sanity system, flashlight, and monster AI.
 *
 * Plugin Commands:
 *   HorrorRPG sanityChange -10
 *   HorrorRPG flashlight on
 *   HorrorRPG monsterNearby true
 *   HorrorRPG hide
 *   HorrorRPG itemFind battery
 *
 * @command sanityChange
 * @text Change Sanity
 * @desc Modify the party's sanity level
 * @arg amount
 * @text Amount
 * @type number
 */

(function() {
    const pluginName = 'HorrorRPG';

    $gameParty._sanity = $gameParty._sanity || 100;
    $gameParty._maxSanity = 100;
    $gameParty._flashlightOn = $gameParty._flashlightOn || false;
    $gameParty._flashlightBattery = $gameParty._flashlightBattery || 100;
    $gameParty._isHiding = $gameParty._isHiding || false;
    $gameParty._monsterAlert = $gameParty._monsterAlert || 0;

    // ---- Sanity System ----
    Game_Party.prototype.changeSanity = function(amount) {
        this._sanity = Math.max(0, Math.min(this._maxSanity, this._sanity + amount));

        if (this._sanity <= 0) {
            this._sanity = 0;
            return 'broken';
        }
        if (this._sanity <= 25) {
            return 'critical';
        }
        if (this._sanity <= 50) {
            return 'unstable';
        }
        return 'stable';
    };

    Game_Party.prototype.getSanityLevel = function() {
        if (this._sanity <= 0) return 'broken';
        if (this._sanity <= 25) return 'critical';
        if (this._sanity <= 50) return 'unstable';
        return 'stable';
    };

    // ---- Flashlight ----
    Game_Party.prototype.toggleFlashlight = function() {
        this._flashlightOn = !this._flashlightOn;
    };

    Game_Party.prototype.updateFlashlight = function() {
        if (this._flashlightOn && this._flashlightBattery > 0) {
            this._flashlightBattery -= 0.5;
            if (this._flashlightBattery <= 0) {
                this._flashlightBattery = 0;
                this._flashlightOn = false;
                return 'dead';
            }
        }
        return this._flashlightBattery > 0 ? 'on' : 'dead';
    };

    // ---- Hiding ----
    Game_Party.prototype.hide = function() {
        this._isHiding = true;
    };

    Game_Party.prototype.unhide = function() {
        this._isHiding = false;
    };

    // ---- Monster AI ----
    let _monsterPosition = { x: 10, y: 10 };
    let _playerLastSeen = { x: 0, y: 0 };

    function updateMonsterAI() {
        const playerX = $gamePlayer.x;
        const playerY = $gamePlayer.y;
        const dist = Math.abs(_monsterPosition.x - playerX) + Math.abs(_monsterPosition.y - playerY);

        // Monster patrols and searches
        if ($gameParty._isHiding) {
            // Monster loses track
            if (Math.random() < 0.1) {
                _monsterPosition.x += Math.random() > 0.5 ? 1 : -1;
                _monsterPosition.y += Math.random() > 0.5 ? 1 : -1;
            }
            $gameParty._monsterAlert = Math.max(0, $gameParty._monsterAlert - 1);
        } else if ($gameParty._flashlightOn) {
            // Flashlight attracts monster
            _monsterPosition.x += Math.sign(playerX - _monsterPosition.x) * 2;
            _monsterPosition.y += Math.sign(playerY - _monsterPosition.y) * 2;
            $gameParty._monsterAlert = Math.min(100, $gameParty._monsterAlert + 5);
        } else {
            // Monster wanders
            if (Math.random() < 0.3) {
                _monsterPosition.x += Math.sign(playerX - _monsterPosition.x);
                _monsterPosition.y += Math.sign(playerY - _monsterPosition.y);
            }
        }

        // Clamp monster position
        _monsterPosition.x = Math.max(0, Math.min(20, _monsterPosition.x));
        _monsterPosition.y = Math.max(0, Math.min(15, _monsterPosition.y));

        // Check if monster caught player
        if (dist <= 1 && !$gameParty._isHiding) {
            $gameParty.changeSanity(-25);
            $gameParty._monsterAlert = 100;
            $gameScreen.startFlash([255, 0, 0, 255], 60);
            $gameMessage.add('Something grabbed you in the dark!');
        }
    }

    // ---- Screen Effects ----
    function applySanityEffects() {
        const sanity = $gameParty._sanity;
        if (sanity <= 25) {
            // Screen distortion
            $gameScreen.setTone([0, 0, 0, 64], 30);
        } else if (sanity <= 50) {
            $gameScreen.setTone([0, 0, 0, 32], 30);
        } else {
            $gameScreen.setTone([0, 0, 0, 0], 30);
        }
    }

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'sanityChange': {
                    const result = $gameParty.changeSanity(parseInt(args[1]));
                    $gameVariables.setValue(6, ['stable', 'unstable', 'critical', 'broken'].indexOf(result));
                    break;
                }
                case 'flashlight':
                    if (args[1] === 'on') $gameParty._flashlightOn = true;
                    else if (args[1] === 'off') $gameParty._flashlightOn = false;
                    else $gameParty.toggleFlashlight();
                    break;
                case 'monsterNearby':
                    $gameParty._monsterAlert = args[1] === 'true' ? 80 : 0;
                    break;
                case 'hide':
                    $gameParty.hide();
                    break;
                case 'unhide':
                    $gameParty.unhide();
                    break;
                case 'itemFind':
                    $gameParty.gainItem($dataItems[parseInt(args[1])], 1);
                    break;
            }
        }
    };

    // Update monster and effects every frame
    const _Scene_Map_update = Scene_Map.prototype.update;
    Scene_Map.prototype.update = function() {
        _Scene_Map_update.call(this);
        updateMonsterAI();
        applySanityEffects();
    };
})();
