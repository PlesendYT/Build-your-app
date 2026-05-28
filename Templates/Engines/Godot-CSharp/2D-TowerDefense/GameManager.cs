using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export] public int StartingGold = 100;
    [Export] public int StartingLives = 20;

    private Label _goldLabel;
    private Label _livesLabel;
    private Label _waveLabel;
    private Timer _spawnTimer;
    private Path2D _spawnPath;

    private int _gold;
    private int _lives;
    private int _currentWave = 0;
    private int _enemiesSpawned = 0;
    private int _enemiesInWave = 0;
    private bool _isWaveActive = false;
    private string _selectedTowerType = "basic";

    public override void _Ready()
    {
        _goldLabel = GetNode<Label>("UI/GoldLabel");
        _livesLabel = GetNode<Label>("UI/LivesLabel");
        _waveLabel = GetNode<Label>("UI/WaveLabel");
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnPath = GetNode<Path2D>("SpawnPath");
        _spawnTimer.Timeout += OnSpawnTimerTimeout;

        _gold = StartingGold;
        _lives = StartingLives;
        UpdateUI();
    }

    public void StartWave()
    {
        if (_isWaveActive) return;

        _enemiesInWave = 10 + (_currentWave * 2);
        _enemiesSpawned = 0;
        _isWaveActive = true;
        _spawnTimer.Start(0.5f);
    }

    private void OnSpawnTimerTimeout()
    {
        if (_enemiesSpawned >= _enemiesInWave)
        {
            _spawnTimer.Stop();
            return;
        }

        var enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
        var enemy = enemyScene.Instantiate<Node2D>();
        enemy.AddToGroup("enemies");
        AddChild(enemy);
        _enemiesSpawned++;
    }

    public void PlaceTower(Vector2 position)
    {
        var towerData = GetTowerData(_selectedTowerType);
        if (_gold < towerData.Cost) return;

        _gold -= towerData.Cost;
        var towerScene = GD.Load<PackedScene>("res://Tower.tscn");
        var tower = towerScene.Instantiate<Node2D>();
        tower.GlobalPosition = position;
        AddChild(tower);
        UpdateUI();
    }

    private TowerData GetTowerData(string type)
    {
        return type switch
        {
            "basic" => new TowerData { Cost = 50, Damage = 10, Range = 150, Rate = 1.0f },
            "sniper" => new TowerData { Cost = 100, Damage = 40, Range = 300, Rate = 2.0f },
            "rapid" => new TowerData { Cost = 75, Damage = 5, Range = 120, Rate = 0.2f },
            _ => new TowerData { Cost = 50, Damage = 10, Range = 150, Rate = 1.0f }
        };
    }

    public void EnemyReachedEnd(Node enemy)
    {
        _lives--;
        enemy.QueueFree();
        if (_lives <= 0) GameOver();
        UpdateUI();
    }

    public void EnemyKilled(int goldValue)
    {
        _gold += goldValue;
        UpdateUI();
    }

    private void GameOver()
    {
        GetTree().Paused = true;
        GetNode<Control>("UI/GameOverScreen").Show();
    }

    private void UpdateUI()
    {
        _goldLabel.Text = $"Gold: {_gold}";
        _livesLabel.Text = $"Lives: {_lives}";
        _waveLabel.Text = $"Wave: {_currentWave + 1}";
    }

    private struct TowerData
    {
        public int Cost;
        public int Damage;
        public float Range;
        public float Rate;
    }
}
