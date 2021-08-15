namespace AirVinyl.Functional.Infrastructure
{
    public struct RightType<T>
    {
        internal readonly T _value;

        internal RightType(T value)
        {
            _value = value;
        }

        public override string ToString() =>
            $"Right({_value})";
    }
}
