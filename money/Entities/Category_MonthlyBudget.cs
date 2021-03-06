using d = Dapper.Contrib.Extensions;

namespace Money.Entities
{
    [d.Table("Categories_MonthlyBudgets")]
    public class Category_MonthlyBudget
    {
        public Category_MonthlyBudget(
            int monthlyBudgetId,
            int categoryId,
            decimal amount)
        {
            MonthlyBudgetID = monthlyBudgetId;
            CategoryID = categoryId;
            Amount = amount;
        }

        [d.ExplicitKey]
        public int MonthlyBudgetID { get; private set; }

        [d.ExplicitKey]
        public int CategoryID { get; private set; }

        public decimal Amount { get; private set; }
    }
}
