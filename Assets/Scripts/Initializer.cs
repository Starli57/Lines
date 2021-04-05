using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private Canvas _gameCanvas;

    [Header("Prefabs")]
    [SerializeField] private GridView _gridViewPrefab;
    [SerializeField] private ScoreView _scoreViewPrefab;

    private ObjectsPool _pool;

    private GridController _gridController;
    private ScoreController _scoreController;

    private void Awake()
    {
        _pool = new ObjectsPool();

        GridView gridView = Instantiate(_gridViewPrefab, _gameCanvas.transform);
        gridView.SetupDependencies(_pool);

        _gridController = new GridController(gridView, GridSettings.instance);
        
        ScoreView scoreView = Instantiate(_scoreViewPrefab, _gameCanvas.transform);
        _scoreController = new ScoreController(scoreView, _gridController);
    }

    private void OnDestroy()
    {
        if (_gridController != null)
            _gridController.Dispose();

        if (_scoreController != null)
            _scoreController.Dispose();
    }
}
