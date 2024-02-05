extends Node

# NOTE: this implements a single, global keymapping system for ALL saves/players on the system running.
#  TODO: maybe something that allows players to save different mappings out?

var KEY_MAPS_DIR:String = "user://player/keybindings.data"

var keymaps:Dictionary = {}

# Called when the node enters the scene tree for the first time.
func _ready():
	for action in InputMap.get_actions():
		keymaps[action] = InputMap.action_get_events(action)
	##
	
	if DirAccess.dir_exists_absolute(KEY_MAPS_DIR):
		load_key_mapping()
	else:
		DirAccess.make_dir_absolute("user://player/")
		save_key_mapping()
	##
##

func load_key_mapping():
	if not FileAccess.file_exists(KEY_MAPS_DIR):
		save_key_mapping()
		return
	##
	
	var contents = FileAccess.open(KEY_MAPS_DIR, FileAccess.READ)
	var temp = contents.get_var(true) as Dictionary
	contents.close()
	
	for action in temp.keys():
		keymaps[action] = temp[action]
		InputMap.erase_action(action)
		InputMap.action_add_event(action, keymaps[action])
	##
##

func save_key_mapping():
	var file:FileAccess = FileAccess.open(KEY_MAPS_DIR, FileAccess.WRITE)
	file.store_var(keymaps, true)
	file.close()
##
