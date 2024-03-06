namespace CLive;

public static class ListExtentions
{
    public static T Shift<T>(this List<T> list)
    {
        T value = list[0];
        list.RemoveAt(0);
        return value;
    }
}