// Modified from a gist by John Azariah
// https://gist.github.com/johnazariah/d95c03e2c56579c11272a647bab4bc38

using System;

// The following #pragma disables are justified because the abstract base class doesn't provide override but the sealed children do
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public abstract class Maybe<T>
    {
        public static explicit operator Maybe<T>(T value) =>
            Some(value);

        public static Maybe<T> Some(T value) =>
            new Choices.Some(value);

        public static Maybe<T> None { get; } = new Choices.None();

        public static Maybe<T> ofNullable(T value) =>
            value == null ? None : Some(value);

        public abstract R Match<R>(Func<T, R> someFunc, Func<R> noneFunc);

        public abstract void Iter(Action<T> someAction, Action noneAction);

        public void If(Action<T> someAction) =>
            Iter(someAction, () => {});

        public Maybe<R> Map<R>(Func<T, R> map) =>
            Match(
                v => Maybe<R>.Some(map(v)),
                () => Maybe<R>.None);

        public R Fold<R>(Func<R, T, R> foldFunc, R seed) =>
            Match(t => foldFunc(seed, t), () => seed);

        public R GetOrElse<R>(Func<T, R> foldFunc, R seed) =>
            Fold((_, t) => foldFunc(t), seed);

        public T GetOrDefault(T defaultValue) =>
            Fold((_, t) => t, defaultValue);

        public static Maybe<T> Return(T value) =>
            Some(value);

        public Maybe<R> Bind<R>(Func<T, Maybe<R>> map) =>
            Match(
                v => map(v).Match(
                    r => Maybe<R>.Some(r),
                    () => Maybe<R>.None),
                () => Maybe<R>.None);

        private Maybe() { }

        private static class Choices
        {
            public sealed class Some : Maybe<T>
            {
                private T Value { get; }

                public Some(T value) =>
                    Value = value;

                public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) =>
                    someFunc(Value);

                public override void Iter(Action<T> someAction, Action noneAction) =>
                    someAction(Value);

                public override string ToString() =>
                    $"Some ({Value})";
            }

            public sealed class None : Maybe<T>
            {
                public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) =>
                    noneFunc();

                public override void Iter(Action<T> someAction, Action noneAction) =>
                    noneAction();

                public override string ToString() =>
                    "None";
            }
        }
    }
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
