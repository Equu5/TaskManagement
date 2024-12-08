namespace Core.Domain;

public interface IEvent
{
    DateTime OccurredOn { get; }
}
