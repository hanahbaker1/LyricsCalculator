using LyricsCalculator.Common;

namespace LyricsCalculator.Processor.Models
{
    public class Artist : ValueEqualityObject<Artist>
    {
        private int? _hashCode;

        public string Name { get; set; }

        public string Id { get; set; }

        protected override bool IsEqual(Artist other)
        {
            return other != null
                   && other.Name.Equals(Name)
                   && other.Id.Equals(Id);
        }

        protected override int TupleBasedHashCode()
        {
            if (!_hashCode.HasValue)
            {
                _hashCode = (Name, Id).GetHashCode();
            }

            return _hashCode.Value;
        }
    }
}