using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node
{
	[Export] private PackedScene EnemyScene; // Drag your SlowEnemy.tscn here
	private TileMapLayer _groundLayer;

	public override void _Ready()
	{
		// Load the Map scene from the Maps folder
		GD.Print("Loading map scene...");
		var mapScene = ResourceLoader.Load<PackedScene>("res://Map/Map.tscn");

		if (mapScene == null)
		{
			GD.PrintErr("Failed to load map.tscn. Check the file path.");
			return;
		}

		var mapInstance = mapScene.Instantiate();
		AddChild(mapInstance);
		GD.Print("Map scene loaded and added to the spawner.");


		// Print children of the mapInstance to verify structure
		GD.Print("Children of mapInstance:");
		foreach (Node child in mapInstance.GetChildren())
		{
			GD.Print(child.Name);
		}

		// Retrieve the TileMap node from the map instance
		GD.Print("Attempting to get TileMap...");
		var tileMap = mapInstance.GetNode<Node2D>("TileMap");
		if (tileMap == null)
		{
			GD.PrintErr("Error: TileMap node not found in Map.tscn.");
			return;
		}
		GD.Print("Successfully retrieved TileMap.");

		// Retrieve the Ground layer from the TileMap
		GD.Print("Attempting to get Ground layer...");
		_groundLayer = tileMap.GetNode<TileMapLayer>("Ground");
		if (_groundLayer == null)
		{
			GD.PrintErr("Error: Ground TileMapLayer not found in TileMap.");
			return;
		}
		GD.Print("Successfully retrieved Ground layer.");

		// Test spawning enemies
		TestSpawnEnemies();
	}



	private void TestSpawnEnemies()
	{
		// Get all valid spawn positions from the Ground layer
		List<Vector2> spawnPositions = GetSpawnPositions();

		if (spawnPositions.Count == 0)
		{
			GD.PrintErr("No valid spawn positions found in Ground layer.");
			return;
		}

		// Limit the number of enemies to spawn
		int maxEnemiesToSpawn = 3; // Set to 2 or 3 depending on your requirement
		int enemiesSpawned = 0;

		// Spawn a limited number of enemies
		foreach (Vector2 spawnPosition in spawnPositions)
		{
			if (enemiesSpawned >= maxEnemiesToSpawn)
			{
				break; // Stop spawning after reaching the max count
			}

			SpawnEnemy(spawnPosition);
			enemiesSpawned++;
		}
	}


	private List<Vector2> GetSpawnPositions()
	{
		var spawnPositions = new List<Vector2>();

		// Iterate over all tiles in the Ground layer
		foreach (Vector2I cell in _groundLayer.GetUsedCells())
		{
			// Get the TileData for the current cell
			TileData tileData = _groundLayer.GetCellTileData(cell);

			// Get the custom data for 'is_spawn'
			var isSpawn = tileData.GetCustomData("is_spawn");
			GD.Print($"Tile at {cell}: is_spawn = {isSpawn}");

			// Check if isSpawn is explicitly true
			if ( isSpawn.AsBool())
			{
				GD.Print($"Tile at {cell} "); // testing the god damn condiotn
				// Convert the grid position to world space position using MapToLocal
				Vector2 worldPosition = _groundLayer.MapToLocal(cell);
				spawnPositions.Add(worldPosition);
				GD.Print($"World position: {worldPosition}");
			}
		}

		return spawnPositions;  // Return after processing all cells
	}



	private void SpawnEnemy(Vector2 position)
	{
		if (EnemyScene == null)
		{
			GD.PrintErr("Error: EnemyScene is not assigned in the Spawner.");
			return;
		}

		// Instance the enemy and position it at the given spawn position
		var enemyInstance = EnemyScene.Instantiate<EnemyBase>();
		AddChild(enemyInstance);
		enemyInstance.GlobalPosition = position;

		GD.Print($"Spawned enemy at {position}");
	}
}
