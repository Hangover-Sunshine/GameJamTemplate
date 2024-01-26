extends Node2D

@export var next_scene:String

# Called when the node enters the scene tree for the first time.
func _ready():
	GlobalSignals.connect("scene_loaded", _scene_loaded)
##

func _scene_loaded(new_scene:String):
	print(name)
	print("new_scene:", name)
	if new_scene != name:
		queue_free()
	##
##

func _on_button_pressed():
	GlobalSignals.emit_signal("load_scene", next_scene)
##
