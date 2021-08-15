using AirVinyl.Functional.Infrastructure;
using System;
using System.Collections.Generic;

namespace AirVinyl.Functional
{
    public struct Either<TLeft, TRight>
    {
        private readonly bool _hasRight;
        private readonly TLeft _left;
        private readonly TRight _right;

        private Either(bool hasRight, TLeft left, TRight right)
        {
            _hasRight = hasRight;
            _left = left;
            _right = right;
        }

        internal static Either<TLeft, TRight> Left(TLeft value) =>
            new(false, value, default);

        internal static Either<TLeft, TRight> Right(TRight value) =>
            new(true, default, value);

        public static implicit operator Either<TLeft, TRight>(LeftType<TLeft> left) =>
            Left(left._value);

        public static implicit operator Either<TLeft, TRight>(RightType<TRight> right) =>
            Right(right._value);

        public static implicit operator Either<TLeft, TRight>(TLeft value) =>
            Left(value);

        public static implicit operator Either<TLeft, TRight>(TRight value) =>
            Right(value);

        public IEnumerable<TRight> AsEnumerable()
        {
            if (_hasRight)
            {
                yield return _right;
            }
        }

        public TResult Match<TResult>(Func<TLeft, TResult> whenLeft, Func<TRight, TResult> whenRight) =>
            _hasRight ? whenRight(_right) : whenLeft(_left);

        public override string ToString() =>
            _hasRight ? $"Right({_right})" : $"Left({_left})";
    }
}
