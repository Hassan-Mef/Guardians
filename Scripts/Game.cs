
using Godot;
using System;

public partial class Game : Control
{
	private PackedScene _mapScene;      // Variable for loading Map scene
	private PackedScene _spawnerScene; // Variable for loading Spawner scene

	public override void _Ready()
	{
		// Load the Map scene
		_mapScene = GD.Load<PackedScene>("res://Map/Map.tscn"); // Path to Map scene
		if (_mapScene != null)
		{
			// Instance the map scene (Node2D is assumed to be the root of Map.tscn)
			var mapInstance = _mapScene.Instantiate<Node2D>();
			AddChild(mapInstance); // Add Map as a child of Game
			GD.Print("Map scene loaded and added to Game.");

			// Pass the map to Spawner (optional)
			SetupSpawner(mapInstance);
		}
		else
		{
			GD.PrintErr("Failed to load Map scene.");
		}
	}

	private void SetupSpawner(Node2D mapInstance)
	{
		// Load the Spawner scene
		_spawnerScene = GD.Load<PackedScene>("res://Scenes/Spawner.tscn"); // Path to Spawner scene
		if (_spawnerScene != null)
		{
			// Instance the spawner scene
			var spawnerInstance = _spawnerScene.Instantiate<Node2D>();
			AddChild(spawnerInstance); // Add Spawner as a child of Game
			GD.Print("Spawner scene loaded and added to Game.");

			// Pass the map reference to Spawner (if needed)
			if (spawnerInstance is Spawner spawnerScript)
			{
				spawnerScript.Initialize (mapInstance); // Initialize Spawner with map reference
				GD.Print("Spawner initialized with map reference.");
			}
		}
		else
		{
			GD.PrintErr("Failed to load Spawner scene.");
		}
	}
}

