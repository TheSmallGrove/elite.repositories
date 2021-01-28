using System;

namespace Elite.Repositories.Abstractions
{
    public abstract class Entity<T1, T2> : IEntity<T1, T2>
    {
        protected IEntityKey<T1, T2> Key { get; set; } = new EntityKey<T1, T2>();
    }

    public abstract class Entity<T1, T2, T3> : IEntity<T1, T2, T3>
    {
        protected IEntityKey<T1, T2, T3> Key { get; set; } = new EntityKey<T1, T2, T3>();
    }

    public abstract class Entity<T1, T2, T3, T4> : IEntity<T1, T2, T3, T4>
    {
        protected IEntityKey<T1, T2, T3, T4> Key { get; set; } = new EntityKey<T1, T2, T3, T4>();
    }

    public abstract class Entity<T1, T2, T3, T4, T5> : IEntity<T1, T2, T3, T4, T5>
    {
        protected IEntityKey<T1, T2, T3, T4, T5> Key { get; set; } = new EntityKey<T1, T2, T3, T4, T5>();
    }
}