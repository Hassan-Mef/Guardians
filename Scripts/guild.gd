extends Control

@export var archor_scene: PackedScene = preload("res://Scenes/archor.tscn")
var selected_character: PackedScene = null

# Define the signal
signal character_selected(character: PackedScene)

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass  # Replace with function body.

func _on_archor_pressed() -> void:
	selected_character = archor_scene
	if selected_character:
		print("Archor is selected and scene is instanced.")
		emit_signal("character_selected", selected_character)
	else:
		print("Scene is not selected")



#extends Control
#
#@export var archor_scene: PackedScene = preload("res://Scenes/archor.tscn")
#var selected_character: PackedScene = null
#
## Declare a signal to notify selection
#signal character_selected(character: PackedScene)
#
#func _ready() -> void:
	#pass
#
#func _on_archor_pressed() -> void:
	#selected_character = archor_scene
	#if selected_character:
		#print("Archor is selected.")
		#emit_signal("character_selected", selected_character)  # Emit signal when character is selected
	#else:
		#print("Scene is not selected.")
#


#extends Control
#
#@export var archor_scene: PackedScene = preload("res://Scenes/archor.tscn")
#var selected_character: PackedScene = null
## Called when the node enters the scene tree for the first time.
#func _ready() -> void:
	#pass # Replace with function body.
	#
#func _on_archor_pressed() -> void:
	#selected_character = archor_scene
	#if(selected_character):
		#print("archor is selected")
		#
		#print("Archor is selected and scene is instanced.")
	#else:
		#print("scene is not selected")
		
#		section of if write here
		#var character_instance = selected_character.instantiate()
		#get_tree().current_scene.add_child(character_instance)
		#get_parent().get_node("Bullets").add_child(character_instance)
		#get_parent().get_node("/root/Game/Characters").add_child(character_instance)
		#end here
		
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
