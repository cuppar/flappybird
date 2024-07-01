using FlappyBird.autoloads;
using FlappyBird.constants;
using Godot;

namespace FlappyBird.scenes;

public partial class Game : Node2D
{
    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Bird _bird;

    [Export] private Timer _scoreTimer;
    [Export] private UI _ui;

    #endregion

    private int _score;

    private int Score
    {
        get => _score;
        set
        {
            if (_score == value) return;
            _score = value;
            _ui.UpdateScore(Score);
        }
    }

    public override void _Ready()
    {
        _bird.GameOver += OnGameOver;
        _bird.CollideWith += OnBirdCollisionWith;
        _scoreTimer.Timeout += OnScoreTimerTimeout;

        Score = 0;
    }

    private void OnScoreTimerTimeout()
    {
        Score += 1;
    }

    private void OnBirdCollisionWith(Node2D body)
    {
        if (body is Pipe)
            _bird.Death();
    }

    private void OnGameOver()
    {
        AutoloadManager.SceneTranslation.Call(SceneTranslation.MethodName.ChangeSceneToFile, ScenePaths.TitlePage);
    }
}