using Godot;
using System;

public class Pause : TextureRect
{
	private SceneTree tree;

	public override void _Ready()
	{
		tree = GetTree();
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause")) {
			tree.Paused = !tree.Paused;
			Visible = !Visible;
		}

		if (Input.IsActionJustPressed("quit_button") && tree.Paused) {
			tree.Paused = false;
			tree.ChangeScene("res://Scenes/Title.tscn");
		}
	}
}
