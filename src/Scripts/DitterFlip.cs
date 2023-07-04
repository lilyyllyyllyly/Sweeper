using Godot;
using System;

public class DitterFlip : Sprite
{
	private Sprite parent;

	public override void _Ready()
	{
		parent = GetNode<Sprite>("..");
	}

	public override void _Process(float delta)
	{
		FlipH = parent.FlipH;
	}
}
