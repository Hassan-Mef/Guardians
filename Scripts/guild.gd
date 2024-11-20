extends Control

@export var knight_scene : PackedScene

var selected_character : PackedScene = null
var map_instance : Node2D = null  # Variable to hold the map reference

signal character_selected(character : PackedScene)

# Called when the node enters the scene tree for the first time
func _ready() -> void:
	pass  # Replace with function body if needed

# Called when the knight button is pressed
func _on_knight_pressed() -> void:
	selected_character = knight_scene
	if selected_character:
		print("Knight is selected.")
		emit_signal("character_selected", selected_character)
	else:
		print("Knight scene is not selected.")

# This method will be called when the character is selected in Game.cs
func _on_character_selected(character : PackedScene) -> void:
	selected_character = character
	print("Character selected for placement.")

# Handle placing the character on the map when the mouse is clicked
func _on_mouse_button_pressed(event : InputEventMouseButton) -> void:
	if event.button_index == MOUSE_BUTTON_LEFT and selected_character != null:
		var screen_position = event.position
		if map_instance:
			# Convert screen position to map's local position
			var map_position = map_instance.to_local(screen_position)
			var tile_coords = (map_instance as TileMap).local_to_map(map_position)
			if IsValidPlacement(tile_coords):
				var character_instance = selected_character.instantiate()
				character_instance.position = (map_instance as TileMap).map_to_local(tile_coords)
				map_instance.add_child(character_instance)
				print("Character placed at " + str(tile_coords))
				selected_character = null
			else:
				print("Invalid placement location.")

# This is a placeholder for the IsValidPlacement method
func IsValidPlacement(tile_coords : Vector2i) -> bool:
	# Add your logic to check if the tile is valid for placement
	return true  # For now, we assume all positions are valid






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
