using FlappyBird.autoloads;
using FlappyBird.constants;
using Godot;

namespace FlappyBird.scenes;

public partial class TitlePage : Control
{
    private void OnStartButtonClicked()
    {
        AutoloadManager.SceneTranslation.Call(SceneTranslation.MethodName.ChangeSceneToFile, ScenePaths.Game);
    }

    private void OnQuitButtonClicked()
    {
        GetTree().Quit();
    }
}