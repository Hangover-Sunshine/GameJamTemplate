extends Node

@export var scene_folder_path:String = "scenes/"
@export var curr_scene:Node

@onready var transition_player:AnimationPlayer = $TransitionPlayer

var undo_load_overlay:bool = false

var scene_name:String
var scene_path:String

var fade_in:bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	GlobalSignals.connect("load_scene", _load_scene)
	set_process(false)
##

func _process(_delta):
	var progress = []
	
	var thread_status:int = ResourceLoader.load_threaded_get_status(scene_path, progress)
	
	# The following two if-statements are fail-safes because multi-threaded loading is tricky and it's best
	# 	to know what's happening with multi-threading stuff. These should never ever ever ever ever
	#	be touched if we have already loaded the scene, more if something goes wrong DURING loading.
	
	# if it's not done or loaded
	if thread_status != 1 or thread_status != 3:
		print("An error has occured loading %s!", scene_name)
		return # stop!
	##
	
	# if we're done, kind of redundant but better safe than sorry
	if thread_status == 3:
		return # no error, but don't continue -- we loaded the scene already
	##
	
	progress = progress[0]
	
	if progress == 1:
		# inform the prior scene it's time to clean up
		GlobalSignals.emit_signal("scene_loaded", scene_name)
		curr_scene.z_index = -100
		
		# get the new scene from the resource loader and instantiate it
		var new_scene = ResourceLoader.load_threaded_get(scene_path).instantiate()
		
		# the new scene is our current scene, we don't care what happens with the other one
		curr_scene = new_scene
		add_child(new_scene)
		
		# play the animation player and make sure it knows we're fading in
		fade_in = true
		transition_player.play("Fade")
		
		# stop from coming back here
		set_process(false)
	else:
		# do something else...
		print(progress)
	##
##

func _load_scene(scene:String):
	scene_path = "res://" + scene_folder_path + scene + ".tscn"
	scene_name = scene
	
	# async loading...
	var error = ResourceLoader.load_threaded_request(scene_path)
	
	# if there's an error, break out and report -- DO NOT CONTINUE!
	if error:
		print("Unable to load scene as a request: %s!", scene_path)
		return
	##
	
	GlobalSignals.emit("scene_transition_started")
	
	transition_player.play_backwards("Fade")
	fade_in = false
	
	set_process(true)
##

func _on_transition_player_animation_finished(anim_name):
	if fade_in:
		GlobalSignals.emit("scene_transition_done")
	##
##
