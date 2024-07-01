using Godot;

namespace FlappyBird.scenes;

public partial class Background : ParallaxBackground
{
    [Export] public Vector2 Speed = Vector2.Left * 20;

    public override void _Process(double delta)
    {
        ScrollOffset += Speed * (float)delta;
    }
}