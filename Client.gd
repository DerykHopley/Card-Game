extends Node

onready var Card = preload("res://Card.tscn")
const colyseus = preload("res://addons/godot_colyseus/lib/colyseus.gd")
var room: colyseus.Room
var roomId = null
#set up basic schema
class FieldState extends colyseus.Schema:
	static func define_fields():
		return [
			colyseus.Field.new("left", colyseus.STRING),
			colyseus.Field.new("mid", colyseus.STRING),
			colyseus.Field.new("right", colyseus.STRING),
		]
class GameState extends colyseus.Schema:
	static func define_fields():
		return [
			colyseus.Field.new("roomFields", colyseus.MAP, FieldState),
		]

func _ready():
	#set up client
	var client = colyseus.Client.new("ws://localhost:2567")
	var promise = client.join_or_create(GameState, "game")
	yield(promise, "completed")
	if promise.get_state() == promise.State.Failed:
		print("Failed")
		return
	var room: colyseus.Room = promise.get_result()

	room.on_message("server-message").on(funcref(self, "_on_server_message"))
	room.on_message("game-message").on(funcref(self, "_on_game_message"))
	room.on_message("client-request").on(funcref(self, "_on_client_request"))
	self.room = room

#func _init():

#signal to request GameManager to instance player cards
signal draw_cards

#signal to request GameManager to instance player cards
signal evaluate_turn

#signal to request GameManager to instance player cards
signal game_start

#signal to request GameManager to instance player cards
signal deck_empty

#signal to request GameManager to render opponent cards
signal render_cards(count)

#signal to request GameManager to handle dropped card
signal dropped_card

#signal to request GameManager to handle dropped card
signal evaluate_score

#log server message to console
func _on_server_message(data):
	if (data.type == "game_start"): 
		print("Server Message:", data.FieldState.left, data.FieldState.mid, data.FieldState.right)
		emit_signal("game_start", data.FieldState.left, data.FieldState.mid, data.FieldState.right)
	else:
		print(data)
	

#log game message to console
func _on_game_message(data):
	print(data)
	print ("Game Message: " + data.type)
	if (data.type == "cards_drawn"):
		print ("Card Count: " + str(data))
		#emit_signal("render_cards", data.count)
	elif (data.type == "card_dropped"):
		emit_signal("dropped_card", data.zone, data.subzone, data.card)
	elif (data.type == "card_addeded"):
		emit_signal("render_cards", data.count)

#log client request to console and draw cards
func _on_client_request(data):
	print (data)
	#startgame /first turn
	if (data.kind == "start_turn"):
		emit_signal("draw_cards",3)
	#next turn
	elif (data.kind == "end_turn"):
		emit_signal("evaluate_turn")

#send message to server that cards have been drawn
func _on_cards_drawn(i):
	var cards_drawn = {"type": "cards_drawn","count": i}
	room.send("game-message", cards_drawn)

#send message to server that card has been dropped
func _on_card_dropped(zone, subzone, card):
	var card_dropped = {
		"type": "card_dropped",
		"zone": zone, 
		"subzone": subzone, 
		"card": card
	}
	room.send("game-message", card_dropped)

#send message to server that card has been dropped
func _on_deck_empty():
	room.send("game-message", {"type": "deck_empty"})

func _on_end_turn_down():
	print ("End Turn")
	room.send("client-request", "end_turn")

func _on_evaluate_score():
	print ("Evaluate Score")
	emit_signal("evaluate_score")
