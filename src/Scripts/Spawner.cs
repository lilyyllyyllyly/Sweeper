using Godot;
using System;

public class Spawner : Node2D
{
    [Export] public float spawnTime = 3f;
    [Export] private PackedScene enemy;
    private float timeSinceSpawn;

    [Signal] public delegate void Spawned(Node2D enemy);
    [Signal] public delegate void SpanwerReady(Spawner spawner);

    public override void _Ready()
    {
        EmitSignal("SpanwerReady", this);
    }

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

        EmitSignal("Spawned", newEnemy);
    }

    private void ChangeSpawnTime(float newTime) 
    {
        spawnTime = newTime;
    }
}
