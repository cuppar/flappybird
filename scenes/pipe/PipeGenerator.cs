using System;
using Godot;

namespace FlappyBird.scenes;

public partial class PipeGenerator : Node2D
{
    private float _lastSpaceY;
    private float _lastWidth;
    private float _lastX;
    private int _pipeCount;
    private Vector2 _screenSize;
    private float _spaceYMax;
    private float _spaceYMin;
    [Export] public float Gap = 40;

    [Export] public int MaxPipeCount = 7;

    [Export] public float MaxSpaceYDistanceFromLastPipe = 50;

    [Export] public PackedScene PipePairScene =
        GD.Load<PackedScene>("res://scenes/pipe/pipe_pair_with_screen_monitor.tscn");

    [Export] public float SpaceHeightMax = 70;
    [Export] public float SpaceHeightMin = 40;
    [Export] public float Speed = 30;
    [Export] public float StartX = 100;
    [Export] public float WidthMax = 35;
    [Export] public float WidthMin = 25;


    private int PipeCount
    {
        get => _pipeCount;
        set
        {
            _pipeCount = value;
            if (_pipeCount < MaxPipeCount) GeneratePipe();
        }
    }

    private void GeneratePipe()
    {
        var startX = _lastX + _lastWidth + Gap;
        _lastX = startX;
        var width = (float)GD.RandRange(WidthMin, WidthMax);
        _lastWidth = width;
        var spaceHeight = (float)GD.RandRange(SpaceHeightMin, SpaceHeightMax);
        var spaceY = (float)Math.Min(
            Math.Max(
                _lastSpaceY + GD.RandRange(-MaxSpaceYDistanceFromLastPipe, MaxSpaceYDistanceFromLastPipe),
                _spaceYMin),
            _spaceYMax);
        _lastSpaceY = spaceY;

        var pipe = PipePairScene.Instantiate<PipePairWithScreenMonitor>();
        pipe.Width = width;
        pipe.MidSpaceY = spaceY;
        pipe.MidSpaceHeight = spaceHeight;
        pipe.Position = pipe.Position with
        {
            X = startX
        };
        pipe.Freed += OnPipeFreed;

        AddChild(pipe);
        PipeCount += 1;
    }

    private void OnPipeFreed()
    {
        PipeCount -= 1;
    }

    public override void _Ready()
    {
        Init();
        GeneratePipe();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = Position with
        {
            X = Position.X - Speed * (float)delta
        };
    }

    private void Init()
    {
        _screenSize = GetViewportRect().Size;
        _spaceYMin = 0;
        _spaceYMax = _screenSize.Y - SpaceHeightMin;
        _lastX = StartX;
        _lastWidth = -Gap;
        _lastSpaceY = _screenSize.Y / 2 - (SpaceHeightMin + SpaceHeightMax) / 2;
    }
}