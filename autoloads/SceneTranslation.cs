using Godot;

namespace FlappyBird.autoloads;

public static class SceneTranslation
{
    #region Nested type: MethodName

    /// <summary>
    ///     Cached StringNames for the methods contained in this class, for fast lookup.
    /// </summary>
    public abstract class MethodName : CanvasLayer.MethodName
    {
        public static readonly StringName FadeToBlack = "fade_to_black";
        public static readonly StringName FadeFromBlack = "fade_from_black";
        public static readonly StringName ChangeSceneToFile = "change_scene_to_file";
        public static readonly StringName ChangeSceneToFileWithPause = "change_scene_to_file_with_pause";
    }

    #endregion

    #region Nested type: PropertyName

    public abstract class PropertyName : CanvasLayer.PropertyName
    {
    }

    #endregion

    #region Nested type: SignalName

    /// <summary>
    ///     Cached StringNames for the signals contained in this class, for fast lookup.
    /// </summary>
    public abstract class SignalName : CanvasLayer.SignalName
    {
        public static readonly StringName GameEntered = "game_entered";
        public static readonly StringName GameExited = "game_exited";
        public static readonly StringName SceneChanged = "scene_changed";
    }

    #endregion
}