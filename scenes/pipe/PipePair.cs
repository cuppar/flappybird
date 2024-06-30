using System;
using Godot;

namespace FlappyBird.scenes;

public partial class PipePair : Node2D
{
    private float _midSpaceHeight;
    private float _midSpaceY;
    private Vector2 _screenSize;
    private Pipe _tempPipe;
    private float _width;

    [Export] public PackedScene PipeScene;

    [Export]
    public float MidSpaceHeight
    {
        get => _midSpaceHeight;
        set
        {
            if (Math.Abs(_midSpaceHeight - value) <= float.Epsilon)
                return;

            _midSpaceHeight = value;
            if (IsNodeReady())
                Update();
        }
    }

    [Export]
    public float MidSpaceY
    {
        get => _midSpaceY;
        set
        {
            if (Math.Abs(_midSpaceY - value) <= float.Epsilon)
                return;

            _midSpaceY = value;
            if (IsNodeReady())
                Update();
        }
    }

    [Export]
    public float Width
    {
        get => _width;
        set
        {
            if (Math.Abs(_width - value) <= float.Epsilon)
                return;

            _width = value;
            if (IsNodeReady())
                Update();
        }
    }

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _tempPipe = PipeScene.Instantiate<Pipe>();
        Update();
    }

    public override void _ExitTree()
    {
        _tempPipe.QueueFree();
    }

    private void Update()
    {
        foreach (var child in GetChildren())
            if (child.Owner == null)
                RemoveChild(child);

        var tileIndex = new Random().Next(_tempPipe.TextureTileRowCount * _tempPipe.TextureTileColCount);

        if (MidSpaceY > 0)
        {
            // 生成上管道
            var upPipe = (Pipe)PipeScene.Instantiate();
            upPipe.CurrentTileIndex = tileIndex;
            upPipe.Size = new Vector2(Width, MidSpaceY);
            upPipe.Position = upPipe.Size / 2;
            AddChild(upPipe);
        }

        // ReSharper disable once InvertIf
        if (MidSpaceY + MidSpaceHeight < _screenSize.Y)
        {
            // 生成下管道
            var downPipe = (Pipe)PipeScene.Instantiate();
            downPipe.CurrentTileIndex = tileIndex;
            downPipe.Size = new Vector2(Width, _screenSize.Y - MidSpaceY - MidSpaceHeight);
            downPipe.Position = downPipe.Size / 2 + new Vector2(0, MidSpaceY + MidSpaceHeight);
            AddChild(downPipe);
        }
    }
}