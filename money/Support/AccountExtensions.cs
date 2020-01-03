using Microsoft.AspNetCore.Html;
using Money.Entities;
using Money.Models;

namespace Money.Support
{
    public static class AccountExtensions
    {
        public static string NumberLast4DigitsForDisplay(this IAccount account)
        {
            if (string.IsNullOrWhiteSpace(account.NumberLast4Digits))
            {
                return default;
            }

            switch (account.Type)
            {
                case AccountType.CreditCard:
                    return $"[●●●● ●●●● ●●●● {account.NumberLast4Digits}]";
                case AccountType.Current:
                case AccountType.Savings:
                    return $"[●●●●{account.NumberLast4Digits}]";
                default:
                    return default;
            }
        }

        public static HtmlString SpanForNumberLast4Digits(this AccountViewModel account)
        {
            var displayString = account.NumberLast4DigitsForDisplay();

            return !string.IsNullOrWhiteSpace(displayString)
                ? new HtmlString($"<span class=\"last-4\">{displayString}</span>")
                : new HtmlString(string.Empty);
        }
    }
}
