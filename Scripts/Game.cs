using Godot;
using System;

public partial class Game : Control
{
	private PackedScene _mapScene;      // Variable for loading Map scene
	private PackedScene _spawnerScene; // Variable for loading Spawner scene
	private PackedScene _guildScene;   // Variable for loading Guild scene

	private Base _base; // Reference to the base

	public override void _Ready()
	{
		// Load the Map scene
		_mapScene = GD.Load<PackedScene>("res://Map/Map.tscn"); // Path to Map scene
		if (_mapScene != null)
		{
			var mapInstance = _mapScene.Instantiate<Node2D>();
			AddChild(mapInstance); // Add Map as a child of Game
			GD.Print("Map scene loaded and added to Game.");

			// Access the base from the map
			var baseNode = mapInstance.GetNode<Base>("Base");
			if (baseNode != null)
			{
				_base = baseNode;
				GD.Print("Base node found and referenced.");

				// Connect the signal emitted when the base is destroyed
				_base.Connect("BaseDestroyed", new Callable(this, nameof(OnBaseDestroyed)));

			}
			else
			{
				GD.PrintErr("Base node not found in Map scene.");
			}

			// Pass the map to Spawner
			SetupSpawner(mapInstance);
		}
		else
		{
			GD.PrintErr("Failed to load Map scene.");
		}

		// Load the Guild scene
		_guildScene = GD.Load<PackedScene>("res://Scenes/guild.tscn");
		if (_guildScene != null)
		{
			var guildInstance = _guildScene.Instantiate<Control>();
			AddChild(guildInstance);
			GD.Print("Guild scene loaded and added to Game.");
		}
		else
		{
			GD.PrintErr("Failed to load Guild scene.");
		}
	}

	private void SetupSpawner(Node2D mapInstance)
	{
		// Load the Spawner scene
		_spawnerScene = GD.Load<PackedScene>("res://Scenes/Spawner.tscn");
		if (_spawnerScene != null)
		{
			var spawnerInstance = _spawnerScene.Instantiate<Node2D>();
			AddChild(spawnerInstance);
			GD.Print("Spawner scene loaded and added to Game.");

			// Pass the map reference to Spawner
			if (spawnerInstance is Spawner spawnerScript)
			{
				spawnerScript.Initialize(mapInstance);
				GD.Print("Spawner initialized with map reference.");
			}
		}
		else
		{
			GD.PrintErr("Failed to load Spawner scene.");
		}
	}

	private void OnBaseDestroyed()
	{
		GD.Print("Game Over! Transitioning to Game Over scene...");
		GetTree().ChangeSceneToFile("res://Scenes/EndingScene.tscn"); // Ensure the path is correct
	}
}
