using System;

namespace LyricsCalculator.Common
{
    public abstract class ValueEqualityObject<T> : IEquatable<T> where T : class
    {
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public sealed override int GetHashCode()
        {
            return TupleBasedHashCode();
        }

        public bool Equals(T other)
        {
            if (ReferenceEquals(other, this))
                return true;

            if (other is null)
                return false;

            if (GetType() != other.GetType())
                return false;

            return IsEqual(other);
        }

        protected abstract bool IsEqual(T other);

        protected abstract int TupleBasedHashCode();
    }
}