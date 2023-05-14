using Godot;
using System;

public class Title : Node2D
{
    [Export] private NodePath _quitBtnPath;
    private Button _quitBtn;

    [Export] private NodePath _playBtnPath;
    private Button _playBtn;

    [Export] private NodePath _optionsBtnPath;
    private Button _optionsBtn;
    [Export] private NodePath _optbackBtnPath;
    private Button _optbackBtn;
    [Export] private NodePath _optionsMenuPath;

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

        _optionsBtn = GetNode<Button>(_optionsBtnPath);
        _optionsBtn.Connect("pressed", this, "OnOptions");
        _optbackBtn = GetNode<Button>(_optbackBtnPath);
        _optbackBtn.Connect("pressed", this, "OnOptionsLeave");
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }

    private void OnPlay()
    {
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

    private void OnOptions()
    {
	GetNode<CanvasItem>(_optionsMenuPath).Visible = true;
    }

    private void OnOptionsLeave()
    {
	GetNode<CanvasItem>(_optionsMenuPath).Visible = false;
    }
}
