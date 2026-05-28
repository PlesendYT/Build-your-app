-- Player module for Solar2D platformer
local M = {}

local player
local physics = require("physics")

local PLAYER_SPEED = 4
local JUMP_FORCE = -12
local GRAVITY = 25

function M.create(x, y)
    player = display.newImageRect("player.png", 32, 48)
    player.x = x
    player.y = y
    player.xScale = 1

    physics.addBody(player, "dynamic", {
        density = 1.0,
        friction = 0.0,
        bounce = 0.0,
        radius = 14
    })
    player.isFixedRotation = true
    player.grounded = false
    player.jumps = 2
    player.maxJumps = 2
    player.speedX = 0

    return player
end

function M.move(dx)
    if player then
        player.speedX = dx * PLAYER_SPEED
        if dx > 0 then player.xScale = 1 end
        if dx < 0 then player.xScale = -1 end
    end
end

function M.jump()
    if player and player.jumps > 0 then
        player:setLinearVelocity(0, 0)
        player:applyLinearImpulse(0, JUMP_FORCE, player.x, player.y)
        player.jumps = player.jumps - 1
        return true
    end
    return false
end

function M.stompBounce()
    if player then
        player:setLinearVelocity(0, 0)
        player:applyLinearImpulse(0, JUMP_FORCE * 0.6, player.x, player.y)
        player.jumps = math.max(player.jumps, 1)
    end
end

function M.resetJumps()
    if player then
        player.jumps = player.maxJumps
        player.grounded = true
    end
end

function M.getPosition()
    if player then
        return player.x, player.y
    end
    return 0, 0
end

function M.setPosition(x, y)
    if player then
        player.x = x
        player.y = y
        player:setLinearVelocity(0, 0)
    end
end

function M.destroy()
    if player then
        display.remove(player)
        player = nil
    end
end

return M
