-- Solar2D Card Game
-- Main game controller: manages deck, hand, mana, turns, and card combat between player and enemy
display.setStatusBar(display.HiddenStatusBar)

local deck = {}
local hand = {}
local discard = {}
local enemyDeck = {}
local enemyHand = {}
local playerHP = 30
local enemyHP = 30
local mana = 1
local maxMana = 1
local turn = 1
local phase = "player"
local gameOver = false

-- Card definitions: name, mana cost, type (attack/defense/spell), value, and display color
local cardDefs = {
    { name = "Strike", cost = 1, type = "attack", value = 6, color = {1, 0.4, 0.4} },
    { name = "Strike", cost = 1, type = "attack", value = 6, color = {1, 0.4, 0.4} },
    { name = "Strike", cost = 1, type = "attack", value = 6, color = {1, 0.4, 0.4} },
    { name = "Defend", cost = 1, type = "defense", value = 5, color = {0.4, 0.6, 1} },
    { name = "Defend", cost = 1, type = "defense", value = 5, color = {0.4, 0.6, 1} },
    { name = "Defend", cost = 1, type = "defense", value = 5, color = {0.4, 0.6, 1} },
    { name = "Fireball", cost = 2, type = "attack", value = 12, color = {1, 0.6, 0.2} },
    { name = "Heal", cost = 2, type = "spell", value = 8, color = {0.4, 1, 0.4} },
    { name = "Lightning", cost = 3, type = "attack", value = 18, color = {1, 0.8, 0.2} },
    { name = "Shield Bash", cost = 2, type = "attack", value = 8, color = {0.6, 0.6, 1} },
}

local cardObjects = {}
local enemyCardObjects = {}

function shuffleDeck(d)
    for i = #d, 2, -1 do
        local j = math.random(i)
        d[i], d[j] = d[j], d[i]
    end
end

function initGame()
    deck = {}
    hand = {}
    discard = {}
    enemyDeck = {}

    for _, def in ipairs(cardDefs) do
        local card = { name = def.name, cost = def.cost, type = def.type, value = def.value, color = def.color }
        table.insert(deck, card)
        table.insert(enemyDeck, { name = def.name, cost = def.cost, type = def.type, value = math.ceil(def.value * 0.7), color = {0.8, 0.2, 0.2} })
    end

    shuffleDeck(deck)
    shuffleDeck(enemyDeck)

    playerHP = 30
    enemyHP = 30
    mana = 1
    maxMana = 1
    turn = 1
    phase = "player"
    gameOver = false

    drawHand(3)
    drawEnemyHand(3)
    renderUI()
    renderHand()
    renderEnemyCards()
end

function drawCard()
    if #deck == 0 then
        deck = discard
        discard = {}
        shuffleDeck(deck)
    end
    if #deck > 0 then
        return table.remove(deck)
    end
    return nil
end

function drawHand(count)
    for _ = 1, count do
        local card = drawCard()
        if card then table.insert(hand, card) end
    end
end

function drawEnemyHand(count)
    for _ = 1, count do
        if #enemyDeck > 0 then
            table.insert(enemyHand, table.remove(enemyDeck))
        end
    end
end

function playCard(index)
    if phase ~= "player" or gameOver then return false end

    local card = hand[index]
    if not card then return false end
    if mana < card.cost then return false end

    mana = mana - card.cost

    if card.type == "attack" then
        enemyHP = enemyHP - card.value
    elseif card.type == "spell" then
        playerHP = math.min(30, playerHP + card.value)
    end

    table.insert(discard, card)
    table.remove(hand, index)

    if enemyHP <= 0 then
        gameOver = true
        showResult("Victory!")
    end

    renderUI()
    renderHand()
    return true
end

function endTurn()
    if phase ~= "player" then return end

    -- Discard hand
    for _, card in ipairs(hand) do
        table.insert(discard, card)
    end
    hand = {}

    -- Enemy turn
    enemyTurn()

    -- New turn setup
    turn = turn + 1
    maxMana = math.min(10, maxMana + 1)
    mana = maxMana
    phase = "player"
    drawHand(5)

    renderUI()
    renderHand()
    renderEnemyCards()
end

