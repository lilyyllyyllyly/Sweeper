using Godot;
using System;

public class GameManager : Node2D
{
    [Export] private NodePath _playerPath;
    public Node2D player;

    public override void _EnterTree()
    {
        Initialize();
    }

    private void Initialize()
    {
        player = GetNode<Node2D>(_playerPath);
    }

    private void OnEnemySpawn(Node2D enemy) 
    {
        enemy.GetNode<Enemy>(".").target = player;
    }
}
