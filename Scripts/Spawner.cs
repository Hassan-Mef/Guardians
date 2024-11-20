using System;
using Godot;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
	[Export] private PackedScene[] EnemyScenes; // Use array instead of List
	private TileMapLayer _groundLayer;

	private Timer spawnTimer;
	private float spawnRate = 10f; // Initial spawn rate (in seconds)
	private int minEnemiesToSpawn = 1; // Initial number of enemies to spawn
	private int maxEnemiesToSpawn = 2;
	private float elapsedTime = 0f;

	private Node2D _mapInstance; // Store the map reference


	public override void _Ready()
	{
		// Load the map and set up the spawner
		//LoadMap();

		// Set up the timer for spawning enemies
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnRate;
		spawnTimer.Autostart = true;
		spawnTimer.Connect("timeout", new Callable(this, nameof(SpawnEnemies)));
		AddChild(spawnTimer);

		GD.Print("Spawner initialized.");
	}

	public void Initialize(Node2D mapInstance)
	{
		_mapInstance = mapInstance;
		var tileMap = _mapInstance.GetNode<Node2D>("TileMap");
		if (tileMap == null)
		{
			GD.PrintErr("Error: TileMap node not found in Map.");
			return;
		}

		_groundLayer = tileMap.GetNode<TileMapLayer>("Ground");
		if (_groundLayer == null)
		{
			GD.PrintErr("Error: Ground TileMapLayer not found in TileMap.");
			return;
		}

		GD.Print("Spawner initialized with map reference.");
	}

	// private void LoadMap()
	// {
	// 	var mapScene = ResourceLoader.Load<PackedScene>("res://Map/Map.tscn");
	// 	if (mapScene == null)
	// 	{
	// 		GD.PrintErr("Failed to load map.tscn. Check the file path.");
	// 		return;
	// 	}

	// 	var mapInstance = mapScene.Instantiate();
	// 	AddChild(mapInstance);

	// 	var tileMap = mapInstance.GetNode<Node2D>("TileMap");
	// 	if (tileMap == null)
	// 	{
	// 		GD.PrintErr("Error: TileMap node not found in Map.tscn.");
	// 		return;
	// 	}

	// 	_groundLayer = tileMap.GetNode<TileMapLayer>("Ground");
	// 	if (_groundLayer == null)
	// 	{
	// 		GD.PrintErr("Error: Ground TileMapLayer not found in TileMap.");
	// 		return;
	// 	}
	// }

	private void LoadMap()
	{
		GD.Print("LoadMap() is no longer needed as the map is passed from Game.cs.");
	}


	private List<Vector2> GetSpawnPositions()
	{
		var spawnPositions = new List<Vector2>();

		foreach (Vector2I cell in _groundLayer.GetUsedCells())
		{
			TileData tileData = _groundLayer.GetCellTileData(cell);
			if (tileData.GetCustomData("is_spawn").AsBool())
			{
				spawnPositions.Add(_groundLayer.MapToLocal(cell));
			}
		}

		return spawnPositions;
	}

	private void SpawnEnemies()
	{
		// Update elapsed time
		elapsedTime += (float)spawnTimer.WaitTime;
		// Increase spawn rate every 30 seconds after 1 minute
		if (elapsedTime >= 60f && spawnRate > 10f)
		{
			spawnRate -= 5f; // Decrease time between spawns
			spawnTimer.WaitTime = (float)spawnRate; // Explicit cast to float
			minEnemiesToSpawn++;
			maxEnemiesToSpawn++;
			GD.Print($"Spawn rate increased: {spawnRate}s, Enemies to spawn: {minEnemiesToSpawn}-{maxEnemiesToSpawn}");
		}

		// Spawn enemies
		var spawnPositions = GetSpawnPositions();
		if (spawnPositions.Count == 0)
		{
			GD.PrintErr("No valid spawn positions found.");
			return;
		}

		int enemiesToSpawn = (int)(GD.Randi() % (maxEnemiesToSpawn - minEnemiesToSpawn + 1)) + minEnemiesToSpawn;
		GD.Print($"Spawning {enemiesToSpawn} enemies...");

		for (int i = 0; i < enemiesToSpawn; i++)
		{
			Vector2 spawnPosition = spawnPositions[(int)(GD.Randi() % spawnPositions.Count)];
			SpawnRandomEnemy(spawnPosition);
		}
	}

	private void SpawnRandomEnemy(Vector2 position)
	{
		if (EnemyScenes.Length == 0)
		{
			GD.PrintErr("Error: EnemyScenes array is empty.");
			return;
		}

		// Randomly select an enemy type
		int randomIndex = (int)(GD.Randi() % EnemyScenes.Length);
		var selectedEnemyScene = EnemyScenes[randomIndex];

		if (selectedEnemyScene == null)
		{
			GD.PrintErr($"Error: Enemy scene at index {randomIndex} is null.");
			return;
		}

		var enemyInstance = selectedEnemyScene.Instantiate<EnemyBase>();
		AddChild(enemyInstance);
		enemyInstance.GlobalPosition = position;

		GD.Print($"Spawned {enemyInstance.Name} at {position}");
	}
}
