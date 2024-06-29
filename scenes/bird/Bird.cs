using Godot;

namespace FlappyBird.scenes;

public partial class Bird : CharacterBody2D
{
    #region Delegates

    [Signal]
    public delegate void GameOverEventHandler();

    [Signal]
    public delegate void CollideWithEventHandler(KinematicCollision2D collision);

    #endregion

    private Vector2 _screenSize;
    [Export] public bool Alive = true;
    [Export] public float DeadSpeed = -100;
    [Export] public float FlyAngle = 30;
    [Export] public float FlySpeed = -50;
    [Export] public float Gravity = 1;


    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        // 死亡后等待死亡动画结束后出发GameOver
        if (!Alive && Position.Y > _screenSize.Y)
        {
            EmitSignal(SignalName.GameOver);
            return;
        }


        // 飞出屏幕下方，死亡
        if (Alive && Position.Y > _screenSize.Y)
        {
            Death();
            return;
        }


        Velocity = Velocity with { Y = Velocity.Y + Gravity };
        if (Alive)
            Rotation = Velocity.Y switch
            {
                > 0 => Mathf.DegToRad(FlyAngle),
                < 0 => -Mathf.DegToRad(FlyAngle),
                _ => Rotation
            };

        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null)
            EmitSignal(SignalName.CollideWith, collision);
    }

    public void Death()
    {
        Alive = false;
        _collisionShape.Disabled = true;
        _animatedSprite.Play("dead");
        _deadSound.Play();

        Rotation = 0;
        Velocity = Velocity with { Y = DeadSpeed };
        if (Position.Y > _screenSize.Y)
            Position = Position with { Y = _screenSize.Y - 1 };
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("fly") || !Alive) return;

        Velocity = Velocity with { Y = FlySpeed };
        _flySound.Play();
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _collisionShape;

    [Export] private AnimatedSprite2D _animatedSprite;
    [Export] private AudioStreamPlayer2D _flySound;
    [Export] private AudioStreamPlayer2D _deadSound;

    #endregion
}