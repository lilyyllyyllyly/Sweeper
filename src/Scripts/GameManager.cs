using Godot;
using System;

public class GameManager : Node2D
{
    [Export] private NodePath _playerPath;
    public Node2D player;

    public static GameManager instance;

    public override void _Ready()
    {
        // Singleton
        if (instance != null) 
        {
            QueueFree();
            return;
        }
        instance = this;
    }

    public override void _EnterTree()
    {
        Initialize();
    }

    private void Initialize()
    {
        player = GetNode<Node2D>(_playerPath);
    }
}
