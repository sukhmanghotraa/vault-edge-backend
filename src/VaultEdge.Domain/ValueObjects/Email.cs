using VaultEdge.Domain.Common.Models;

namespace VaultEdge.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (!value.Contains('@') || !value.Contains('.'))
                return null;

            return new Email(value.Trim().ToLowerInvariant());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
