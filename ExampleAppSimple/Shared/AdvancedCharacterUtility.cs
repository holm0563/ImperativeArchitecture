namespace ExampleAppSimple.Shared;

public static class AdvancedCharacterUtility
{
    public static char? EdgeShape(int x, int y, int maxX, int maxY)
    {
        if (y == 0 && x == 0)
            return '┌';
        if (y == maxY && x == maxX)
            return '┘';
        if (y == maxY && x == 0)
            return '└';
        if (y == 0 && x == maxX)
            return '┐';
        if (y == 0 || y == maxY)
            return '─';
        if (x == 0 || x == maxX)
            return '│';

        return null;
    }
}