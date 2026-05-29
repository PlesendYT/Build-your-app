-- Player module for Solar2D platformer
-- Handles player physics, movement, jumping, double-jump, and stomp bounce
local M = {}

local player
local physics = require("physics")

local PLAYER_SPEED = 4
local JUMP_FORCE = -12
local GRAVITY = 25

-- Creates the player at (x, y) with physics body and double-jump capability
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

-- Sets horizontal speed and flips sprite based on direction
function M.move(dx)
    if player then
        player.speedX = dx * PLAYER_SPEED
        if dx > 0 then player.xScale = 1 end
        if dx < 0 then player.xScale = -1 end
    end
end

-- Makes the player jump if jumps remain; returns true if jump occurred
function M.jump()
    if player and player.jumps > 0 then
        player:setLinearVelocity(0, 0)
        player:applyLinearImpulse(0, JUMP_FORCE, player.x, player.y)
        player.jumps = player.jumps - 1
        return true
    end
    return false
end

-- Applies a smaller upward impulse after stomping an enemy
function M.stompBounce()
    if player then
        player:setLinearVelocity(0, 0)
        player:applyLinearImpulse(0, JUMP_FORCE * 0.6, player.x, player.y)
        player.jumps = math.max(player.jumps, 1)
    end
end

-- Resets jump count to max and sets grounded flag
function M.resetJumps()
    if player then
        player.jumps = player.maxJumps
        player.grounded = true
    end
end

-- Returns current player position or (0, 0) if nil
function M.getPosition()
    if player then
        return player.x, player.y
    end
    return 0, 0
end

-- Teleports player to (x, y) and resets velocity
function M.setPosition(x, y)
    if player then
        player.x = x
        player.y = y
        player:setLinearVelocity(0, 0)
    end
end

-- Removes player display object and clears reference
function M.destroy()
    if player then
        display.remove(player)
        player = nil
    end
end

return M
