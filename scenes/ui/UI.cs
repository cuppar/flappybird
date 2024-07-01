using Godot;

namespace FlappyBird.scenes;

public partial class UI : Control
{
    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Label _scoreLabel;

    #endregion

    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }
}