using Godot;

namespace FlappyBird.scenes;

public partial class PipePairWithScreenMonitor : Node2D
{
    #region Delegates

    [Signal]
    public delegate void FreedEventHandler();

    #endregion

    private bool _hasShowed;
    private Vector2 _screenSize;
    [Export] public float MidSpaceHeight;
    [Export] public float MidSpaceY;
    [Export] public PackedScene PipePairScene;
    [Export] public float Width;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;

        var pipePair = PipePairScene.Instantiate<PipePair>();
        pipePair.Width = Width;
        pipePair.MidSpaceY = MidSpaceY;
        pipePair.MidSpaceHeight = MidSpaceHeight;
        AddChild(pipePair);


        var screenNotifier = new VisibleOnScreenNotifier2D();
        screenNotifier.Rect = screenNotifier.Rect with
        {
            Position = Vector2.Zero,
            Size = new Vector2(Width, _screenSize.Y)
        };

        if (screenNotifier.IsOnScreen())
            _hasShowed = true;

        screenNotifier.ScreenEntered += OnScreenEntered;
        screenNotifier.ScreenExited += OnScreenExited;

        AddChild(screenNotifier);
    }

    private void OnScreenExited()
    {
        if (!_hasShowed) return;
        EmitSignal(SignalName.Freed);
        QueueFree();
    }

    private void OnScreenEntered()
    {
        _hasShowed = true;
    }
}