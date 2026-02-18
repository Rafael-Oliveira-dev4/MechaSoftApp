namespace MechaSoft.Domain.Interfaces;

public interface IEntity<T>
{
    public T Id { get; set; }
}