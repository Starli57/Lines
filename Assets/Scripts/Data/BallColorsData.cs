using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallColorsData", menuName = "Data/BallColorsData")]
public class BallColorsData : ScriptableObject
{
    public Color GetColor(int id)
    {
        return colors[id];
    }

    public static BallColorsData instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<BallColorsData>(_path);

            return _instance;
        }
    }

    [SerializeField] private List<Color> colors;

    private static BallColorsData _instance;
    private const string _path = "BallColorsData";
}
