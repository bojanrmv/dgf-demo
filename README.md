# dgf demo: one small shop, before and after

This repository holds one tiny C# codebase in two states, for a live demo of
`dgf`, a code-health CLI.

- **`main`, before**: the code as a hurried developer or coding assistant
  might leave it. It works and every test passes, but it is hard to change
  safely: one tangled function, copied code, and tests that run the code
  without really checking it.
- **`improved`, after**: the same behaviour, cleaned up in three commits, one
  per problem. Every test from `main` still passes unchanged.

The code covers member discounts, subscriptions, shipping costs, and totals and
validation for invoices and quotes. Every function is static and pure, and the
whole test suite runs in well under a second.

## Run it

You need the .NET 10 SDK and dgf 0.1.4.

```sh
git switch main        # or: git switch improved
dotnet test tests/Shop.Tests --collect "Code Coverage;Format=cobertura" --results-directory TestResults
dgf analyze --coverage TestResults
dgf mutate --run
dgf dup
```

On `improved`, also run:

```sh
dgf changes --base main
dgf history
```

Coverage is not committed. Delete `TestResults` when you switch branches, so
CRAP scores come from the code you are looking at.

`dgf.toml` flags functions above complexity 6 and fails above 10. It reads
coverage from `TestResults` and runs the `basic` mutators, which keeps a full
mutation run to a few seconds.

## What dgf measures

| | `main` | `improved` |
| --- | --- | --- |
| `dgf analyze` | exit 1: `Shipping.Cost` fails | exit 0: all ok |
| Highest complexity | 14 | 6 |
| Highest CRAP score | 38.5 | 6.0 |
| Tests | 11 | 25 |
| Mutation score | 53% (25 of 47 mutants caught) | 100% (29 of 29) |
| Bugs no test notices | 22 (9 with tests passing, 13 with no test running the code) | 0 |
| Duplication groups | 2 | 0 |

## Before and after

### 1. A tangled function

Before: a single method of nested `if`/`else` (abridged), with the same zone
logic repeated in each branch and the prices worked out by hand.

```csharp
if (zone == Zone.Domestic)
{
    if (express)
    {
        if (weightKg > 10) return 18m;
        return 10m;
    }
    else
    {
        if (orderTotal >= 50) return 0m;
        if (weightKg > 10) return 9m;
        return 5m;
    }
}
else if (zone == Zone.Europe)
{
    // ...the same again, twice, with other numbers
```

dgf: complexity **14**, above the fail limit of 10; 50% covered; CRAP **38.5**;
mutation score **32%** (6 of 19 mutants caught).

After: a guard clause, a rate lookup and two small helpers.

```csharp
public static decimal Cost(Zone zone, decimal weightKg, bool express, decimal orderTotal)
{
    if (weightKg <= 0)
    {
        throw new ArgumentOutOfRangeException(nameof(weightKg), weightKg, "Weight must be more than zero.");
    }

    return ShipsFree(zone, express, orderTotal) ? 0m : Price(zone, weightKg, express);
}

private static (decimal Base, decimal HeavySurcharge) RateFor(Zone zone) => zone switch
{
    Zone.Domestic => (5m, 4m),
    Zone.Europe => (9m, 8m),
    _ => (15m, 12m),
};
```

dgf: four functions of complexity **3**, all 100% covered, CRAP **3.0**; mutation
score **100%** (12 of 12). `dgf changes --base main` reports `Shipping.Cost`
as better, 14 → 3.

### 2. Copied code

Before: `Quotes.Validate` is `Invoices.Validate` with the names changed, and
the two totals compute the same thing two different ways.

```csharp
// Invoices.NetTotal
decimal sum = 0m;
foreach (var line in lines)
{
    sum += line.Quantity * line.Price;
}
return Math.Max(sum - discount, 0m);

// Quotes.NetTotal
var net = lines.Sum(l => l.Quantity * l.Price) - discount;
return Math.Max(net, 0m);
```

dgf dup: **2 groups**, 31 duplicated lines (31% of all statement lines). One is a
renamed copy of 21 lines (type 2), the other the same behaviour written
differently (type 4).

After: one shared implementation, which invoices and quotes both call.

```csharp
public static class Invoices
{
    public static decimal NetTotal(IReadOnlyList<Line> lines, decimal discount) =>
        OrderLines.NetTotal(lines, discount);

    public static List<string> Validate(string customerName, IReadOnlyList<Line> lines) =>
        OrderLines.Validate(customerName, lines);
}
```

dgf dup: **0 groups**, 0 duplicated lines.

### 3. A test that passes without checking

Before: the test runs the code, so coverage says 100%, but almost any answer
passes.

```csharp
var net = Invoices.NetTotal(lines, discount: 5m);

Assert.True(net > 0);
```

dgf mutate: turning `sum - discount` into `sum + discount` is a bug that
**no test notices**. The same happens at every boundary: an order of exactly
100, a subscription on its last day, and a free shipping total of exactly 50.

After: exact values, on both sides of each threshold.

```csharp
Assert.Equal(21m, Invoices.NetTotal(lines, discount: 5m));

Assert.True(Subscription.IsActive(lastDay, today: lastDay));
Assert.False(Subscription.IsActive(lastDay, today: lastDay.AddDays(1)));
```

dgf mutate: every mutant is caught, giving a score of **100%** (53% on `main`).
