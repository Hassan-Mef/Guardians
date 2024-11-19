#extends Node2D
#
#@onready var tilemap = $Path  # Replace "TileMap" with the correct node path
#@onready var guild: Node = $"Guild"  # Automatically find the Guild node
##@export var guild_scene:PackedScene = preload("res://Scenes/archor.tscn")
#var archor:PackedScene = null
## Called when the node enters the scene tree for the first time.
#func _ready() -> void:
	#pass # Replace with function body.
#
## Called every frame. 'delta' is the elapsed time since the previous frame.
##func _process(delta: float) -> void:
	##pass
#
#func _input(event):
	#if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.is_pressed():
		#if guild.selected_character:
			## Get the mouse position in the map's local coordinates
			#var map_position = tilemap.to_local(event.position)
			#var tile_coords = tilemap.local_to_map(map_position)
			## Validate placement area
			#if is_valid_tile(tile_coords):
				 ##Instantiate and place the character
				#var character_instance = guild.selected_character.instance()
				#add_child(character_instance)
				#character_instance.position = tilemap.map_to_local(tile_coords)
				## Reset the selected character
				#guild.selected_character = null
			#else:
				#print("Invalid placement area.")
				#
#func is_valid_tile(tile_coords: Vector2) -> bool:
	## Get the tile ID at the clicked position
	#var tile_id = tilemap.get_cellv(tile_coords)
#
	## Check if the tile has the `is_path` property
	#if tile_id != null:
		#return tilemap.tile_set.tile_get_metadata(tile_id).get("is_path", false)
	#return false


#extends Node2D
#
#@onready var tilemap = self  # Since TileMap is the root Node2D
#@onready var path_layer = $Path  # Reference the Path layer
#@onready var guild: Node = $Guild  # Reference the Guild node (Scene instance)
#
## Handles mouse input for placing a character
#func _input(event):
	##print("in the map code")
	#if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.is_pressed():
		#print("key of mouse is pressed")
		#print("Selected Character in Guild:", guild.selected_character)
#
		#if guild.selected_character :
			## Convert the mouse position to tilemap coordinates
			#print("enterd in character")
			#var map_position = tilemap.to_local(event.position)
			#var tile_coords = path_layer.local_to_map(map_position)
#
			## Check if the tile is valid for placement
			#if is_valid_tile(tile_coords):
				## Instantiate and place the character
				#var character_instance = guild.selected_character.instance()
				#add_child(character_instance)
				#character_instance.position = path_layer.map_to_local(tile_coords)
#
				## Reset the selected character
				#guild.selected_character = null
			#else:
				#print("Invalid placement area.")
#
## Validates whether the tile is a valid placement area
#func is_valid_tile(tile_coords: Vector2) -> bool:
	## Get the tile ID from the Path layer
	#var tile_id = path_layer.get_cellv(tile_coords)
#
	## Check if the tile ID has the `is_path` property in its metadata
	#if tile_id != null and tile_id != -1:  # Ensure the tile ID is valid
		#return path_layer.tile_set.tile_get_metadata(tile_id).get("is_path", false)
	#return false

extends Node2D

@onready var tilemap = self  # Since TileMap is the root Node2D
@onready var path_layer = $Path  # Reference the Path layer
@onready var guild: Node = $Guild  # Reference the Guild node (Scene instance)

# Called when the scene is ready
func _ready():
	# Connect the signal from Guild to the local method '_on_character_selected'
	guild.connect("character_selected", Callable(self, "_on_character_selected"))

# Handle the selection of a character
func _on_character_selected(character: PackedScene):
	print("Character selected in TileMap:", character)
	# Optionally, you can now update anything else in TileMap related to the selected character

# Handles mouse input for placing a character
func _input(event):
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.is_pressed():
		print("Key of mouse is pressed")
		print("Selected Character in Guild:", guild.selected_character)

		if guild.selected_character:
			print("Entered in character")
			var map_position = tilemap.to_local(event.position)
			var tile_coords = path_layer.local_to_map(map_position)

			# Check if the tile is valid for placement
			if is_valid_tile(tile_coords):
				# Instantiate and place the character
				var character_instance = guild.selected_character.instance()
				add_child(character_instance)
				character_instance.position = path_layer.map_to_local(tile_coords)

				# Reset the selected character
				guild.selected_character = null
			else:
				print("Invalid placement area.")

# Validates whether the tile is a valid placement area
func is_valid_tile(tile_coords: Vector2) -> bool:
	# Get the tile ID from the Path layer
	var tile_id = path_layer.get_cellv(tile_coords)

	# Check if the tile ID has the `is_path` property in its metadata
	if tile_id != null and tile_id != -1:  # Ensure the tile ID is valid
		return path_layer.tile_set.tile_get_metadata(tile_id).get("is_path", false)
	return false
