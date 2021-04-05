using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreView : MonoBehaviour
{
    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    private Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<Text>();
    }
}
