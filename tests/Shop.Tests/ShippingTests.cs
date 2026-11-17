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

    [Theory]
    [InlineData(Zone.Domestic, 5, 9)]
    [InlineData(Zone.Europe, 9, 17)]
    [InlineData(Zone.World, 15, 27)]
    public void Each_zone_has_a_base_rate_and_a_heavy_parcel_rate(Zone zone, decimal light, decimal heavy)
    {
        Assert.Equal(light, Shipping.Cost(zone, 2m, express: false, orderTotal: 20m));
        Assert.Equal(heavy, Shipping.Cost(zone, 12m, express: false, orderTotal: 20m));
    }

    [Fact]
    public void A_parcel_is_heavy_only_above_10_kg()
    {
        Assert.Equal(15m, Shipping.Cost(Zone.World, 10m, express: false, orderTotal: 20m));
        Assert.Equal(27m, Shipping.Cost(Zone.World, 10.01m, express: false, orderTotal: 20m));
    }

    [Fact]
    public void Express_doubles_the_heavy_parcel_rate()
    {
        Assert.Equal(54m, Shipping.Cost(Zone.World, 12m, express: true, orderTotal: 20m));
    }

    [Fact]
    public void Domestic_standard_shipping_is_free_from_50()
    {
        Assert.Equal(5m, Shipping.Cost(Zone.Domestic, 2m, express: false, orderTotal: 49.99m));
        Assert.Equal(0m, Shipping.Cost(Zone.Domestic, 2m, express: false, orderTotal: 50m));
        Assert.Equal(10m, Shipping.Cost(Zone.Domestic, 2m, express: true, orderTotal: 50m));
        Assert.Equal(9m, Shipping.Cost(Zone.Europe, 2m, express: false, orderTotal: 50m));
    }

    [Fact]
    public void Weight_must_be_more_than_zero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Shipping.Cost(Zone.Domestic, 0m, express: false, orderTotal: 20m));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Shipping.Cost(Zone.Domestic, -1m, express: false, orderTotal: 20m));
    }
}
