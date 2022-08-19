using Godot;
using System;

public class Title : Node2D
{
    [Export] private NodePath _quitBtnPath;
    private Button _quitBtn;

    [Export] private NodePath _playBtnPath;
    private Button _playBtn;

    public override void _Ready()
    {
        Initialize();
    }

    private void Initialize()
    {
        _quitBtn = GetNode<Button>(_quitBtnPath);
        _quitBtn.Connect("pressed", this, "OnQuit");

        _playBtn = GetNode<Button>(_playBtnPath);
        _playBtn.Connect("pressed", this, "OnPlay");
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }

    private void OnPlay()
    {
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
