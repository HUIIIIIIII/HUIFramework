using UniFramework.Event;

namespace LocalCode
{
    public class EventItem : IEventMessage
    {
        public void SendMsg()
        {
            UniEvent.SendMessage(this);
        }
    }
}