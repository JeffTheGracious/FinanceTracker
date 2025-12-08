using AiFinanceTracker.Models;

namespace AiFinanceTracker.Services;

public class AIInsightService
{
    public string GenerateInsight(IEnumerable<Transaction> txns)
    {
        var list = txns.ToList();
        if (!list.Any())
            return "No financial data yet — add income or expenses to get insights.";

        // ===== Basic totals =====
        var income = list.Where(t => t.IsIncome).Sum(t => t.Amount);
        var expenses = list.Where(t => !t.IsIncome).Sum(t => t.Amount);
        var net = income - expenses;

        // ===== Recent window =====
        var last30 = list.Where(t => t.Date >= DateTime.Today.AddDays(-30)).ToList();
        var last30Income = last30.Where(t => t.IsIncome).Sum(t => t.Amount);
        var last30Expenses = last30.Where(t => !t.IsIncome).Sum(t => t.Amount);

        // ===== Trend calculation =====
        decimal trendValue = net switch
        {
            > 0 => 1m,
            0 => 0m,
            < 0 => -1m
        };

        // ===== Big expense detection =====
        var biggestExpense = list
            .Where(t => !t.IsIncome)
            .OrderByDescending(t => t.Amount)
            .FirstOrDefault();

        // ===== Spending spike detection =====
        var avgExpense = list.Where(t => !t.IsIncome).DefaultIfEmpty().Average(t => t?.Amount ?? 0);
        var latestExpense = list.Where(t => !t.IsIncome)
                                .OrderByDescending(t => t.Date)
                                .FirstOrDefault();
        bool spike = latestExpense != null && latestExpense.Amount > (avgExpense * 2);

        // ===== Risk scoring =====
        float riskScore = 0;

        if (net < 0) riskScore -= 1.5f;                       // Negative net
        if (expenses > income) riskScore -= 1.0f;             // Overspending
        if (last30Expenses > last30Income) riskScore -= 0.5f; // Recent overspending
        if (spike) riskScore -= 0.3f;                         // Spending spike
        if (income > expenses * 1.5m) riskScore += 1.0f;      // Healthy surplus

        string riskText = riskScore switch
        {
            >= 1 => "Your financial risk is low — you're maintaining a healthy surplus.",
            >= 0 => "Your financial situation is stable, but watching spending patterns is recommended.",
            >= -1 => "You're entering a moderate-risk zone — expenses are approaching or exceeding income.",
            < -1 => "High financial risk detected — expenses exceed income and trends suggest overspending."
        };

        // ===== Insight construction =====
        var insights = new List<string>();

        // Trend
        if (trendValue > 0)
            insights.Add("Your cashflow is positive, meaning you're earning more than you spend.");
        else if (trendValue < 0)
            insights.Add("Your cashflow is negative — expenses are exceeding income.");
        else
            insights.Add("Your cashflow is neutral this period.");

        // Recent 30-day analysis
        if (last30Expenses > last30Income)
            insights.Add($"In the last 30 days, expenses ({last30Expenses:C}) exceeded income ({last30Income:C}).");

        // Big expense
        if (biggestExpense != null)
            insights.Add($"Your largest recent expense was {biggestExpense.Title} at {biggestExpense.Amount:C}.");

        // Spike
        if (spike && latestExpense != null)
            insights.Add($"A recent expense ({latestExpense.Title}, {latestExpense.Amount:C}) is unusually high compared to your typical spending.");

        // Positive reinforcement
        if (net > 0 && expenses < income * 0.6m)
            insights.Add("Great job — your spending is well below income, giving you strong savings potential.");

        // Risk result
        insights.Add(riskText);

        // Combine into a paragraph
        return string.Join(" ", insights);
    }
}
