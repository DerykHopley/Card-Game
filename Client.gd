extends Node

onready var Card = preload("res://Card.tscn")
const colyseus = preload("res://addons/godot_colyseus/lib/colyseus.gd")
var room: colyseus.Room

#set up basic schema
class GameState extends colyseus.Schema:
	static func define_fields():
		var mySynchronizedProperty = "Hello world"
		return [
			colyseus.Field.new("mySynchronizedProperty", colyseus.STRING, mySynchronizedProperty),
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

#signal to request GameManager to instance player cards
signal draw_cards

#signal to request GameManager to instance player cards
signal add_cards(count)

#signal to request GameManager to instance player cards
signal deck_empty()

#signal to request GameManager to render opponent cards
signal render_cards(count)

#signal to request GameManager to handle dropped card
signal dropped_card

#log server message to console
func _on_server_message(data):
	print(data)
#log game message to console
func _on_game_message(data):
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
	print ("Client Request: " + data.kind)
	if (data.kind == "draw"):
		emit_signal("draw_cards",3)
	elif (data.kind == "add"):
		emit_signal("draw_cards",1)

#send request to server to draw cards on button down
func _on_drawcards_down():
	print ("Button Draw Cards")
	room.send("client-request", "draw")

func _on_addcards_down():
	print ("Button Add Cards")
	room.send("client-request", "add")

#send message to server that cards have been drawn
func _on_cards_drawn(i):
	var cards_drawn = {"type": "cards_drawn","count": i
	}
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
