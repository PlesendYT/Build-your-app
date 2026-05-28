using Godot;
using System;
using System.Collections.Generic;

public partial class Card : Resource
{
    [Export] public string CardName { get; set; } = "Basic Attack";
    [Export] public string Description { get; set; } = "Deal 5 damage";
    [Export] public int Cost { get; set; } = 1;
    [Export] public string CardType { get; set; } = "attack";
    [Export] public int Damage { get; set; } = 5;
    [Export] public int Heal { get; set; } = 0;
    [Export] public int Shield { get; set; } = 0;
    [Export] public int DrawCards { get; set; } = 0;
    [Export] public string Rarity { get; set; } = "common";
}
