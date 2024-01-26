@tool
extends Node
## A pool for queues of sounds in a stereo manner, meant for short sounds.
class_name SoundQueuePool

@export_group("Random Sounds")
## Prevent the same sound from playing twice in a row.
@export var stopRepeatingSounds:bool = false

var m_prev_index:int = -1
var m_audio_stream_player:Array[SoundQueue]

func _ready():
	var children = get_children()
	
	for ci in range(get_child_count()):
		if children[ci] is SoundQueue:
			m_audio_stream_player.append(children[ci])
		else:
			print("Non-SoundQueue child detected at index %s.", ci)
		##
	##
##

func play_random_sound() -> bool:
	var index:int = randi() % len(m_audio_stream_player)
	
	if stopRepeatingSounds:
		while index == m_prev_index or m_audio_stream_player[index].playing:
			index = randi() % len(m_audio_stream_player)
		##
		
		if m_audio_stream_player[index].playing == false:
			m_audio_stream_player[index].play()
			m_prev_index = index
			return true
		##
	else:
		while m_audio_stream_player[index].playing:
			index = randi() % len(m_audio_stream_player)
		##
		
		if m_audio_stream_player[index].playing == false:
			m_audio_stream_player[index].play()
			return true
		##
	##
	
	return false
##

func play_sound(index:int) -> bool:
	if m_audio_stream_player[index].playing == false:
		m_audio_stream_player[index].play()
		return true
	##
	
	return false
##
