//using Godot;
//using System;
//
//public partial class Guild : Control
//{
	//// Exported variables for scenes
	//[Export]
	//public PackedScene knightScene;
	//[Export]
	//public PackedScene archerScene;
//
	//private PackedScene selectedCharacter = null; // Holds the selected character scene (e.g., Knight or Archer)
	//private TileMapLayer mapLayer = null;  // Reference to the TileMapLayer in the game scene
//
	//// Signal to notify when a character is selected
	//public delegate void CharacterSelected(PackedScene character);
	//public event CharacterSelected OnCharacterSelected;
//
	//// Guild.cs
	//public override void _Ready()
	//{
		//// Access map layer, configure properties, etc.
		//mapLayer = GetNode<TileMapLayer>("/root/Game/Map/TileMap/Ground");
		//GD.Print("Guild scene ready and configured.");
	//}
//
//
	//// Called when the knight button is pressed
	//public void _on_knight_pressed()
	//{
		//selectedCharacter = knightScene;
		//if (selectedCharacter != null)
		//{
			//GD.Print("Knight is selected.");
			//// Emit the signal to notify that a character is selected
			//OnCharacterSelected?.Invoke(selectedCharacter);
		//}
		//else
		//{
			//GD.Print("Knight scene is not available.");
		//}
	//}
//
	//// Called when the archer button is pressed
	//public void _on_archer_pressed()
	//{
		//selectedCharacter = archerScene;
		//if (selectedCharacter != null)
		//{
			//GD.Print("Archer is selected.");
			//OnCharacterSelected?.Invoke(selectedCharacter);
		//}
		//else
		//{
			//GD.Print("Archer scene is not available.");
		//}
	//}
//
	//// This method will be called when the character is selected in Game.cs
	//public void _on_character_selected(PackedScene character)
	//{
		//selectedCharacter = character;
		//GD.Print("Character selected for placement.");
	//}
//
	//// Handle placing the character on the map when the mouse is clicked
	//public void _on_mouse_button_pressed(InputEventMouseButton @event)
	//{
		//GD.Print("Hello Nigga") ;
		//if (@event.ButtonIndex == MouseButton.Left && selectedCharacter != null)
		//{
			//// Get the mouse position
			//var screenPosition = @event.Position;
//
			//// Ensure we have a valid mapLayer instance
			//if (mapLayer != null)
			//{
				//// Convert screen position to map's local position
				//Vector2 mapPosition = mapLayer.ToLocal(screenPosition);
//
				//// Convert map position to tile coordinates
				//Vector2I tileCoords = mapLayer.LocalToMap(mapPosition);
//
				//if (IsValidPlacement(tileCoords))
				//{
					//// Instance the selected character and place it on the map
					//var characterInstance = selectedCharacter.Instantiate<Node2D>();
					//characterInstance.Position = mapLayer.MapToLocal(tileCoords);
					//mapLayer.AddChild(characterInstance);
					//GD.Print($"Character placed at {tileCoords}.");
//
					//// Reset the selected character after placement
					//selectedCharacter = null;
				//}
				//else
				//{
					//GD.Print("Invalid placement location.");
				//}
			//}
		//}
	//}
//
	//// This method checks if the tile coordinates are valid for placement
	//private bool IsValidPlacement(Vector2I tileCoords)
	//{
		//// Add your logic to check if the tile is valid for placement (e.g., not an obstacle)
		//return true;  // For now, assume all tiles are valid
	//}
//
	//// Method to set the mapLayer instance from the Game class
	//public void SetMapLayer(TileMapLayer map)
	//{
		//mapLayer = map;
	//}
//
	//public override void _Input(InputEvent @event)
	//{
		//if (@event is InputEventMouseButton mouseEvent)
		//{
			//_on_mouse_button_pressed(mouseEvent);
		//}
	//}
//
//}
