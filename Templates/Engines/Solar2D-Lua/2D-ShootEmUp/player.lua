-- Player module for Solar2D shoot-em-up
-- Provides player creation, movement, damage handling, and cleanup
local M = {}

local physics = require("physics")
local player
local hp = 3
local maxHP = 3

-- Creates the player ship at the given position with physics body
function M.create(x, y)
    player = display.newImageRect("ship.png", 32, 32)
    player.x = x
    player.y = y

    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        radius = 14
    })
    player.isFixedRotation = true
    player.moveX = 0
    player.moveY = 0
    hp = maxHP

    return player
end

-- Sets movement velocity based on direction input
function M.move(dx, dy)
    if player then
        player.moveX = dx * 5
        player.moveY = dy * 5
    end
end

-- Updates player position and clamps to screen bounds
function M.update()
    if player then
        player.x = player.x + player.moveX
        player.y = player.y + player.moveY
        player.x = math.max(16, math.min(display.contentWidth - 16, player.x))
        player.y = math.max(16, math.min(display.contentHeight - 16, player.y))
    end
end

-- Reduces HP by amount, flashes player, returns remaining HP
function M.takeDamage(amount)
    hp = hp - (amount or 1)
    if player then
        player.alpha = 0.4
        timer.performWithDelay(100, function()
            if player then player.alpha = 1 end
        end)
    end
    return hp
end

-- Returns current HP value
function M.getHP()
    return hp
end

-- Returns player position or (0, 0) if nil
function M.getPosition()
    if player then return player.x, player.y end
    return 0, 0
end

-- Resets HP, alpha, and movement to initial state
function M.reset()
    hp = maxHP
    if player then
        player.alpha = 1
        player.moveX = 0
        player.moveY = 0
    end
end

-- Removes player display object and sets reference to nil
function M.destroy()
    if player then
        display.remove(player)
        player = nil
    end
end

return M
