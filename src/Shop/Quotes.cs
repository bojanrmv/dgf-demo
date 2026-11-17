namespace Shop;

public static class Quotes
{
    public static decimal NetTotal(IReadOnlyList<Line> lines, decimal discount)
    {
        var net = lines.Sum(l => l.Quantity * l.Price) - discount;
        return Math.Max(net, 0m);
    }

    public static List<string> Validate(string clientName, IReadOnlyList<Line> quoteLines)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(clientName))
        {
            errors.Add("Customer name is required.");
        }
        if (quoteLines.Count == 0)
        {
            errors.Add("At least one line is required.");
        }
        foreach (var item in quoteLines)
        {
            if (item.Quantity <= 0)
            {
                errors.Add($"Quantity must be positive: {item.Product}.");
            }
            if (item.Price < 0)
            {
                errors.Add($"Price cannot be negative: {item.Product}.");
            }
        }
        return errors;
    }
}
