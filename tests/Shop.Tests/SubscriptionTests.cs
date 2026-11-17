using Shop;

namespace Shop.Tests;

public class SubscriptionTests
{
    [Fact]
    public void Subscription_is_active_before_it_ends()
    {
        Assert.True(Subscription.IsActive(new DateOnly(2026, 12, 31), new DateOnly(2026, 6, 1)));
    }

    [Fact]
    public void Subscription_is_inactive_after_it_ends()
    {
        Assert.False(Subscription.IsActive(new DateOnly(2026, 1, 31), new DateOnly(2026, 6, 1)));
    }
}
