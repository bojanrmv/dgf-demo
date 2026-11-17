using Shop;

namespace Shop.Tests;

public class DiscountsTests
{
    [Fact]
    public void Member_with_large_order_gets_a_discount()
    {
        Assert.Equal(20m, Discounts.For(200m, member: true));
    }

    [Fact]
    public void Discount_is_capped()
    {
        Assert.Equal(25m, Discounts.For(1000m, member: true));
    }

    [Fact]
    public void Non_member_gets_nothing()
    {
        Assert.Equal(0m, Discounts.For(500m, member: false));
    }

    [Fact]
    public void Discount_starts_above_an_order_of_exactly_100()
    {
        Assert.Equal(0m, Discounts.For(100m, member: true));
        Assert.Equal(10.01m, Discounts.For(100.10m, member: true));
    }

    [Fact]
    public void Discount_reaches_its_cap_of_25_at_an_order_of_250()
    {
        Assert.Equal(24m, Discounts.For(240m, member: true));
        Assert.Equal(25m, Discounts.For(250m, member: true));
        Assert.Equal(25m, Discounts.For(260m, member: true));
    }
}
