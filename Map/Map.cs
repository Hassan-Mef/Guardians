using Godot;
using System;

public partial class Map : Node2D
{
	public Base BaseNode;

	public override void _Ready()
	{
		BaseNode = GetNode<Base>("Base"); // Adjust the path to match your scene structure
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
