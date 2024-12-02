using Godot;
using System;

public partial class Base : Node2D
{
	[Export] public int MaxHealth = 100;
	public int CurrentHealth;
	private ProgressBar _healthBar;
	[Signal] public delegate void BaseDestroyedEventHandler();

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		if (HasNode("HealthBar"))
		{
			_healthBar = GetNode<ProgressBar>("HealthBar");
			_healthBar.MaxValue = MaxHealth;
			UpdateHealthBar();
		}

		// Connect the Area2D's signal if the node exists
		if (HasNode("Area2D"))
		{
			var area = GetNode<Area2D>("Area2D");
			area.BodyEntered += OnBodyEntered;
		}
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print($"Collision detected with {body.Name}");

		if (body is EnemyBase enemy) // Assuming your enemy class is named "Enemy"
		{
			TakeDamage(enemy.Damage); // Ensure the Enemy script has a Damage property
			enemy.QueueFree(); // Remove the enemy after dealing damage
		}
	}

	public void TakeDamage(int damage)
	{
		CurrentHealth -= damage;
		CurrentHealth = Mathf.Max(CurrentHealth, 0); // Ensures health doesn't go below 0

		GD.Print($"Base took {damage} damage! Current health: {CurrentHealth}");
		UpdateHealthBar();

		// If health is zero or less, signal destruction
		if (CurrentHealth <= 0)
		{
			GD.Print("Base destroyed! Game Over.");
			EmitSignal(nameof(BaseDestroyed));
		}
	}


	private void UpdateHealthBar()
	{
		if (_healthBar != null)
		{
			GD.Print($"Updating health bar: CurrentHealth = {CurrentHealth}, MaxHealth = {MaxHealth}");
			_healthBar.Value = CurrentHealth;
			GD.Print($"ProgressBar Value = {_healthBar.Value}");
		}
		else
		{
			GD.Print("HealthBar node not found or is null.");
		}
	}

}