function enemyTurn()
    -- Enemy plays cards
    local playable = {}
    for i, card in ipairs(enemyHand) do
        if card.cost <= 3 then
            table.insert(playable, { index = i, card = card })
        end
    end

    for _, pc in ipairs(playable) do
        if pc.card.type == "attack" then
            playerHP = playerHP - pc.card.value
        end
        table.remove(enemyHand, pc.index)
    end

    -- Enemy draws
    drawEnemyHand(2)

    if playerHP <= 0 then
        gameOver = true
        showResult("Defeat!")
    end
end

function showResult(text)
    local overlay = display.newRect(
        display.contentCenterX, display.contentCenterY,
        display.contentWidth, display.contentHeight
    )
    overlay:setFillColor(0, 0, 0, 0.7)
    display.newText(text, display.contentCenterX, display.contentCenterY - 20, nil, 48)
    display.newText("Press SPACE to restart", display.contentCenterX, display.contentCenterY + 30, nil, 18)
end

-- Renders the game UI showing HP, mana, enemy HP, turn info, deck/discard counts, and end turn button
function renderUI()
    if uiGroup then uiGroup:removeSelf() end
    uiGroup = display.newGroup()

    display.newText("HP: " .. playerHP, 20, 20, nil, 20):setFillColor(1, 0.4, 0.4)
    display.newText("Mana: " .. mana .. "/" .. maxMana, 20, 50, nil, 18)
    display.newText("Enemy HP: " .. enemyHP, display.contentWidth - 150, 20, nil, 20):setFillColor(0.8, 0.2, 0.2)
    display.newText("Turn: " .. turn, display.contentCenterX, 20, nil, 18)
    display.newText("Phase: " .. phase, display.contentCenterX, 45, nil, 16)
    display.newText("Deck: " .. #deck, 20, 80, nil, 14)
    display.newText("Discard: " .. #discard, 20, 100, nil, 14)

    if phase == "player" then
        local endBtn = display.newRect(display.contentWidth - 60, display.contentHeight - 30, 100, 30)
        endBtn:setFillColor(0.6, 0.2, 0.2)
        display.newText("End Turn", display.contentWidth - 60, display.contentHeight - 30, nil, 14):setFillColor(1, 1, 1)
        endBtn:addEventListener("tap", function() endTurn() end)
    end
end

function renderHand()
    if handGroup then handGroup:removeSelf() end
    handGroup = display.newGroup()

    local startX = display.contentCenterX - (#hand * 40)
    for i, card in ipairs(hand) do
        local cx = startX + (i - 1) * 80 + 40
        local cy = display.contentHeight - 80

        local bg = display.newRoundedRect(cx, cy, 70, 100, 6)
        bg:setFillColor(card.color[1], card.color[2], card.color[3])
        bg.strokeWidth = 2
        bg:setStrokeColor(1, 1, 1)

        display.newText(card.name, cx, cy - 25, nil, 12):setFillColor(1, 1, 1)
        display.newText("Cost: " .. card.cost, cx, cy + 5, nil, 11):setFillColor(1, 1, 1)
        display.newText("Val: " .. card.value, cx, cy + 22, nil, 11):setFillColor(1, 1, 1)

        bg:addEventListener("tap", function()
            playCard(i)
        end)

        handGroup:insert(bg)
    end
end

function renderEnemyCards()
    if enemyGroup then enemyGroup:removeSelf() end
    enemyGroup = display.newGroup()

    local startX = display.contentCenterX - (#enemyHand * 20)
    for i, card in ipairs(enemyHand) do
        local cx = startX + (i - 1) * 40 + 20
        local cy = 120

        local bg = display.newRoundedRect(cx, cy, 30, 44, 4)
        bg:setFillColor(0.6, 0.1, 0.1)
        bg.alpha = 0.8
        enemyGroup:insert(bg)
    end
end

-- Handles SPACE key to restart the game when game over
function handleKey(event)
    if event.keyName == "space" and event.phase == "down" and gameOver then
        if uiGroup then uiGroup:removeSelf() end
        if handGroup then handGroup:removeSelf() end
        if enemyGroup then enemyGroup:removeSelf() end
        initGame()
    end
    return true
end
Runtime:addEventListener("key", handleKey)

initGame()
