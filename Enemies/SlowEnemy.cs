using Godot;
using System;

public partial class SlowEnemy : EnemyBase
{
	public override void _Ready()
	{
		base._Ready(); // Call the parent class's _Ready method

		// Customize properties for SlowEnemy
		Speed = 50.0f; // Reduce speed
		MaxHealth = 200; // Increase health
		Damage = 15; // Modify attack damage
		

		// Additional setup for SlowEnemy
		GD.Print("SlowEnemy ready with custom properties.");
	}

	// Override behavior if needed
	public override void MoveTowards(Vector2 target, double delta)
	{
		// Optional: Add a delay or wobble for slow movement
		GD.Print("SlowEnemy moving towards target...");
		base.MoveTowards(target, delta); // Call base movement logic
	}

	// Additional unique behaviors can be added here
}
