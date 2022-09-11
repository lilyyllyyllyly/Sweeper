using Godot;
using System;

public class Spawner : Node2D
{
    [Export] public float spawnTime = 3f;
    [Export] private PackedScene enemy;
    private float timeSinceSpawn;

    [Signal] public delegate void Spawned(Node2D enemy);

    public override void _Process(float delta)
    {
        timeSinceSpawn += delta;
        if (timeSinceSpawn >= spawnTime) 
        {
            Spawn(enemy);
        }
    }

    private void Spawn(PackedScene enemy)
    {
        Node newEnemy = enemy.Instance();
        newEnemy.GetNode<Node2D>(".").Position = GlobalPosition;
        GetTree().CurrentScene.AddChild(newEnemy);
        timeSinceSpawn = 0;

        EmitSignal(nameof(Spawned), newEnemy);
    }
}
