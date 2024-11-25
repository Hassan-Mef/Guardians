extends Control

# Exported variables for scenes
@export var knight_scene: PackedScene
@export var archer_scene: PackedScene

var selected_character: PackedScene = null  # Holds the selected character scene (e.g., Knight or Archer)
var map_layer: TileMapLayer = null  # Reference to the TileMap layer in the game scene

# target time to make button enabled or disabled
var target_time=0
#var change_archer_texture=preload("res://Assets/Images/archor_icon_clickable.png")
#var normal_archer_texture=preload("res://Assets/Images/archor_icon.png")
var archerPanel:Panel
var knightPanel:Panel
var KnightStyleBox:StyleBoxFlat
var archerStyleBox:StyleBoxFlat
# to change the picture to show enable and disable of button
var archerButton:TextureButton 
var knightButton:TextureButton

# Signal to notify when a character is selected
signal character_selected(character)

func _ready() -> void:
	# Access map layer, configure properties, etc.
	map_layer = get_node("/root/Game/Map/TileMap/Ground")
	archerButton=$Archor/archor
	knightButton=$Knight/knight
	archerPanel=$Archor
	knightPanel=$Knight
#	 for changing the background of knight button
	KnightStyleBox=StyleBoxFlat.new()
	KnightStyleBox.bg_color= Color(1,1,1,0)
	KnightStyleBox.set_border_width_all(3)
	KnightStyleBox.border_color=Color(0,0,0)
#	 for changing the background the archer button
	archerStyleBox=StyleBoxFlat.new()
	archerStyleBox.bg_color= Color(1,1,1,0)
	archerStyleBox.set_border_width_all(3)
	archerStyleBox.border_color=Color(0,0,0)
	archerPanel.add_theme_stylebox_override("panel",archerStyleBox)
	knightPanel.add_theme_stylebox_override("panel",KnightStyleBox)
	archerButton.disabled=true
	knightButton.disabled=true
	if map_layer:
		print("Guild scene ready and configured.")
	else:
		print("Map layer not found!")
		

func _process(delta):
	target_time+=delta
	#print(target_time)
	KnightStyleBox.bg_color= Color(1,1,1,0)
	archerStyleBox.bg_color= Color(1,1,1,0)
	if(target_time>=5):
		#print("5 delta time passed")
		archerStyleBox.bg_color= Color(0,1,1,0.5)
		#archerPanel.add_theme_stylebox_override("archerPanel",styleBox)
		archerButton.disabled=false
		#archerButton.texture_normal=change_archer_texture
	if(target_time>=8):
		#print("8 dekta passed")
		KnightStyleBox.bg_color= Color(0,1,1,0.5)
		knightButton.disabled=false


# Called when the archer button is pressed
func _on_archor_pressed() -> void:
	target_time-=5
	archerButton.disabled=true
	selected_character = archer_scene
	if selected_character:
		#print("Archer is selected.")
		emit_signal("character_selected", selected_character)
	else:
		print("Archer scene is not available.")

# Called when the knight button is pressed
func _on_knight_pressed() -> void:
	target_time-=8
	knightButton.disabled=true
	selected_character = knight_scene
	if selected_character:
		#print("Knight is selected.")
		# Emit the signal to notify that a character is selected
		emit_signal("character_selected", selected_character)
	else:
		print("Knight scene is not available.")



# This method will be called when the character is selected in Game.gd
func _on_character_selected(character: PackedScene) -> void:
	selected_character = character
	#print("Character selected for placement.")

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
				#print("Character placed at %s." % str(tile_coords))

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
