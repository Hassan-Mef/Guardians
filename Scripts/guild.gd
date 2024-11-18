extends Control

@export var archor_scene: PackedScene = preload("res://Scenes/archor.tscn")
var selected_character: PackedScene = null
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


## Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
	#pass
#

func _on_archor_pressed() -> void:
	selected_character = archor_scene
	if(selected_character):
		print("archor is selected")
		var character_instance = selected_character.instantiate()
		#get_tree().current_scene.add_child(character_instance)
		#get_parent().get_node("Bullets").add_child(character_instance)
		get_parent().get_node("/root/Game/Characters").add_child(character_instance)
		print("Archor is selected and scene is instanced.")
	#else:
		
	#get_tree().change_scene_to_file(selected_character)
	
	#get_tree().change_scene_to_file("res://Scenes/MainMenu.tscn")


#func _on_knight_pressed() -> void:
	#get_tree().quit()

#extends Control

# Array of character scenes
#@export var character_scenes: Array = []

#@export var knight_scene: PackedScene = preload()

# To store the selected character scene
#var selected_character_scene: PackedScene = null
#
#func _on_button_pressed(button_index):
	## Select a character when a button is clicked
	#if button_index >= 0 and button_index < character_scenes.size():
		#selected_character_scene = character_scenes[button_index]
		#print("Character selected: ", selected_character_scene)
