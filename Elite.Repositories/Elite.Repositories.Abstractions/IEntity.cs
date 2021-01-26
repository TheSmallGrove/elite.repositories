namespace Elite.Repositories.Abstractions
{
    public interface IEntity
    { }

    public interface IEntity<T1> : IEntity
    { }

    public interface IEntity<T1, T2> : IEntity
    { }

    public interface IEntity<T1, T2, T3> : IEntity
    { }

}