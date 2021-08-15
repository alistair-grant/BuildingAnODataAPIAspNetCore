namespace AirVinyl.Functional.Infrastructure
{
    public struct LeftType<T>
    {
        internal readonly T _value;

        internal LeftType(T value)
        {
            _value = value;
        }

        public override string ToString() =>
            $"Left({_value})";
    }
}
