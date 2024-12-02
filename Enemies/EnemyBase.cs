using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class EnemyBase : CharacterBody2D
{
	// Enemy properties
	[Export] public float Speed { get; set; } = 100.0f; // Movement speed
	[Export] public int MaxHealth { get; set; } = 100;  // Maximum health
	[Export] public int Damage { get; set; } = 10;      // Attack damage
	[Export] public int AttackDamage { get; set; } = 20; // Damage dealt to units
	[Export] public float AttackInterval { get; set; } = 1.0f; // Time between attacks
	private Timer _attackTimer; // Timer to handle attack intervals
	private Node _unitInRange; // Tracks the unit currently in range
	private Node _base; // Reference to the base node
	private bool _isAttackingBase = false;
	private List<Node> _unitsInRange = new();
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


		// Add attack timer
		_attackTimer = new Timer();
		_attackTimer.WaitTime = AttackInterval;
		_attackTimer.OneShot = false;
		_attackTimer.Connect("timeout", new Callable(this, nameof(OnAttackTimeout)));
		AddChild(_attackTimer);

		// Ensure Area2D is set up correctly
		if (HasNode("Area2D"))
		{
			var attackArea = GetNode<Area2D>("Area2D");
			attackArea.Connect("body_entered", new Callable(this, nameof(OnUnitEntered)));
			attackArea.Connect("body_exited", new Callable(this, nameof(OnUnitExited)));

		}
		else
		{
			GD.PrintErr("Area2D node is missing in the enemy scene.");
		}

		_base = GetNodeOrNull<Node>("/root/Game/Map/Base"); // Adjust the path according to your scene structure

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
			_targetPosition.X = _targetPosition.X + 25 ;
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

			// Check if enemy has reached the base
			if (currentPosition.DistanceTo(_targetPosition) < 20) // Close enough to the base
			{
				AttackBase();
			}
		}
	}

	private void AttackBase()
	{
		if (_base != null && !_isAttackingBase)
		{
			_isAttackingBase = true;
			_base.Call("TakeDamage", AttackDamage);
			GD.Print($"Enemy dealt {AttackDamage} damage to the base.");
			GetTree().CreateTimer(1.0f).Connect("timeout", new Callable(this, nameof(ResetBaseAttack)));
		}
	}

	private void ResetBaseAttack()
	{
		_isAttackingBase = false;
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

	private void OnUnitEntered(Node body)
	{
		if (body.IsInGroup("units"))
		{
			_unitsInRange.Add(body); // Add to the list of units in range
			GD.Print($"{Name}: Unit {body.Name} entered attack range."); // Debug: unit entered

			// Start the attack timer if not already running
			if (_attackTimer.TimeLeft == 0) // Check if the timer is not running
			{
				GD.Print($"{Name}: Starting attack timer."); // Debug: starting attack timer
				_attackTimer.Start();
			}
		}
	}


	private void OnUnitExited(Node body)
	{
		if (_unitsInRange.Contains(body))
		{
			_unitsInRange.Remove(body); // Remove the unit from the list
			GD.Print($"{Name}: Unit {body.Name} exited attack range."); // Debug: unit exited

			// Stop the attack timer if no units are left in range
			if (_unitsInRange.Count == 0)
			{
				GD.Print($"{Name}: No units in range, stopping attack timer."); // Debug: stopping attack timer
				_attackTimer.Stop();
			}
		}
	}

	private void OnAttackTimeout()
	{
		if (_unitsInRange.Count > 0)
		{
			// Attack the first unit in the list
			Node target = _unitsInRange[0];
			if (target != null)
			{
				GD.Print($"{Name}: Attacking {target.Name} for {AttackDamage} damage."); // Debug: attacking unit
				target.Call("TakeDamage", AttackDamage); // Deal damage to the unit
			}
			else
			{
				GD.Print($"{Name}: First unit in range is null, skipping attack."); // Debug: if target is null
			}
		}
		else
		{
			GD.Print($"{Name}: No units in range during attack timeout."); // Debug: no units in range
		}
	}


}
