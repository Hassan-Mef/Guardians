using Godot;
using System;

public partial class SlowEnemy : EnemyBase
{
	private AnimationPlayer _animationPlayer = null;
	public override void _Ready()
	{
		base._Ready(); // Call the parent class's _Ready method

		// Customize properties for SlowEnemy
		// Speed = 35.0f; // Reduce speed
		// MaxHealth = 200; // Increase health
		// Damage = 15; // Modify attack damage

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (_animationPlayer != null)
		{
			_animationPlayer.Play("idel");
		}
		else
		{
			GD.PrintErr("AnimationPlayer not found for SlowEnemy!");
		}


		// Additional setup for SlowEnemy
		GD.Print("SlowEnemy ready with custom properties.");
	}

	// Override behavior if needed
	

	// Additional unique behaviors can be added here
}
