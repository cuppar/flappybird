using System;
using Godot;

namespace FlappyBird.scenes;

public partial class Pipe : StaticBody2D
{
    [Export] private int _textureRow = 2;
    [Export] private int _textureCol = 4;
    [Export] private Vector2 _textureTileSize = new(32, 80);

    private Vector2 _collisionShapeSize;
    private Vector2 _size;

    [Export]
    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            _collisionShapeSize = new Vector2(value.X - 4, value.Y);
        }
    } 

    private Pipe()
    {
        Size = new Vector2(32, 80);
    }


    public override void _Ready()
    {
        var row = new Random().Next(_textureRow);
        var col = new Random().Next(_textureCol);

        _ninePatchRect.RegionRect = _ninePatchRect.RegionRect with
        {
            Size = _textureTileSize,
            Position = new Vector2(col * _textureTileSize.X, row * _textureTileSize.Y),
        };

        _ninePatchRect.Size = Size;
        _ninePatchRect.Position = -Size / 2;

        _collisionShape.Position = Vector2.Zero;
        var shape = (RectangleShape2D)_collisionShape.Shape;
        shape.Size = _collisionShapeSize;
    }

    public override void _Process(double delta)
    {
        // Position = Position with { X = Position.X - 10 * (float)delta };
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _collisionShape;

    [Export] private NinePatchRect _ninePatchRect;

    #endregion
}