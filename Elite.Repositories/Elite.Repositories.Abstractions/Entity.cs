using System;

namespace Elite.Repositories.Abstractions
{
    public class Entity<T1> : IEntity<T1>
    {
        protected Tuple<T1> Key { get; set; } 
            = new Tuple<T1>(default(T1));
    }

    public abstract class Entity<T1, T2> : IEntity<T1, T2>
    {
        protected Tuple<T1, T2> Key { get; set; } 
            = new Tuple<T1, T2>(default(T1), default(T2));
    }

    public abstract class Entity<T1, T2, T3> : IEntity<T1, T2, T3>
    {
        protected Tuple<T1, T2, T3> Key { get; set; } 
            = new Tuple<T1, T2, T3>(default(T1), default(T2), default(T3));
    }

    public abstract class Entity<T1, T2, T3, T4> : IEntity<T1, T2, T3, T4>
    {
        protected Tuple<T1, T2, T3, T4> Key { get; set; } 
            = new Tuple<T1, T2, T3, T4>(default(T1), default(T2), default(T3), default(T4));
    }

    public abstract class Entity<T1, T2, T3, T4, T5> : IEntity<T1, T2, T3, T4, T5>
    {
        protected Tuple<T1, T2, T3, T4, T5> Key { get; set; }
            = new Tuple<T1, T2, T3, T4, T5>(default(T1), default(T2), default(T3), default(T4), default(T5));
    }
}