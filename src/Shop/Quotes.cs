namespace Shop;

public static class Quotes
{
    public static decimal NetTotal(IReadOnlyList<Line> lines, decimal discount) =>
        OrderLines.NetTotal(lines, discount);

    public static List<string> Validate(string customerName, IReadOnlyList<Line> lines) =>
        OrderLines.Validate(customerName, lines);
}
