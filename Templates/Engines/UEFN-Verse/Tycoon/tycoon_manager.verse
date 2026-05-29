# Tycoon manager mode: players earn passive income from generators and purchase upgrades.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Tycoon device class managing money, generators, and income loops.
tycoon_manager := class(creative_device):
    @editable
    GeneratorDevices : []item_spawner_device = array{}
    @editable
    VendingDevices : []item_spawner_device = array{}
    @editable
    BaseIncome : float = 1.0
    @editable
    GeneratorCost : int = 100
    
    var PlayerMoney : [agent]int = map{}
    var PlayerGenerators : [agent]int = map{}
    var PlayerGeneratorLevels : [agent]int = map{}
    
    # Subscribes to player joins and starts the income loop.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { IncomeLoop[] }
    
    # Gives a new player starting money and a base generator.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerMoney[Agent] = 50
        set PlayerGenerators[Agent] = 1
        set PlayerGeneratorLevels[Agent] = 1
        Print("Welcome to Tycoon! Start with $50")
    
    # Every 5 seconds, grants income to each player based on their generators and levels.
    IncomeLoop()<suspends>() : void =
        loop:
            Sleep(5.0)
            for (Agent : PlayerMoney, Money : PlayerMoney[Agent]):
                Generators := PlayerGenerators[Agent]
                Level := PlayerGeneratorLevels[Agent]
                Income := BaseIncome * Generators * Level
                set PlayerMoney[Agent] = Money + Income
                Print("{Agent} earned ${Income}")
    
    # Purchases a generator for the player if they have enough money.
    BuyGenerator(Agent : agent) : void =
        if (Money := PlayerMoney[Agent]):
            if Money >= GeneratorCost:
                set PlayerMoney[Agent] = Money - GeneratorCost
                set PlayerGenerators[Agent] = PlayerGenerators[Agent] + 1
                Print("{Agent} bought a generator!")
    
    # Upgrades all of a player's generators to the next level.
    UpgradeGenerators(Agent : agent) : void =
        UpgradeCost := PlayerGeneratorLevels[Agent] * 200
        if (Money := PlayerMoney[Agent]):
            if Money >= UpgradeCost:
                set PlayerMoney[Agent] = Money - UpgradeCost
                set PlayerGeneratorLevels[Agent] = PlayerGeneratorLevels[Agent] + 1
                Print("{Agent} upgraded generators to level {PlayerGeneratorLevels[Agent]}!")
    
    # Adds the item value to the player's money when they sell an item.
    SellItem(Agent : agent, ItemValue : int) : void =
        if (Money := PlayerMoney[Agent]):
            set PlayerMoney[Agent] = Money + ItemValue
    
    # Returns the current money for a given player.
    GetPlayerMoney(Agent : agent) : int =
        PlayerMoney[Agent]
