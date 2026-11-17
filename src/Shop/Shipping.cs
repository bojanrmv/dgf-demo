namespace Shop;

public enum Zone
{
    Domestic,
    Europe,
    World,
}

public static class Shipping
{
    public static decimal Cost(Zone zone, decimal weightKg, bool express, decimal orderTotal)
    {
        if (weightKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weightKg));
        }

        if (zone == Zone.Domestic)
        {
            if (express)
            {
                if (weightKg > 10) return 18m;
                return 10m;
            }
            else
            {
                if (orderTotal >= 50) return 0m;
                if (weightKg > 10) return 9m;
                return 5m;
            }
        }
        else if (zone == Zone.Europe)
        {
            if (express)
            {
                if (weightKg > 10) return 34m;
                return 18m;
            }
            else
            {
                if (weightKg > 10) return 17m;
                return 9m;
            }
        }
        else
        {
            if (express)
            {
                if (weightKg > 10) return 54m;
                return 30m;
            }
            else
            {
                if (weightKg > 10) return 27m;
                return 15m;
            }
        }
    }
}
