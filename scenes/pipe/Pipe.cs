using System;
using Godot;

namespace FlappyBird.scenes;

public partial class Pipe : StaticBody2D
{
    private Vector2 _collisionShapeSize;
    private int _currentTileCol;
    private int _currentTileIndex;
    private int _currentTileRow;
    private Vector2 _size;
    [Export] private Texture2D _texture;
    [Export] private Vector2 _textureTileSize = new(32, 80);
    [Export] public int FixBottomHeight = 19;
    [Export] public int FixTopHeight = 19;

    [Export] public int TextureTileColCount = 4;
    [Export] public int TextureTileRowCount = 2;

    private Pipe()
    {
        Size = new Vector2(32, 80);
    }

    [Export]
    public int CurrentTileIndex
    {
        get => _currentTileIndex;
        set
        {
            if (_currentTileIndex == value || value < 0 ||
                value >= TextureTileRowCount * TextureTileColCount)
                return;
            _currentTileIndex = value;
            _currentTileRow = _currentTileIndex / TextureTileColCount;
            _currentTileCol = _currentTileIndex - _currentTileRow * TextureTileColCount;

            if (IsNodeReady())
                Update();
        }
    }

    [Export]
    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size == value)
                return;

            _size = value;
            _collisionShapeSize = _size with
            {
                X = Math.Max(_size.X * 28 / 32, 0)
            };
            if (IsNodeReady())
                Update();
        }
    }


    public override void _Ready()
    {
        Update();
    }

    private void Update()
    {
        foreach (var child in GetChildren())
            if (child.Owner == null)
                child.QueueFree();

        var ninePatchRect = new NinePatchRect();
        AddChild(ninePatchRect);
        ninePatchRect.Texture = _texture;
        ninePatchRect.RegionRect = ninePatchRect.RegionRect with
        {
            Size = _textureTileSize,
            Position = new Vector2(_currentTileCol * _textureTileSize.X, _currentTileRow * _textureTileSize.Y)
        };

        if (Size.Y < FixTopHeight + FixBottomHeight)
        {
            ninePatchRect.PatchMarginTop = 0;
            ninePatchRect.PatchMarginBottom = 0;
        }
        else
        {
            ninePatchRect.PatchMarginTop = FixTopHeight;
            ninePatchRect.PatchMarginBottom = FixBottomHeight;
        }

        ninePatchRect.Size = Size;
        ninePatchRect.Position = -Size / 2;


        var collisionShape = new CollisionShape2D();
        AddChild(collisionShape);
        var shape = new RectangleShape2D();
        shape.Size = _collisionShapeSize;
        collisionShape.Shape = shape;
        collisionShape.Position = Vector2.Zero;
    }
}