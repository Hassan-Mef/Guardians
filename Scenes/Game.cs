using Godot;
using System;

public partial class Game : Control
{
	private PackedScene mapScene;  // Variable to hold the loaded scene

	public override void _Ready()
	{
		// Load the Map scene
		mapScene = (PackedScene)GD.Load("res://Map/Map.tscn");

		// Check if the scene was loaded successfully
		if (mapScene != null)
		{
			// Instance the map scene
			Node2D mapInstance = (Node2D)mapScene.Instantiate();

			// Add the map instance as a child of the Game node
			AddChild(mapInstance);

			GD.Print("Map scene loaded and added to Game.");
		}
		else
		{
			GD.PrintErr("Failed to load Map scene.");
		}
	}
}
