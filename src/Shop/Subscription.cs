namespace Shop;

public static class Subscription
{
    public static bool IsActive(DateOnly lastDay, DateOnly today)
    {
        return today <= lastDay;
    }
}
