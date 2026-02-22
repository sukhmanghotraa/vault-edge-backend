using VaultEdge.Domain.Common.Models;

namespace VaultEdge.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Value { get; private set; }

        private Address(string value)
        {
            Value = value;
        }

        public static Address? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (value.Length > 500)
                return null;

            return new Address(value.Trim());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Address address) => address.Value;
    }
}
