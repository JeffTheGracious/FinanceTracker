using AiFinanceTracker.Models;
using AiFinanceTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TransactionService
{
    private const string StorageKey = "transactions";
    private readonly LocalStorageService _storage;

    public List<Transaction> Transactions { get; private set; } = new();

    public TransactionService(LocalStorageService storage)
    {
        _storage = storage;
    }

    // Load saved transactions
    public async Task InitializeAsync()
    {
        var saved = await _storage.GetItemAsync<List<Transaction>>(StorageKey);
        Transactions = saved ?? new List<Transaction>();
    }

    // Persist current list
    private async Task SaveAsync()
    {
        await _storage.SetItemAsync(StorageKey, Transactions);
    }

    // Add a new transaction
    public async Task AddAsync(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        Transactions.Add(transaction);
        await SaveAsync();
    }

    // Remove a transaction
    public async Task RemoveAsync(Guid id)
    {
        Transactions.RemoveAll(t => t.Id == id);
        await SaveAsync();
    }

    // Summary values
    public decimal TotalIncome =>
        Transactions.Where(t => t.IsIncome).Sum(t => t.Amount);

    public decimal TotalExpenses =>
        Transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);

    public decimal NetTotal => TotalIncome - TotalExpenses;

    // Generate chart data
    public ChartData GetChartData(string range)
    {
        var now = DateTime.Today;
        var labels = new List<string>();
        var values = new List<double>();

        switch (range.ToLower())
        {
            case "daily":
                for (int i = 6; i >= 0; i--)
                {
                    var day = now.AddDays(-i);
                    labels.Add(day.ToString("MMM d"));
                    values.Add((double)DayTotal(day));
                }
                break;

            case "weekly":
                for (int i = 7; i >= 0; i--)
                {
                    var start = now.AddDays(-(int)now.DayOfWeek).AddDays(-i * 7);
                    labels.Add($"W{start:MM-dd}");
                    values.Add((double)WeekTotal(start));
                }
                break;

            case "monthly":
                for (int i = 5; i >= 0; i--)
                {
                    var dt = now.AddMonths(-i);
                    labels.Add(dt.ToString("MMM yy"));
                    values.Add((double)MonthTotal(dt.Year, dt.Month));
                }
                break;

            case "yearly":
                for (int i = 4; i >= 0; i--)
                {
                    var year = now.AddYears(-i).Year;
                    labels.Add(year.ToString());
                    values.Add((double)YearTotal(year));
                }
                break;
        }

        return new ChartData
        {
            Labels = labels.ToArray(),
            Values = values.ToArray(),
            Label = "Net (Income - Expenses)"
        };
    }

    // Totals by date range
    private decimal DayTotal(DateTime day) =>
        Transactions.Where(t => t.Date.Date == day.Date)
                    .Sum(t => t.IsIncome ? t.Amount : -t.Amount);

    private decimal WeekTotal(DateTime weekStart) =>
        Transactions.Where(t => t.Date.Date >= weekStart.Date &&
                                t.Date.Date < weekStart.AddDays(7).Date)
                    .Sum(t => t.IsIncome ? t.Amount : -t.Amount);

    private decimal MonthTotal(int year, int month) =>
        Transactions.Where(t => t.Date.Year == year &&
                                t.Date.Month == month)
                    .Sum(t => t.IsIncome ? t.Amount : -t.Amount);

    private decimal YearTotal(int year) =>
        Transactions.Where(t => t.Date.Year == year)
                    .Sum(t => t.IsIncome ? t.Amount : -t.Amount);
}
