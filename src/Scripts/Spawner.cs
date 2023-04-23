using Godot;
using System;

public class Spawner : Node2D
{
    [Export] public float spawnTime = 3f;
    [Export] private PackedScene enemy;
    private float timeSinceSpawn;

    [Signal] public delegate void Spawned(Node2D enemy);
    [Signal] public delegate void SpawnerDead(Spawner spawner);

    public int index;

    [Export] private int _maxHealth;
    public int hp;

    public override void _Ready()
    {
        base._Ready();
        hp = _maxHealth;
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

    private void SlashEntered(Area2D area) 
    {
        hp--;

        if (hp <= 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        QueueFree();
        EmitSignal("SpawnerDead", this);
    }
}
