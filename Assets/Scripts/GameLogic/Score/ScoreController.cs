using System;

public class ScoreController : IDisposable
{
    public ScoreController(ScoreView view, GridController gridController)
    {
        _view = view;
        _gridController = gridController;

        _model = new ScoreModel();
        _model.onScoreChanged += UpdateScore;

        gridController.onStacked += AddScore;
    }

    public void Dispose()
    {
        if (_model != null)
            _model.onScoreChanged -= UpdateScore;

        if (_gridController != null)
            _gridController.onStacked -= AddScore;
    }

    private ScoreModel _model;
    private ScoreView _view;

    private GridController _gridController;

    private void AddScore(int count)
    {
        _model.score += count;
    }

    private void UpdateScore()
    {
        _view.UpdateScore(_model.score);
    }
}
