using AirVinyl.Functional.Infrastructure;

namespace AirVinyl.Functional
{
    public static class F
    {
        public static LeftType<T> Left<T>(T value) =>
            new(value);

        public static NoneType None =>
            default;

        public static RightType<T> Right<T>(T value) =>
            new(value);

        public static Option<T> Some<T>(T value) =>
            new(value);
    }
}
