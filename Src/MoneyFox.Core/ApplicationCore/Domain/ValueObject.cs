namespace MoneyFox.Core.ApplicationCore.Domain
{

    using System.Collections.Generic;
    using System.Linq;

    // TODO remove and use record or record struct on base
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(objA: left, objB: null) ^ ReferenceEquals(objA: right, objB: null))
            {
                return false;
            }

            return ReferenceEquals(objA: left, objB: null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left: left, right: right);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
        }
    }

}
