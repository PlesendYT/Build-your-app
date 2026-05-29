-- Card module for Solar2D card game
-- Provides card type definitions, visual creation, deck generation, shuffling, and drawing
local M = {}

-- Card data: color and icon per type (attack, defense, spell)
M.cardTypes = {
    attack = { color = {1, 0.4, 0.4}, icon = "sword" },
    defense = { color = {0.4, 0.6, 1}, icon = "shield" },
    spell = { color = {0.4, 1, 0.4}, icon = "sparkle" },
}

function M.createCardVisual(card, x, y, width, height)
    local group = display.newGroup()

    local bg = display.newRoundedRect(group, x, y, width, height, 6)
    bg:setFillColor(card.color[1], card.color[2], card.color[3])

    local nameText = display.newText(group, card.name, x, y - height / 2 + 20, nil, 14)
    nameText:setFillColor(1, 1, 1)

    local costText = display.newText(group, "Cost: " .. card.cost, x, y - 8, nil, 12)
    costText:setFillColor(1, 1, 1)

    local valText = display.newText(group, "Val: " .. card.value, x, y + height / 2 - 20, nil, 12)
    valText:setFillColor(1, 1, 1)

    return group
end

function M.createDeck(count)
    local d = {}
    local names = {"Strike", "Defend", "Fireball", "Heal", "Lightning", "Shield Bash"}
    local types = {"attack", "attack", "attack", "defense", "defense", "spell"}
    local costs = {1, 1, 2, 2, 3, 2}
    local values = {6, 6, 12, 8, 18, 8}

    for _ = 1, (count or 10) do
        local idx = math.random(#names)
        table.insert(d, {
            name = names[idx],
            cost = costs[idx],
            type = types[idx],
            value = values[idx],
            color = M.cardTypes[types[idx]].color
        })
    end
    return d
end

-- Fisher-Yates shuffle for a deck table
function M.shuffle(d)
    for i = #d, 2, -1 do
        local j = math.random(i)
        d[i], d[j] = d[j], d[i]
    end
end

-- Draws the top card from deck; returns nil if empty
function M.draw(d)
    if #d > 0 then
        return table.remove(d)
    end
    return nil
end

return M
