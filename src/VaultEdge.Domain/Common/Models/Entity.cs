namespace VaultEdge.Domain.Common.Models
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            return obj is Entity entity && Id.Equals(entity.Id);
        }

        public bool Equals(Entity? other)
        {
            return Equals((object?)other);
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
