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

    [Export] public int TextureTileColCount = 4;
    [Export] public int TextureTileRowCount = 2;
    [Export] private Vector2 _textureTileSize = new(32, 80);
    [Export] private Texture2D _texture;

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
            _size = value;
            _collisionShapeSize = new Vector2(Math.Max(_size.X - 4, 1), _size.Y);
        }
    }


    public override void _Ready()
    {
        _ninePatchRect.Texture = _texture;
        Update();
    }

    private void Update()
    {
        foreach (var child in GetChildren())
        {
            if (child.Owner == null)
            {
                child.QueueFree();
            }
        }

        _ninePatchRect.RegionRect = _ninePatchRect.RegionRect with
        {
            Size = _textureTileSize,
            Position = new Vector2(_currentTileCol * _textureTileSize.X, _currentTileRow * _textureTileSize.Y)
        };

        if (Size.Y <= _ninePatchRect.PatchMarginTop + _ninePatchRect.PatchMarginBottom)
        {
            _ninePatchRect.PatchMarginTop = 0;
            _ninePatchRect.PatchMarginBottom = 0;
        }

        _ninePatchRect.Size = Size;
        _ninePatchRect.Position = -Size / 2;

        var collisionShape = new CollisionShape2D();
        AddChild(collisionShape);
        var shape = new RectangleShape2D();
        shape.Size = _collisionShapeSize;
        collisionShape.Shape = shape;
        collisionShape.Position = Vector2.Zero;
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private NinePatchRect _ninePatchRect;

    #endregion
}