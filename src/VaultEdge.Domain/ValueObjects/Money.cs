using VaultEdge.Domain.Common.Models;

namespace VaultEdge.Domain.ValueObjects
{
    public class Money : ValueObject
    {        
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
        
        public Money(decimal amount, Currency currency) {
            Amount = amount;
            Currency = currency;
        }

        public static Money? Create(decimal amount, Currency currency) { 
            if(currency is null) 
                return null;
            
            var roundedAmount = Math.Round(amount, currency.DecimalPlaces);
            return new Money(roundedAmount, currency);
        }

        public static Money Zero(Currency currency)
        {
            if(currency is null)
                throw new ArgumentNullException(nameof(currency));

            return new Money(0, currency);
        }

        public bool IsZero() => Amount == 0;

        public bool IsPositive() => Amount > 0;

        public bool IsNegative() => Amount < 0;

        public Money Add(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot add money in different currencies: {Currency.Code} and {other.Currency.Code}");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot subtract money in different currencies: {Currency.Code} and {other.Currency.Code}");

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(decimal factor)
        {
            var newAmount = Math.Round(Amount * factor, Currency.DecimalPlaces);
            return new Money(newAmount, Currency);
        }

        public Money Divide(decimal divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            var newAmount = Math.Round(Amount / divisor, Currency.DecimalPlaces);

            return new Money(newAmount, Currency);
        }

        public Money Abs()
        {
            return new Money(Math.Abs(Amount), Currency);
        }

        public Money Negate()
        {
            return new Money(-Amount, Currency);
        }

        public int CompareTo(Money other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot compare money in different currencies: {Currency.Code} and {other.Currency.Code}");

            return Amount.CompareTo(other.Amount);
        }

        public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
        public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
        public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;
        public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

        public static Money operator +(Money left, Money right) => left.Add(right);
        public static Money operator -(Money left, Money right) => left.Subtract(right);
        public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
        public static Money operator /(Money money, decimal divisor) => money.Divide(divisor);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString()
        {
            var format = $"N{Currency.DecimalPlaces}";
            return $"{Currency.Symbol}{Amount.ToString(format)}";
        }

        public string ToStringWithCode()
        {
            var format = $"N{Currency.DecimalPlaces}";
            return $"{Amount.ToString(format)} {Currency.Code}";
        }
    }
}
