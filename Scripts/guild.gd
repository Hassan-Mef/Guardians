extends Control

# Exported variables for scenes
@export var knight_scene: PackedScene
@export var archer_scene: PackedScene

var selected_character: PackedScene = null  # Holds the selected character scene (e.g., Knight or Archer)
var map_layer: TileMapLayer = null  # Reference to the TileMap layer in the game scene

# Signal to notify when a character is selected
signal character_selected(character)

func _ready() -> void:
	# Access map layer, configure properties, etc.
	map_layer = get_node("/root/Game/Map/TileMap/Ground")
	if map_layer:
		print("Guild scene ready and configured.")
	else:
		print("Map layer not found!")

# Called when the knight button is pressed
func _on_knight_pressed() -> void:
	selected_character = knight_scene
	if selected_character:
		print("Knight is selected.")
		# Emit the signal to notify that a character is selected
		emit_signal("character_selected", selected_character)
	else:
		print("Knight scene is not available.")

# Called when the archer button is pressed
func _on_archer_pressed() -> void:
	selected_character = archer_scene
	if selected_character:
		print("Archer is selected.")
		emit_signal("character_selected", selected_character)
	else:
		print("Archer scene is not available.")

# This method will be called when the character is selected in Game.gd
func _on_character_selected(character: PackedScene) -> void:
	selected_character = character
	print("Character selected for placement.")

# Handle placing the character on the map when the mouse is clicked
func _on_mouse_button_pressed(event: InputEventMouseButton) -> void:
	if event.button_index == MOUSE_BUTTON_LEFT and selected_character:
		# Get the mouse position
		var screen_position = event.position

		# Ensure we have a valid map_layer instance
		if map_layer:
			# Convert screen position to map's local position
			var map_position = map_layer.to_local(screen_position)

			# Convert map position to tile coordinates
			var tile_coords = map_layer.local_to_map(map_position)

			if is_valid_placement(tile_coords):
				# Instance the selected character and place it on the map
				var character_instance = selected_character.instantiate()
				character_instance.position = map_layer.map_to_local(tile_coords)
				map_layer.add_child(character_instance)
				print("Character placed at %s." % str(tile_coords))

				# Reset the selected character after placement
				selected_character = null
			else:
				print("Invalid placement location.")

# This method checks if the tile coordinates are valid for placement
func is_valid_placement(tile_coords: Vector2i) -> bool:
	# Add your logic to check if the tile is valid for placement (e.g., not an obstacle)
	return true  # For now, assume all tiles are valid

# Method to set the map_layer instance from the Game class
func set_map_layer(map: TileMapLayer) -> void:
	map_layer = map

func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		_on_mouse_button_pressed(event)
