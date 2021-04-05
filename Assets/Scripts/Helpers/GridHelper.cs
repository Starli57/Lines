
public class GridHelper
{
    public static (int, int) GetСoordinates(int position, int size)
    {
        return (position /size, position % size);
    }

    public static int GetPosition(int i, int j, int size)
    {
        return i * size + j;
    }

}
