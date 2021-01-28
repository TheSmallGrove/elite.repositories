using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    internal class EntityKey<T1, T2> : IEntityKey<T1, T2>
    {
        public T1 Key1 { get; set; } = default(T1);
        public T2 Key2 { get; set; } = default(T2);

        public static implicit operator EntityKey<T1, T2>((T1, T2) tuple)
        {
            return new EntityKey<T1, T2>
            {
                Key1 = tuple.Item1,
                Key2 = tuple.Item2
            };
        }

        public static implicit operator (T1, T2)(EntityKey<T1, T2> key)
        {
            return (key.Key1, key.Key2);
        }
    }

    internal class EntityKey<T1, T2, T3> : IEntityKey<T1, T2, T3>
    {
        public T1 Key1 { get; set; } = default(T1);
        public T2 Key2 { get; set; } = default(T2);
        public T3 Key3 { get; set; } = default(T3);

        public static implicit operator EntityKey<T1, T2, T3>((T1, T2, T3) tuple)
        {
            return new EntityKey<T1, T2, T3>
            {
                Key1 = tuple.Item1,
                Key2 = tuple.Item2,
                Key3 = tuple.Item3
            };
        }

        public static implicit operator (T1, T2, T3)(EntityKey<T1, T2, T3> key)
        {
            return (key.Key1, key.Key2, key.Key3);
        }
    }

    internal class EntityKey<T1, T2, T3, T4> : IEntityKey<T1, T2, T3, T4>
    {
        public T1 Key1 { get; set; } = default(T1);
        public T2 Key2 { get; set; } = default(T2);
        public T3 Key3 { get; set; } = default(T3);
        public T4 Key4 { get; set; } = default(T4);

        public static implicit operator EntityKey<T1, T2, T3, T4>((T1, T2, T3, T4) tuple)
        {
            return new EntityKey<T1, T2, T3, T4>
            {
                Key1 = tuple.Item1,
                Key2 = tuple.Item2,
                Key3 = tuple.Item3,
                Key4 = tuple.Item4,
            };
        }

        public static implicit operator (T1, T2, T3, T4)(EntityKey<T1, T2, T3, T4> key)
        {
            return (key.Key1, key.Key2, key.Key3, key.Key4);
        }
    }

    internal class EntityKey<T1, T2, T3, T4, T5> : IEntityKey<T1, T2, T3, T4, T5>
    {
        public T1 Key1 { get; set; } = default(T1);
        public T2 Key2 { get; set; } = default(T2);
        public T3 Key3 { get; set; } = default(T3);
        public T4 Key4 { get; set; } = default(T4);
        public T5 Key5 { get; set; } = default(T5);

        public static implicit operator EntityKey<T1, T2, T3, T4, T5>((T1, T2, T3, T4, T5) tuple)
        {
            return new EntityKey<T1, T2, T3, T4, T5>
            {
                Key1 = tuple.Item1,
                Key2 = tuple.Item2,
                Key3 = tuple.Item3,
                Key4 = tuple.Item4,
                Key5 = tuple.Item5
            };
        }

        public static implicit operator (T1, T2, T3, T4, T5)(EntityKey<T1, T2, T3, T4, T5> key)
        {
            return (key.Key1, key.Key2, key.Key3, key.Key4, key.Key5);
        }
    }
}