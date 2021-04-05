using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridView : MonoBehaviour
{
    public Action<int> onCellPressed;

    public void SetupDependencies(ObjectsPool pool)
    {
        _pool = pool;
    }

    public void InitializeGrid(GridSettings settings)
    {
        _grid.constraintCount = settings.size;

        for (int i = 0; i < settings.size; i++)
            for(int j = 0; j < settings.size; j++)
            {
                Cell cell = Instantiate(_cellPrefab, transform);
                cell.onPressed += OnCellPressed;

                int position = GridHelper.GetPosition(i, j, settings.size);
                cell.SetPosition(position);

                _cells.Add(position, cell);
            }
    }

    public void AddBall(int position, int color)
    {
        Ball ball = GetBall(_cells[position].transform);
        ball.SetDefaultPosition();
        ball.SetColor(color);

        _cells[position].SetBall(ball);
    }

    public void RemoveBall(int position)
    {        
        AddToPool(_cells[position].ball);
        _cells[position].SetBall(null);
    }

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Ball _ballPrefab;

    private GridLayoutGroup _grid;
    private ObjectsPool _pool;

    private Dictionary<int, Cell> _cells = new Dictionary<int, Cell>();

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
    }

    private void OnDestroy()
    {
        foreach (var cell in _cells)
            cell.Value.onPressed -= OnCellPressed; 
    }

    private void OnCellPressed(int position)
    {
        onCellPressed?.Invoke(position);
    }

    private Ball GetBall(Transform parent)
    {
        if (_pool.Contains(typeof(Ball)))
        {
            Ball ball = _pool.GetObj(typeof(Ball)) as Ball;
            ball.transform.SetParent(parent);
            ball.gameObject.SetActive(true);

            return ball;
        }

        return Instantiate(_ballPrefab, parent);
    }

    private void AddToPool(Ball ball)
    {
        ball.gameObject.SetActive(false);
        _pool.Add(typeof(Ball), ball);
    }
}
