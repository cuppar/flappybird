using FlappyBird.autoloads;
using FlappyBird.constants;
using Godot;

namespace FlappyBird.scenes;

public partial class Game : Node2D
{
    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Bird _bird;

    #endregion

    public override void _Ready()
    {
        _bird.GameOver += OnGameOver;
        _bird.CollideWith += OnBirdCollisionWith;
    }

    private void OnBirdCollisionWith(KinematicCollision2D collision)
    {
        if (collision.GetCollider() is Pipe)
            _bird.Death();
    }

    private void OnGameOver()
    {
        AutoloadManager.SceneTranslation.Call(SceneTranslation.MethodName.ChangeSceneToFile, ScenePaths.TitlePage);
    }
}