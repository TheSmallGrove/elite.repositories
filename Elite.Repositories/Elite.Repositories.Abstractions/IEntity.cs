namespace Elite.Repositories.Abstractions
{
    public interface IEntity
    { }

    public interface IEntity<T1, T2> : IEntity
    { }

    public interface IEntity<T1, T2, T3> : IEntity
    { }

    public interface IEntity<T1, T2, T3, T4> : IEntity
    { }

    public interface IEntity<T1, T2, T3, T4, T5> : IEntity
    { }
}