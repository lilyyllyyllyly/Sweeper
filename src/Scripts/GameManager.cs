using Godot;
using System;

public class GameManager : Node2D
{
    [Export] private NodePath _playerPath;
    public Node2D player;

    [Export] private float _spawnerSpawnTime;
    private float _timeSinceNewSpawner;

    [Export] private PackedScene _spawnerScene;
    [Export] private Vector2[] _spawnerPositions;
    private bool[] _isPositionOccupied;

    public float score = 0;
    [Signal] public delegate void ScoreUpdated(float newScore);

    public override void _EnterTree()
    {
        Initialize();
    }

    private void Initialize()
    {
        _isPositionOccupied = new bool[_spawnerPositions.Length];
        player = GetNode<Node2D>(_playerPath);
    }

    public override void _Ready()
    {
        base._Ready();
        CallDeferred("TryCreateSpawner");
    }

    public override void _Process(float delta)
    {
        _timeSinceNewSpawner += delta;
        if (_timeSinceNewSpawner >= _spawnerSpawnTime)
        {
            TryCreateSpawner();
            _timeSinceNewSpawner = 0;
        }
    }

    private void TryCreateSpawner()
    {
        int? r = ChooseSpawnerPosition();
        if (r == null) {
            return;
        }
        CreateSpawner((int)r);
    }

    private int? ChooseSpawnerPosition()
    {
        // Choosing random number
        Godot.RandomNumberGenerator rng = new Godot.RandomNumberGenerator();
        rng.Randomize();
        int r = rng.RandiRange(0, _spawnerPositions.Length - 1);

        bool rIsFree = false;
        for (int i = 0; i < _spawnerPositions.Length - 1; i++)
        {
            // Checking if random position is already occupied by a spawner
            if (!_isPositionOccupied[r]) 
            {
                rIsFree = true;
                break;
            }
            r = (r+1) % _spawnerPositions.Length; // Going to the next possible position, and wrapping around if reached the last one
        }

        if (!rIsFree) return null; // This happens when all the positions are occupied
        
        return r;
    }

    private void CreateSpawner(int index)
    {
        // Instatiating new spawner
        Node2D newSpawner = _spawnerScene.Instance().GetNode<Node2D>(".");
        newSpawner.Position = _spawnerPositions[index];
        newSpawner.GetNode<Spawner>(".").Connect("Spawned", this, "OnEnemySpawn");
        newSpawner.GetNode<Spawner>(".").Connect("SpawnerDead", this, "OnSpawnerDead");
        newSpawner.GetNode<Spawner>(".").index = index;
        _isPositionOccupied[index] = true;

        // Adding spawner to the tree (VERY IMPORTANT, IT WILL NOT APPEAR OTHERWISE)
        GetTree().CurrentScene.AddChild(newSpawner);
    }

    private void OnEnemySpawn(Node2D enemy) 
    {
        enemy.GetNode<Enemy>(".").player = player;
        enemy.GetNode<Enemy>(".").Connect("EnemyDie", this, "OnEnemyDie");
    }

    private void OnEnemyDie(Node2D enemy) 
    {
        ChangeScore(score + 1);
    }

    private void OnSpawnerDead(Spawner spawner)
    {
        _isPositionOccupied[spawner.index] = false;
    }

    private void ChangeScore(float newValue)
    {
        score = newValue;
        EmitSignal("ScoreUpdated", score);
    }
}
