-- Player module for Solar2D shoot-em-up
local M = {}

local physics = require("physics")
local player
local hp = 3
local maxHP = 3

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

function M.move(dx, dy)
    if player then
        player.moveX = dx * 5
        player.moveY = dy * 5
    end
end

function M.update()
    if player then
        player.x = player.x + player.moveX
        player.y = player.y + player.moveY
        player.x = math.max(16, math.min(display.contentWidth - 16, player.x))
        player.y = math.max(16, math.min(display.contentHeight - 16, player.y))
    end
end

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

function M.getHP()
    return hp
end

function M.getPosition()
    if player then return player.x, player.y end
    return 0, 0
end

function M.reset()
    hp = maxHP
    if player then
        player.alpha = 1
        player.moveX = 0
        player.moveY = 0
    end
end

function M.destroy()
    if player then
        display.remove(player)
        player = nil
    end
end

return M
