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
}
