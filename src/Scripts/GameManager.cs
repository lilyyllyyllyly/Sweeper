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
    private int _spawnerNumber;
    private int _wave = 1;

    public float score = 0;

    [Signal] public delegate void ScoreUpdated(float newScore);
    [Signal] public delegate void NewWave(int wave);
    [Signal] public delegate void SpawnerNumberChanged(int spawners);

    public override void _EnterTree()
    {
        Initialize();
    }

    private void Initialize()
    {
        _isPositionOccupied = new bool[_spawnerPositions.Length];
        player = GetNode<Node2D>(_playerPath);
    }

    private void OnPlayerDeath()
    {
        SaveScore();
	GetTree().ChangeScene("res://Scenes/Title.tscn");
    }

    private void SaveScore()
    {
	string path = "user://stats.csv";
        File file = new File();
	int storedScore = 0, storedWave = 0;
	
	try {
            /* Read from file */
            file.Open(path, File.ModeFlags.Read);
            string[] lines = file.GetAsText().Split('\n');
            storedScore = int.Parse(lines[0].Split(',')[1]);
            storedWave  = int.Parse(lines[1].Split(',')[1]);
	} catch {} finally { /* i hate this but i want to write to the file regardless of exceptions... */
            /* Write to file */
            file.Open(path, File.ModeFlags.Write);
            file.StoreString(String.Format("score,{0}\n", score > storedScore ? score : storedScore));
            file.StoreString(String.Format("wave,{0}",    _wave > storedWave  ? _wave : storedWave ));
            file.Close();
	}
    }

    public override void _Ready()
    {
        base._Ready();
        CallDeferred("TryCreateSpawner"); // Using CallDeferred to avoid errors from adding the spawner to the tree before it's ready
    }

    public override void _Process(float delta)
    {
        _spawnerNumber = Mathf.Clamp(Mathf.CeilToInt(_wave/3f), 1, _spawnerPositions.Length); // Adding 1 spawner every 3 waves basically (i think its a bit wrong)
        _timeSinceNewSpawner += delta;
        if (_timeSinceNewSpawner >= _spawnerSpawnTime)
        {
            for (int i = 0; i < _spawnerNumber; i++) 
            {
                TryCreateSpawner();
            }
            _timeSinceNewSpawner = 0;
            _wave++;
            EmitSignal("NewWave", _wave);
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

        EmitSignal("SpawnerNumberChanged", CountSpawners());
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
        ChangeScore(score + 100);

        EmitSignal("SpawnerNumberChanged", CountSpawners());
    }

    private int CountSpawners()
    {
        int spawners = 0;
        for (int i = 0; i < _isPositionOccupied.Length; i++)
        {
            if (_isPositionOccupied[i])
            {
                spawners++;
            }
        }
        return spawners;
    }

    private void ChangeScore(float newValue)
    {
        score = newValue;
        EmitSignal("ScoreUpdated", score);
    }
}
