-- Obstacle module for Solar2D endless runner
local M = {}

local physics = require("physics")

local function createObstacle(img, x, y, w, h, obsType)
    local obs = display.newImageRect(img, w, h)
    obs.x = x
    obs.y = y
    obs.obstacleType = obsType
    obs.name = "obstacle"

    physics.addBody(obs, "kinematic", {
        isSensor = true,
        box = { halfWidth = w / 2 - 2, halfHeight = h / 2 - 2 }
    })
    return obs
end

function M.createLow(x, y)
    return createObstacle("obstacle_low.png", x, y, 24, 36, "low")
end

function M.createHigh(x, y)
    return createObstacle("obstacle_high.png", x, y, 32, 24, "high")
end

function M.createDouble(x, y)
    return createObstacle("obstacle_double.png", x, y, 48, 48, "double")
end

function M.update(obstacles, speed, dt)
    for i = #obstacles, 1, -1 do
        local obs = obstacles[i]
        obs.x = obs.x - speed * dt
        if obs.x < -100 then
            display.remove(obs)
            table.remove(obstacles, i)
        end
    end
end

function M.clear(obstacles)
    for _, obs in ipairs(obstacles) do
        display.remove(obs)
    end
end

return M
