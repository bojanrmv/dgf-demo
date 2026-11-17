namespace Shop;

/// <summary>Rules shared by invoices and quotes.</summary>
public static class OrderLines
{
    public static decimal NetTotal(IReadOnlyList<Line> lines, decimal discount)
    {
        var net = lines.Sum(line => line.Quantity * line.Price) - discount;
        return Math.Max(net, 0m);
    }

    public static List<string> Validate(string customerName, IReadOnlyList<Line> lines)
    {
        var problems = new List<string>();
        if (string.IsNullOrWhiteSpace(customerName))
        {
            problems.Add("Customer name is required.");
        }
        if (lines.Count == 0)
        {
            problems.Add("At least one line is required.");
        }
        foreach (var line in lines)
        {
            if (line.Quantity <= 0)
            {
                problems.Add($"Quantity must be positive: {line.Product}.");
            }
            if (line.Price < 0)
            {
                problems.Add($"Price cannot be negative: {line.Product}.");
            }
        }
        return problems;
    }
}
