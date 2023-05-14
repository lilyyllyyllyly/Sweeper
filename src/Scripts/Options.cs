using Godot;
using System;

public class Options : TextureRect
{
    [Export] private NodePath _fullscreenBtnPath;
    private Button _fullscreenBtn;
    [Export] private NodePath _borderlessBtnPath;
    private Button _borderlessBtn;

    public override void _Ready()
    {
	_fullscreenBtn = GetNode<Button>(_fullscreenBtnPath);
	_fullscreenBtn.Connect("pressed", this, "OnFullscreen");
	_borderlessBtn = GetNode<Button>(_borderlessBtnPath);
	_borderlessBtn.Connect("pressed", this, "OnBorderless");
    }

    private void OnFullscreen()
    {
	OS.WindowFullscreen = GetNode<CheckBox>(_fullscreenBtnPath).IsPressed();
    }

    private void OnBorderless()
    {
	OS.WindowBorderless = GetNode<CheckBox>(_borderlessBtnPath).IsPressed();
    }
}
