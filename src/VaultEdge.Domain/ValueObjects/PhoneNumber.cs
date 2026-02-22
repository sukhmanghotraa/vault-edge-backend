using VaultEdge.Domain.Common.Models;

namespace VaultEdge.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Value { get; private set; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var cleaned = value.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");

            if (cleaned.Length < 10 || cleaned.Length > 15)
                return null;

            if (!cleaned.All(char.IsDigit))
                return null;

            return new PhoneNumber(cleaned);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(PhoneNumber phone) => phone.Value;
    }
}
