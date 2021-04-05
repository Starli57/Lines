using System;

public class ScoreModel
{
    public Action onScoreChanged;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            onScoreChanged?.Invoke();
        }
    }

    private int _score;
}