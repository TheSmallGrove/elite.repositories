namespace Elite.Repositories.Abstractions
{
    public interface IEntityKey<T1, T2>
    {
        T1 Key1 { get; set; }
        T2 Key2 { get; set; }
    }

    public interface IEntityKey<T1, T2, T3>
    {
        T1 Key1 { get; set; }
        T2 Key2 { get; set; }
        T3 Key3 { get; set; }
    }

    public interface IEntityKey<T1, T2, T3, T4>
    {
        T1 Key1 { get; set; }
        T2 Key2 { get; set; }
        T3 Key3 { get; set; }
        T4 Key4 { get; set; }
    }

    public interface IEntityKey<T1, T2, T3, T4, T5>
    {
        T1 Key1 { get; set; }
        T2 Key2 { get; set; }
        T3 Key3 { get; set; }
        T4 Key4 { get; set; }
        T5 Key5 { get; set; }
    }
}