
using UnityEngine;

[CreateAssetMenu(fileName = "GridSettings", menuName = "Data/GridSettings")]
public class GridSettings : ScriptableObject
{
    public static GridSettings instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GridSettings>(_path);

            return _instance;
        }
    }

    public int size { get { return _size; } }
    public int colorsCount { get { return _colors; } }
    public int roundBalls { get { return _roundBalls; } }
    public int lineSizeForRemove { get { return _lineSizeForRemove; } }

    [SerializeField] private int _size;
    [SerializeField] private int _colors;
    [SerializeField] private int _roundBalls;//сколько шаров добавляется в каждом раунде
    [SerializeField] private int _lineSizeForRemove;

    private static GridSettings _instance;
    private const string _path = "GridSettings";
}
