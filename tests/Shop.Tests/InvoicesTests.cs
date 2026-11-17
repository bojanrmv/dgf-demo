using Shop;

namespace Shop.Tests;

public class InvoicesTests
{
    [Fact]
    public void Net_total_is_calculated()
    {
        var lines = new List<Line> { new("Pen", 2, 3m), new("Book", 1, 20m) };

        var net = Invoices.NetTotal(lines, discount: 5m);

        Assert.True(net > 0);
    }

    [Fact]
    public void Valid_invoice_has_no_problems()
    {
        var lines = new List<Line> { new("Pen", 2, 3m) };

        Assert.Empty(Invoices.Validate("Ada", lines));
    }

    [Fact]
    public void Invalid_invoice_has_problems()
    {
        var lines = new List<Line> { new("Pen", 0, -1m) };

        Assert.Equal(3, Invoices.Validate("", lines).Count);
    }

    [Fact]
    public void Net_total_is_quantity_times_price_minus_the_discount()
    {
        var lines = new List<Line> { new("Pen", 2, 3m), new("Book", 1, 20m) };

        Assert.Equal(21m, Invoices.NetTotal(lines, discount: 5m));
    }

    [Fact]
    public void An_invoice_needs_at_least_one_line()
    {
        Assert.Equal(["At least one line is required."], Invoices.Validate("Ada", []));
    }
}
