namespace Shop;

public static class Invoices
{
    public static decimal NetTotal(IReadOnlyList<Line> lines, decimal discount)
    {
        decimal sum = 0m;
        foreach (var line in lines)
        {
            sum += line.Quantity * line.Price;
        }

        return Math.Max(sum - discount, 0m);
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
