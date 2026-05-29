# Card game logic manager: deck, hand, discard, mana, shields, damage, and effects
extends Node

@export var starting_deck: Array[Card] = []
@export var max_hand_size: int = 7
@export var max_mana: int = 3

var draw_pile: Array[Card] = []
var hand: Array[Card] = []
var discard_pile: Array[Card] = []
var current_mana: int = 0
var current_health: int = 30
var max_health: int = 30
var shield: int = 0

signal hand_updated(hand: Array)
signal mana_updated(current: int, max: int)
signal health_updated(current: int, max: int)
signal combat_end(victory: bool)

func _ready() -> void:
	shuffle_deck()

func shuffle_deck() -> void:
	draw_pile = starting_deck.duplicate()
	draw_pile.shuffle()
	hand.clear()
	discard_pile.clear()

func start_turn() -> void:
	current_mana = max_mana
	draw_cards(1)
	emit_signals()

func draw_cards(count: int) -> void:
	for i in range(count):
		if hand.size() >= max_hand_size:
			return
		if draw_pile.is_empty():
			if discard_pile.is_empty():
				return
			reshuffle_discard()
		hand.append(draw_pile.pop_front())
	emit_signals()

func reshuffle_discard() -> void:
	draw_pile = discard_pile.duplicate()
	draw_pile.shuffle()
	discard_pile.clear()

func play_card(card_index: int, target: Node = null) -> bool:
	if card_index >= hand.size():
		return false

	var card: Card = hand[card_index]
	if current_mana < card.cost:
		return false

	current_mana -= card.cost
	hand.remove_at(card_index)
	apply_card_effects(card, target)
	discard_pile.append(card)
	emit_signals()
	return true

func apply_card_effects(card: Card, target: Node) -> void:
	if card.damage > 0 and target != null and target.has_method("take_damage"):
		target.take_damage(card.damage)

	if card.heal > 0:
		current_health = min(current_health + card.heal, max_health)

	if card.shield > 0:
		shield += card.shield

	if card.draw_cards > 0:
		draw_cards(card.draw_cards)

func take_damage(amount: int) -> void:
	if shield > 0:
		var blocked: int = min(shield, amount)
		shield -= blocked
		amount -= blocked

	current_health -= amount
	if current_health <= 0:
		current_health = 0
		combat_end.emit(false)
	emit_signals()

func end_turn() -> void:
	discard_hand()

func discard_hand() -> void:
	for card in hand:
		discard_pile.append(card)
	hand.clear()
	emit_signals()

func emit_signals() -> void:
	hand_updated.emit(hand)
	mana_updated.emit(current_mana, max_mana)
	health_updated.emit(current_health, max_health)
