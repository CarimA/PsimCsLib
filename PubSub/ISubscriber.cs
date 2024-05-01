namespace PsimCsLib.PubSub;

public interface ISubscriber
{

}

public interface ISubscriber<TEvent> : ISubscriber
{
	Task HandleEvent(TEvent e);
}