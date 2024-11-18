extends Node2D

@onready var tilemap = $Path  # Replace "TileMap" with the correct node path
@onready var guild: Node = $"Guild"  # Automatically find the Guild node

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
	#pass

func _input(event):
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.is_pressed():
		if guild.selected_character:
			# Get the mouse position in the map's local coordinates
			var map_position = tilemap.to_local(event.position)
			var tile_coords = tilemap.local_to_map(map_position)
			# Validate placement area
			if is_valid_tile(tile_coords):
				# Instantiate and place the character
				var character_instance = guild.selected_character.instance()
				add_child(character_instance)
				character_instance.position = tilemap.map_to_local(tile_coords)
				# Reset the selected character
				guild.selected_character = null
			else:
				print("Invalid placement area.")
				
func is_valid_tile(tile_coords: Vector2) -> bool:
	# Get the tile ID at the clicked position
	var tile_id = tilemap.get_cellv(tile_coords)

	# Check if the tile has the `is_path` property
	if tile_id != null:
		return tilemap.tile_set.tile_get_metadata(tile_id).get("is_path", false)
	return false
