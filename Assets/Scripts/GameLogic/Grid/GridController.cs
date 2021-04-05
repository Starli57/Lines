using System;
using UnityEngine;

public class GridController : IDisposable
{
    public Action<int> onStacked;
    public Action onGridFilled;

    public GridController(GridView view, GridSettings settings)
    {
        _model = new GridModel(settings);

        _view = view;
        _view.InitializeGrid(_model.gridSettings);

        _view.onCellPressed += OnCellPressed;

        SpawnRoundBalls();
    }

    public void Dispose()
    {
        if (_view != null)
            _view.onCellPressed -= OnCellPressed;
    }

    private GridModel _model;
    private GridView _view;

    private int? _pickedPosition;

    private void SpawnRoundBalls()
    {
        int count = _model.gridSettings.roundBalls;

        for(int i = 0; i < count; i++)
        {
            if (_model.freeCells.Count == 0)
            {
                onGridFilled?.Invoke();
                return;
            }

            int indx = UnityEngine.Random.Range(0, _model.freeCells.Count);
            int freePosition = _model.freeCells[indx];
            var coordinates = GridHelper.GetСoordinates(freePosition, _model.gridSettings.size);

            int color = UnityEngine.Random.Range(0, _model.gridSettings.colorsCount) + 1;

            AddBall(coordinates.Item1, coordinates.Item2, color);
            RemoveStacked(freePosition);
        }
    }

    private void OnCellPressed(int position)
    {
        if (IsFreePosition(position) == false)
            PickCell(position);
        else if (_pickedPosition.HasValue == false)
            PickCell(position);
        else if (CanReplace(position))
        {
            ReplaceBall(position);
            FinishRound(position);
        }
    }

    private void FinishRound(int position)
    {
        int removed = RemoveStacked(position);
        if (removed == 0)
            SpawnRoundBalls();
    }

    private void PickCell(int position)
    {
        _pickedPosition = position;
    }

    private void ReplaceBall(int position)
    {
        var current = GridHelper.GetСoordinates(_pickedPosition.Value, _model.gridSettings.size);
        var next = GridHelper.GetСoordinates(position, _model.gridSettings.size);

        int color = _model.grid[current.Item1, current.Item2];
        RemoveBall(current.Item1, current.Item2);
        AddBall(next.Item1, next.Item2, color);

        _pickedPosition = null;
    }

    private void RemoveBall(int x, int y)
    {
        _model.grid[x,y] = 0;

        int position = GridHelper.GetPosition(x, y, _model.gridSettings.size);
        _model.freeCells.Add(position);
        _view.RemoveBall(position);
    }

    private void AddBall(int x, int y, int color)
    {
        _model.grid[x, y] = color;

        int position = GridHelper.GetPosition(x, y, _model.gridSettings.size);
        _model.freeCells.Remove(position);
        _view.AddBall(position, color);
    }

    private bool CanReplace(int position)
    {
        if (_pickedPosition.Value == position)
            return false;

        var current = GridHelper.GetСoordinates(_pickedPosition.Value, _model.gridSettings.size);
        var next = GridHelper.GetСoordinates(position, _model.gridSettings.size);
        if (IsFreePosition(current.Item1, current.Item2) || IsFreePosition(next.Item1, next.Item2) == false)
            return false;
        
        int iDiff = next.Item1 - current.Item1;
        int jDiff = next.Item2 - current.Item2;

        if (iDiff != 0 && jDiff != 0)
            return false;

        int rowSign = MathHelper.GetSign(iDiff);
        for(int i = current.Item1 + rowSign; i != next.Item1; i = i + rowSign)
        {
            if (IsFreePosition(i, current.Item2) == false)
                return false;
        }

        int colSign = MathHelper.GetSign(jDiff);
        for(int j = current.Item2 + colSign; j != next.Item2; j = j + colSign)
        {
            if (IsFreePosition(current.Item1, j) == false)
                return false;
        }

        return true;
    }
    
    private bool IsFreePosition(int position)
    {
        var coordinates = GridHelper.GetСoordinates(position, _model.gridSettings.size);
        return IsFreePosition(coordinates.Item1, coordinates.Item2);
    }

    private bool IsFreePosition(int x, int y)
    {
        return _model.grid[x, y] == 0;
    }

    #region RemoveStacked
    private int RemoveStacked(int position)
    {
        var coordinates = GridHelper.GetСoordinates(position, _model.gridSettings.size);
        int x = coordinates.Item1;
        int y = coordinates.Item2;

        int color = _model.grid[x, y];
        if (color == 0)
        {
            //Функция вызывается после того как шар был помещен в позицию position
            //если Color == 0, значит функция используется не правильно
            throw(new Exception("Something went wrong, color cant be 0 in this place"));
        }

        int removedCount = 0;

        if (TryRemoveHorizontalLine(x, y, color, ref removedCount)
            || TryRemoveVericalLine(x, y, color, ref removedCount)
            || TryRemoveMainDiagonal(x, y, color, ref removedCount)
            || TryRemoveSecondDiagonal(x, y, color, ref removedCount))
        {
            onStacked?.Invoke(removedCount);
            return removedCount;
        }

        return removedCount;
    }

    private bool TryRemoveHorizontalLine(int x, int y, int color, ref int removedCount)
    {
        int left = y;
        int right = y;

        for (int i = right + 1; i < _model.gridSettings.size; i++)
        {
            if (_model.grid[x, i] == color) right = i;
            else break;
        }

        for (int i = left - 1; i >= 0; i--)
        {
            if (_model.grid[x, i] == color) left = i;
            else break;
        }

        int stackedCount = right - left + 1;
        if (stackedCount < _model.gridSettings.lineSizeForRemove)
            return false;

        for (int i = left; i <= right; i++)
            RemoveBall(x, i);

        removedCount = stackedCount;

        return true;
    }

    private bool TryRemoveVericalLine(int x, int y, int color, ref int removedCount)
    {
        int top = x;
        int bottom = x;

        for (int i = top + 1; i < _model.gridSettings.size; i++)
        {
            if (_model.grid[i, y] == color) top = i;
            else break;
        }

        for (int i = bottom - 1; i >= 0; i--)
        {
            if (_model.grid[i, y] == color) bottom = i;
            else break;
        }

        int stackedCount = top - bottom + 1;
        if (stackedCount < _model.gridSettings.lineSizeForRemove)
            return false;

        for (int i = bottom; i <= top; i++)
            RemoveBall(i, y);

        removedCount = stackedCount;

        return true;
    }

    //todo: Добавить реализацию
    private bool TryRemoveMainDiagonal(int x, int y, int color, ref int removedCount)
    {
        return false;
    }


    //todo: Добавить реализацию
    private bool TryRemoveSecondDiagonal(int x, int y, int color, ref int removedCount)
    {
        return false;
    }
    #endregion
}
