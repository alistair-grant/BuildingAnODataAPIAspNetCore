using System;

using static AirVinyl.Functional.F;

namespace AirVinyl.Functional
{
    public static class EitherExtensions
    {
        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either, Func<TRight, TResult> mapRight) =>
                either.Match<Either<TLeft, TResult>>(
                    l => Left(l),
                    r => Right(mapRight(r)));

        public static Either<TLeftResult, TRightResult> Map<TLeft, TLeftResult, TRight, TRightResult>(
            this Either<TLeft, TRight> either, Func<TLeft, TLeftResult> mapLeft, Func<TRight, TRightResult> mapRight) =>
                either.Match<Either<TLeftResult, TRightResult>>(
                    l => Left(mapLeft(l)),
                    r => Right(mapRight(r)));

        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TResult>> bindRight) =>
                either.Match(
                    l => Left(l),
                    r => bindRight(r));

    }
}
