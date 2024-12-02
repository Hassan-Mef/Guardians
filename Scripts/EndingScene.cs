using Godot;
using System;

public partial class EndingScene : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        // Connect the button signal
        var retryButton = GetNode<Button>("RetryButton");
        retryButton.Pressed += OnRetryPressed;
    }

    private void OnRetryPressed()
    {
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn"); // Change to the main menu
	}
}
