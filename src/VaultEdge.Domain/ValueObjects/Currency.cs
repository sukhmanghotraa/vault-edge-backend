using VaultEdge.Domain.Common.Models;

namespace VaultEdge.Domain.ValueObjects
{
    public class Currency: ValueObject
    {
        public string Code { get; private set; }
        public int DecimalPlaces { get; private set; }
        public string Symbol { get; private set; }

        private Currency(string code, int decimalPlaces, string symbol)
        {
            Code = code;
            DecimalPlaces = decimalPlaces;
            Symbol = symbol;
        }

        public static Currency EUR => new Currency("EUR", 2, "€");
        public static Currency CHF => new Currency("CHF", 2, "CHF");
        public static Currency USD => new Currency("USD", 2, "$");
        public static Currency GBP => new Currency("GBP", 2, "£");
        public static Currency JPY => new Currency("JPY", 0, "¥");

        public static Currency? FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var upperCode = code.ToUpperInvariant();

            return upperCode switch
            {
                "EUR" => EUR,
                "CHF" => CHF,
                "USD" => USD,
                "GBP" => GBP,
                "JPY" => JPY,
                _ => null
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public override string ToString() => Code;
    }
}
