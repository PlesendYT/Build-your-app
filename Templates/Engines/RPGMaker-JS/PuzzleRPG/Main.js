/*:
 * @target MZ
 * @plugindesc Puzzle RPG Core v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Puzzle RPG with sliding puzzles, code breaking, and logic challenges.
 *
 * Plugin Commands:
 *   PuzzleRPG slidingPuzzle 3
 *   PuzzleRPG codeBreak 4
 *   PuzzleRPG logicPuzzle puzzle_id
 *   PuzzleRPG reward itemId count
 *
 * @command slidingPuzzle
 * @text Sliding Puzzle
 * @desc Start a sliding puzzle minigame
 * @arg size
 * @text Grid Size (3-5)
 * @type number
 */

(function() {
    const pluginName = 'PuzzleRPG';

    $gameParty._puzzlesSolved = $gameParty._puzzlesSolved || 0;
    $gameParty._puzzleAttempts = $gameParty._puzzleAttempts || {};

    // ---- Sliding Puzzle ----
    let _slidingGrid = [];
    let _slidingEmptyPos = { x: 0, y: 0 };
    let _slidingSolved = false;
    let _slidingSize = 3;

    function initSlidingPuzzle(size) {
        _slidingSize = size;
        _slidingSolved = false;
        _slidingGrid = [];
        let nums = [];
        for (let i = 0; i < size * size - 1; i++) nums.push(i + 1);
        nums.push(0); // 0 = empty

        // Fisher-Yates shuffle (ensuring solvability)
        for (let i = nums.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [nums[i], nums[j]] = [nums[j], nums[i]];
        }

        // Ensure solvable by counting inversions
        let inv = 0;
        for (let i = 0; i < nums.length; i++) {
            for (let j = i + 1; j < nums.length; j++) {
                if (nums[i] && nums[j] && nums[i] > nums[j]) inv++;
            }
        }
        if (inv % 2 !== 0) {
            // Swap two non-empty tiles
            [nums[0], nums[1]] = [nums[1], nums[0]];
        }

        for (let y = 0; y < size; y++) {
            _slidingGrid[y] = [];
            for (let x = 0; x < size; x++) {
                _slidingGrid[y][x] = nums[y * size + x];
                if (nums[y * size + x] === 0) {
                    _slidingEmptyPos = { x, y };
                }
            }
        }
    }

    function slideTile(dirX, dirY) {
        if (_slidingSolved) return false;

        const newX = _slidingEmptyPos.x + dirX;
        const newY = _slidingEmptyPos.y + dirY;

        if (newX < 0 || newX >= _slidingSize || newY < 0 || newY >= _slidingSize) {
            return false;
        }

        // Swap
        _slidingGrid[_slidingEmptyPos.y][_slidingEmptyPos.x] = _slidingGrid[newY][newX];
        _slidingGrid[newY][newX] = 0;
        _slidingEmptyPos = { x: newX, y: newY };

        // Check solved
        _slidingSolved = checkSlidingSolved();
        return true;
    }

    function checkSlidingSolved() {
        let expected = 1;
        for (let y = 0; y < _slidingSize; y++) {
            for (let x = 0; x < _slidingSize; x++) {
                if (y === _slidingSize - 1 && x === _slidingSize - 1) {
                    if (_slidingGrid[y][x] !== 0) return false;
                } else {
                    if (_slidingGrid[y][x] !== expected) return false;
                    expected++;
                }
            }
        }
        return true;
    }

    // ---- Code Breaking ----
    let _code = [];
    let _codeGuess = [];
    let _codeAttempts = 0;
    let _codeMaxAttempts = 10;
    let _codeSolved = false;
    let _codeLength = 4;

    function initCodeBreak(length) {
        _codeLength = length;
        _code = [];
        _codeGuess = [];
        _codeAttempts = 0;
        _codeSolved = false;
        _codeMaxAttempts = 8 + length;

        for (let i = 0; i < length; i++) {
            _code.push(Math.floor(Math.random() * 10));
        }
    }

    function guessCode(guess) {
        if (_codeSolved) return null;
        _codeAttempts++;

        let exact = 0;
        let partial = 0;
        const codeCopy = [..._code];
        const guessCopy = [...guess];

        // Exact matches
        for (let i = 0; i < _codeLength; i++) {
            if (guessCopy[i] === codeCopy[i]) {
                exact++;
                codeCopy[i] = -1;
                guessCopy[i] = -2;
            }
        }

        // Partial matches
        for (let i = 0; i < _codeLength; i++) {
            if (guessCopy[i] >= 0) {
                const idx = codeCopy.indexOf(guessCopy[i]);
                if (idx >= 0) {
                    partial++;
                    codeCopy[idx] = -1;
                }
            }
        }

        if (exact === _codeLength) _codeSolved = true;
        return { exact, partial, attempts: _codeAttempts, max: _codeMaxAttempts };
    }

    // ---- Logic Puzzles ----
    const logicPuzzles = {
        riddle1: {
            question: 'What has keys but can\'t open locks?',
            answer: 'piano',
            hint: 'Think musical...'
        },
        riddle2: {
            question: 'What gets wetter the more it dries?',
            answer: 'towel',
            hint: 'You use it after a shower.'
        },
        riddle3: {
            question: 'I speak without a mouth and hear without ears. What am I?',
            answer: 'echo',
            hint: 'Sound bouncing off walls...'
        }
    };

    function solveLogicPuzzle(puzzleId, answer) {
        const puzzle = logicPuzzles[puzzleId];
        if (!puzzle) return false;
        return answer.toLowerCase().trim() === puzzle.answer;
    }

    // ---- Plugin Commands ----
    const _Game_Interpreter_pluginCommand = Game_Interpreter.prototype.pluginCommand;
    Game_Interpreter.prototype.pluginCommand = function(command, args) {
        _Game_Interpreter_pluginCommand.call(this, command, args);
        if (command === pluginName) {
            switch (args[0]) {
                case 'slidingPuzzle':
                    initSlidingPuzzle(parseInt(args[1]) || 3);
                    $gameSwitches.setValue(11, false);
                    break;
                case 'codeBreak':
                    initCodeBreak(parseInt(args[1]) || 4);
                    $gameSwitches.setValue(12, false);
                    break;
                case 'logicPuzzle': {
                    const answer = args[2] || '';
                    const solved = solveLogicPuzzle(args[1], answer);
                    $gameSwitches.setValue(13, solved);
                    break;
                }
                case 'reward':
                    $gameParty.gainItem($dataItems[parseInt(args[1])], parseInt(args[2]));
                    $gameParty._puzzlesSolved++;
                    break;
            }
        }
    };

    // Expose puzzle state for scene rendering
    window._puzzleState = {
        getSlidingGrid: () => _slidingGrid,
        getSlidingSolved: () => _slidingSolved,
        slideTile: (x, y) => slideTile(x, y),
        getCodeResult: () => ({ codeLength: _codeLength, attempts: _codeAttempts, max: _codeMaxAttempts, solved: _codeSolved }),
        guessCode: (g) => guessCode(g),
        getCode: () => {
            if (_codeSolved) return _code.join('');
            return '*'.repeat(_codeLength);
        }
    };
})();
