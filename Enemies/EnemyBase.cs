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

	// Timer for periodic actions (optional, if you have a Timer node)
	

	public override void _Ready()
	{
		_currentHealth = MaxHealth;

		// Get the ProgressBar node
		_healthBar = GetNode<ProgressBar>("HealthBar");
		if (_healthBar != null)
		{
			_healthBar.MaxValue = MaxHealth;
			UpdateHealthBar();
		}

		// Initialize the Timer if present
		// _actionTimer = GetNode<Timer>("Timer");
		// if (_actionTimer != null)
		// {
		// 	_actionTimer.Timeout += OnTimerTimeout;
		// }
	}

	// Movement logic
	virtual public void MoveTowards(Vector2 target, double delta)
	{
		// Calculate direction towards the target
		Vector2 direction = (target - GlobalPosition).Normalized();
		Velocity = direction * Speed;
		MoveAndSlide();
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

	// Update the health bar
	private void UpdateHealthBar()
	{
		if (_healthBar != null)
		{
			_healthBar.Value = _currentHealth;
		}
	}

	// Death logic
	public void Die()
	{
		QueueFree(); // Remove the enemy from the scene
					 // Add additional logic like rewarding the player
		
	}

	// Timer timeout logic (optional)
	private void OnTimerTimeout()
	{
		// Logic for periodic actions like special abilities
		GD.Print("Timer triggered for enemy action.");
	}
}
