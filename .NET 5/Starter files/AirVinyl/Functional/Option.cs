using AirVinyl.Functional.Infrastructure;
using System;
using System.Collections.Generic;

namespace AirVinyl.Functional
{
    public struct Option<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        internal Option(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _hasValue = true;
        }

        public static implicit operator Option<T>(NoneType _) =>
            default;

        public static implicit operator Option<T>(T value) =>
            value is null ? default : new(value);

        public IEnumerable<T> AsEnumerable()
        {
            if (_hasValue)
            {
                yield return _value;
            }
        }

        public TResult Match<TResult>(Func<TResult> whenNone, Func<T, TResult> whenSome) =>
            _hasValue ? whenSome(_value) : whenNone();

        public override string ToString() =>
            _hasValue ? $"Some({_value})" : "None";
    }
}
