/*:
 * @target MZ
 * @plugindesc Puzzle Mechanics UI v1.0.0
 * @author BuildYourApp
 *
 * @help
 * Puzzle UI rendering for sliding puzzles and code breaking.
 */

(function() {
    const pluginName = 'PuzzleRPG_UI';

    // ---- Sliding Puzzle Window ----
    function Window_SlidingPuzzle() {
        this.initialize.apply(this, arguments);
    }
    Window_SlidingPuzzle.prototype = Object.create(Window_Base.prototype);
    Window_SlidingPuzzle.prototype.constructor = Window_SlidingPuzzle;

    Window_SlidingPuzzle.prototype.initialize = function() {
        Window_Base.prototype.initialize.call(this, 0, 0, 360, 420);
        this._tileSize = 100;
    };

    Window_SlidingPuzzle.prototype.update = function() {
        Window_Base.prototype.update.call(this);
        this.refresh();
    };

    Window_SlidingPuzzle.prototype.refresh = function() {
        this.contents.clear();
        const state = window._puzzleState;
        if (!state) return;

        const grid = state.getSlidingGrid();
        for (let y = 0; y < grid.length; y++) {
            for (let x = 0; x < grid[y].length; x++) {
                const val = grid[y][x];
                const px = x * this._tileSize + 10;
                const py = y * this._tileSize + 10;

                if (val === 0) {
                    this.drawText('', px, py, this._tileSize, 'center');
                } else {
                    this.drawText(String(val), px + 20, py + 20, this._tileSize - 40, 'center');
                }
            }
        }

        if (state.getSlidingSolved()) {
            this.drawText('SOLVED!', 0, 360, 340, 'center');
        }
    };

    // ---- Code Breaking Window ----
    function Window_CodeBreak() {
        this.initialize.apply(this, arguments);
    }
    Window_CodeBreak.prototype = Object.create(Window_Base.prototype);
    Window_CodeBreak.prototype.constructor = Window_CodeBreak;

    Window_CodeBreak.prototype.initialize = function() {
        Window_Base.prototype.initialize.call(this, 0, 0, 400, 300);
    };

    Window_CodeBreak.prototype.refresh = function() {
        this.contents.clear();
        const state = window._puzzleState;
        if (!state) return;

        const result = state.getCodeResult();
        this.drawText(`Code: ${state.getCode()}`, 0, 0, 380, 'left');
        this.drawText(`Attempts: ${result.attempts}/${result.max}`, 0, 30, 380, 'left');

        if (result.solved) {
            this.drawText('ACCESS GRANTED', 0, 60, 380, 'center');
        } else if (result.attempts >= result.max) {
            this.drawText('ACCESS DENIED - LOCKED OUT', 0, 60, 380, 'center');
        } else {
            this.drawText(`Enter 4-digit code (0-9)`, 0, 60, 380, 'left');
            this.drawText(`Press SHIFT to confirm`, 0, 90, 380, 'left');
        }
    };

    // Register as available window type
    Window_SlidingPuzzle.prototype.constructor = Window_SlidingPuzzle;
    Window_CodeBreak.prototype.constructor = Window_CodeBreak;
})();
