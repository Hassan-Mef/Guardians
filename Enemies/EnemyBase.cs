using Godot;
using System;
using System.Text.RegularExpressions;

public partial class EnemyBase : CharacterBody2D
{
	// Enemy properties
	[Export] public float Speed { get; set; } = 100.0f; // Movement speed
	[Export] public int MaxHealth { get; set; } = 100;  // Maximum health
	[Export] public int Damage { get; set; } = 10;      // Attack damage

	private int _currentHealth;
	private ProgressBar _healthBar;
	private NavigationAgent2D _navigationAgent;

	// Target position for the enemy (e.g., the base to attack)
	private Vector2 _targetPosition;

	public override void _Ready()
	{
		// Initialize health
		_currentHealth = MaxHealth;

		// Fetch health bar
		if (HasNode("HealthBar"))
		{
			_healthBar = GetNode<ProgressBar>("HealthBar");
			_healthBar.MaxValue = MaxHealth;
			UpdateHealthBar();
		}
		else
		{
			GD.PrintErr("HealthBar node not found in EnemyBase.");
		}

		// Fetch NavigationAgent2D
		if (HasNode("NavigationAgent2D"))
		{
			_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		}
		else
		{
			GD.PrintErr("NavigationAgent2D node not found in EnemyBase.");
		}

		// Set initial target (e.g., player's base)
		SetTargetToBase();
		AddToGroup("enemies");
		GD.Print("Enemies added to group");
		
	}

	private void SetTargetToBase()
	{
		// Access the TileMap from the Map node
		var tileMap = GetNode<Node2D>("/root/Game/Map/TileMap");
		if (tileMap == null)
		{
			GD.PrintErr("Error: TileMap node not found under Map.");
			return;
		}

		// Access the Base layer
		var baseLayer = tileMap.GetNode<TileMapLayer>("Base");
		if (baseLayer == null)
		{
			GD.PrintErr("Error: Base TileMapLayer not found in TileMap.");
			return;
		}

		// Set target to the center of the Base layer
		var usedRect = baseLayer.GetUsedRect();
		Vector2I centerTile = usedRect.Position + usedRect.Size / 2;
		_targetPosition = baseLayer.MapToLocal(centerTile);

		GD.Print($"Target position set to Base layer center: {_targetPosition}");


		// Assign target to the NavigationAgent2D
		if (_navigationAgent != null)
		{
			_targetPosition.X = _targetPosition.X - 100 ;
			_navigationAgent.TargetPosition = _targetPosition;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_navigationAgent != null)
		{
			Vector2 currentPosition = GlobalPosition;
			Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();

			// Check if the enemy is stuck or not moving (i.e., position is almost the same as the next position)
			if (currentPosition.DistanceTo(nextPathPosition) < 5) // Threshold for being stuck
			{
				//GD.Print("Enemy is stuck, recalculating path...");
				_navigationAgent.SetTargetPosition(_targetPosition); // Recalculate the path
			}

			if (_navigationAgent.AvoidanceEnabled)
			{
				Vector2 nextPosition = _navigationAgent.GetNextPathPosition();
			//	GD.Print($"Next Path: {nextPosition}, Current Position: {GlobalPosition}");
			}


			// Handle movement using NavigationAgent2D
			if (_navigationAgent.AvoidanceEnabled)
			{
				Velocity = _navigationAgent.GetNextPathPosition() - currentPosition;
				_navigationAgent.SetVelocity(Velocity.Normalized() * Speed);
			}
			else
			{
				Vector2 newVelocity = currentPosition.DirectionTo(nextPathPosition) * Speed;
				Velocity = newVelocity;
			}

			// Move the character
			MoveAndSlide();
		}
	}



	private void OnVelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		
	}

	// debugguing 
	private void CheckForFoliage()
	{
		// Convert global position to a tilemap coordinate (integer grid position)
		Vector2I currentTile = (Vector2I)GlobalPosition.Floor();

		// Access the TileMapLayer for foliage
		var foliageLayer = GetNode<TileMapLayer>("/root/Game/Map/TileMap/foliage");
		if (foliageLayer != null)
		{
			// Check if a tile exists using GetCellTileData
			var tileData = foliageLayer.GetCellTileData(currentTile);
			if (tileData != null) // TileData is null if no tile exists at the given position
			{
				GD.Print("Foliage detected at current position!");
			}
			else
			{
				GD.Print("No foliage at current position.");
			}
		}
		else
		{
			GD.PrintErr("Foliage layer not found.");
		}
	}





	// Take damage
	public void TakeDamage(int amount)
	{
		_currentHealth -= amount;
		GD.Print($"Enemy took {amount} damage, current health: {_currentHealth}"); // Debug print
		UpdateHealthBar();

		if (_currentHealth <= 0)
		{
			GD.Print("Enemy died.");
			Die();
		}
	}

	private void UpdateHealthBar()
	{
		if (_healthBar != null)
		{
			_healthBar.Value = _currentHealth;
			GD.Print($"Health bar updated: {_healthBar.Value}"); // Debug print
		}
	}

	private void Die()
	{
		QueueFree();
		// Add logic for enemy death, such as rewarding the player
	}

	public void _on_enemy_entered(Node body)
	{
		GD.Print("Body entered: " + body.Name);  // Print which body enters
		if (body.IsInGroup("enemies"))
		{
			GD.Print("Enemy detected, applying damage.");
			body.Call("TakeDamage", 10);  // Apply damage
		}
	}
}
