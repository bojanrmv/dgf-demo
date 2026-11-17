namespace Shop;

public enum Zone
{
    Domestic,
    Europe,
    World,
}

public static class Shipping
{
    private const decimal HeavyParcelKg = 10m;
    private const decimal FreeDomesticFrom = 50m;

    public static decimal Cost(Zone zone, decimal weightKg, bool express, decimal orderTotal)
    {
        if (weightKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weightKg), weightKg, "Weight must be more than zero.");
        }

        return ShipsFree(zone, express, orderTotal) ? 0m : Price(zone, weightKg, express);
    }

    private static bool ShipsFree(Zone zone, bool express, decimal orderTotal) =>
        zone == Zone.Domestic && !express && orderTotal >= FreeDomesticFrom;

    private static decimal Price(Zone zone, decimal weightKg, bool express)
    {
        var rate = RateFor(zone);
        var cost = weightKg > HeavyParcelKg ? rate.Base + rate.HeavySurcharge : rate.Base;
        return express ? cost * 2 : cost;
    }

    private static (decimal Base, decimal HeavySurcharge) RateFor(Zone zone) => zone switch
    {
        Zone.Domestic => (5m, 4m),
        Zone.Europe => (9m, 8m),
        _ => (15m, 12m),
    };
}
