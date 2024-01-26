@tool
extends Node2D
## A sound queue that plays sound in a 2D space.
class_name SoundQueue2D

## Number of sounds to queue at one time.
@export var count:int = 1

var m_next:int = 0
var m_audio_stream_player:Array[AudioStreamPlayer2D]

func _ready():
	if get_child_count() == 0:
		print("No AudioStreamPlayer2D was found!")
		return
	##
	
	var child = get_child(0)
	
	if child is AudioStreamPlayer2D:
		m_audio_stream_player.append(child)
		
		for i in range(count):
			var duplicate:AudioStreamPlayer2D = child.duplicate()
			add_child(duplicate)
			m_audio_stream_player.append(duplicate)
		##
	##
##

func _get_configuration_warnings():
	if get_child_count() == 0:
		return ["SoundQueue2D needs at least one child AudioStreamPlayer2D!"]
	##
	
	if !(get_child(0) is AudioStreamPlayer2D):
		return ["First child is not an AudioStreamPlayer2D!"]
	##
	
	return self._get_configuration_warnings()
##

## Play the next available sound from the queue.
## Returns: A boolean if the Queue was able to play a sound.
func play_sound() -> bool:
	if m_audio_stream_player[m_next].playing == false:
		m_audio_stream_player[m_next].play()
		m_next = (m_next + 1) % len(m_audio_stream_player)
		return true
	##
	return false
##
