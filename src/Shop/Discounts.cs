namespace Shop;

public static class Discounts
{
    public static decimal For(decimal total, bool member)
    {
        if (member && total > 100m)
        {
            return Math.Min(total * 0.10m, 25m);
        }

        return 0m;
    }
}
