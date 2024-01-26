@tool
extends Node2D
## A pool for individual sounds in 2D, meant for short sounds. If you have sounds
## that could last a while, then consider using a SoundQueuePool instead.
class_name SoundPool2D

@export_group("Random Sounds")
## Prevent the same sound from playing twice in a row.
@export var stopRepeatingSounds:bool = false

## The max attempts to try and select a sound in the event one is already playing.
@export var maxAttempts:int = 5

var m_prev_index:int = -1
var m_audio_stream_player:Array[AudioStreamPlayer2D]

func _ready():
	var children = get_children()
	
	for ci in range(get_child_count()):
		if children[ci] is AudioStreamPlayer2D:
			m_audio_stream_player.append(children[ci])
		else:
			print("Non-AudioStreamPlayer2D child detected at index %s.", ci)
		##
	##
##

func play_random_sound() -> bool:
	var index:int = randi() % len(m_audio_stream_player)
	var attempts:int = 0
	
	if stopRepeatingSounds:
		while (index == m_prev_index or m_audio_stream_player[index].playing) and attempts < maxAttempts:
			attempts += 1
			index = randi() % len(m_audio_stream_player)
		##
		
		if m_audio_stream_player[index].playing == false:
			m_audio_stream_player[index].play()
			m_prev_index = index
			return true
		##
	else:
		while m_audio_stream_player[index].playing and attempts < maxAttempts:
			attempts += 1
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
