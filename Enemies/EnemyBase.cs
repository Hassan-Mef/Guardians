using Godot;
using System;

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
			_navigationAgent.TargetPosition = _targetPosition;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Handle movement using NavigationAgent2D
		if (_navigationAgent != null)
		{
			Vector2 currentPosition = GlobalPosition;
			Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
			GD.Print("Next path position: " + nextPathPosition);
			Vector2 newVelocity = currentPosition.DirectionTo(nextPathPosition) * Speed;
			GD.Print("tetsing:" +newVelocity);

			if (_navigationAgent.AvoidanceEnabled)
			{
				_navigationAgent.SetVelocity(newVelocity);
				GD.Print("Bro Testing: 1");
			}
			else
			{
				OnVelocityComputed(newVelocity);
				GD.Print("Bro Testing: 2");
			}

			MoveAndSlide();
			MoveAndCollide(newVelocity);
		}
	}

	private void OnVelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		
	}

	// Take damage
	public void TakeDamage(int amount)
	{
		_currentHealth -= amount;
		UpdateHealthBar();

		if (_currentHealth <= 0)
		{
			Die();
		}
	}

	private void UpdateHealthBar()
	{
		if (_healthBar != null)
		{
			_healthBar.Value = _currentHealth;
		}
	}

	private void Die()
	{
		QueueFree();
		// Add logic for enemy death, such as rewarding the player
	}
}
