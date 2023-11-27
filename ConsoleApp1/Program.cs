

using VRChat_Local_API;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EventListener listener = new EventListener();
            WebSocketListener webSocketListener = new WebSocketListener();

            webSocketListener.OnFriendAdded += WebSocketListener_OnFriendAdded;
            

            listener.Initialize(new VRChat_Local_API.Objects.EventListenerConfig() { DebugMode = true, ListenToActiveEvent = true, RequireGameRunning = true });
            webSocketListener.Initialize(Console.ReadLine());
            Thread.Sleep(-1);
        }

        private static void WebSocketListener_OnFriendAdded(object? sender, VRChat_Local_API.Objects.FriendAddObject e)
        {
            Console.WriteLine($"({e.id})OnFriendAdd: {e.displayName}");
        }
    }
}