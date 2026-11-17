using Shop;

namespace Shop.Tests;

public class ShippingTests
{
    [Fact]
    public void Domestic_parcel_costs_the_base_rate()
    {
        Assert.Equal(5m, Shipping.Cost(Zone.Domestic, 2m, express: false, orderTotal: 30m));
    }

    [Fact]
    public void Express_to_Europe_costs_double()
    {
        Assert.Equal(18m, Shipping.Cost(Zone.Europe, 3m, express: true, orderTotal: 30m));
    }
}
