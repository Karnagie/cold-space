using UniRx;

namespace CodeBase.Infrastructure.Stats
{
    public abstract class Stat<T> : ReactiveProperty<T>
    {
        public abstract void Decrease(T delta);
        public abstract void Increase(T delta);
    }
}