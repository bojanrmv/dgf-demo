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
}
