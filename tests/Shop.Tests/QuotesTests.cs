using Shop;

namespace Shop.Tests;

public class QuotesTests
{
    [Fact]
    public void Invalid_quote_has_problems()
    {
        var lines = new List<Line> { new("Lamp", 0, -1m) };

        Assert.Equal(3, Quotes.Validate(" ", lines).Count);
    }

    [Fact]
    public void Quote_net_total_never_goes_below_zero()
    {
        var lines = new List<Line> { new("Lamp", 2, 40m) };

        Assert.Equal(70m, Quotes.NetTotal(lines, discount: 10m));
        Assert.Equal(0m, Quotes.NetTotal(lines, discount: 80m));
        Assert.Equal(0m, Quotes.NetTotal(lines, discount: 100m));
    }

    [Fact]
    public void A_free_line_is_valid_but_a_negative_price_is_not()
    {
        Assert.Empty(Quotes.Validate("Grace", [new("Sample", 1, 0m)]));
        Assert.Equal(
            ["Price cannot be negative: Sample."],
            Quotes.Validate("Grace", [new("Sample", 1, -0.01m)]));
    }
}
