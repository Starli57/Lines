
using System.Collections.Generic;

public class GridModel
{
    public int[,] grid { get; private set; }
    public List<int> freeCells { get; private set; }

    public readonly GridSettings gridSettings;
    
    public GridModel(GridSettings settings)
    {
        gridSettings = settings;

        grid = new int[gridSettings.size, gridSettings.size];

        freeCells = new List<int>();
        for (int i = 0; i < gridSettings.size * gridSettings.size; i++)
            freeCells.Add(i);
    }
}
