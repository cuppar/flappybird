using Godot;

namespace FlappyBird.autoloads;

public static class AutoloadManager
{
    public static CanvasLayer SceneTranslation { get; } =
        ((SceneTree)Engine.GetMainLoop()).Root.GetNode<CanvasLayer>("/root/SceneTranslation");
}