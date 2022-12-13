using Godot;
using System;

public class GameManager : Node2D
{
    [Export] private NodePath _playerPath;
    public Node2D player;

    [Export] private float _spawnTime = 3f;
    private float _timeSinceHasten;
    [Export] private float _hastenSpawnDelay;
    [Export] private float _spawnTimeReduction;
    [Export] private float _minimumSpawnTime;
    [Signal] public delegate void HastenSpawn(float newTime);

    public float score = 0;
    [Signal] public delegate void ScoreUpdated(float newScore);

    public override void _EnterTree()
    {
        Initialize();
    }

    public override void _Process(float delta)
    {
        if (_timeSinceHasten >= _hastenSpawnDelay && _spawnTime - _spawnTimeReduction > _minimumSpawnTime) 
        {
            _spawnTime -= _spawnTimeReduction;
            EmitSignal("HastenSpawn", _spawnTime);
            _timeSinceHasten = 0;
        }
        _timeSinceHasten += delta;
    }

    private void Initialize()
    {
        EmitSignal("HastenSpawn", _spawnTime);
        player = GetNode<Node2D>(_playerPath);
    }

    private void OnEnemySpawn(Node2D enemy) 
    {
        enemy.GetNode<Enemy>(".").target = player;
        enemy.GetNode<Enemy>(".").player = player;
        enemy.GetNode<Enemy>(".").Connect("EnemyDie", this, "OnEnemyDie");
    }

    private void OnEnemyDie(Node2D enemy) 
    {
        ChangeScore(score + 1);
    }

    private void ChangeScore(float newValue)
    {
        score = newValue;
        EmitSignal("ScoreUpdated", score);
    }

    private void OnSpawnerReady(Spawner spawner) 
    {
        spawner.spawnTime = _spawnTime;
    }
}
