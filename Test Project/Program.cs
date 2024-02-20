using VRChat_Local_API;
using VRChat_Local_API.Objects;

namespace Test_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EventListenerConfig eventListenerConfig = new EventListenerConfig();
            eventListenerConfig.RequireGameRunning = true; 

            EventListener listener = new EventListener();

            listener.OnRoomJoined += OnRoomJoined;
            listener.OnPlayerJoined += OnPlayerJoined;
            listener.OnPlayerBlocked += OnPlayerBlocked;
            listener.OnPlayerAvatarModerationChanged += OnPlayerAvatarModerationChanged;
            listener.OnPlayerUnBlocked += OnPlayerUnBlocked;
            listener.OnPlayerLeft += OnPlayerLeft;
            listener.OnRoomLeft += OnRoomLeft;

            listener.Initialize(eventListenerConfig);
            Thread.Sleep(0);
        }

        private static void OnPlayerUnBlocked(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnPlayerUnBlocked e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerUnBlocked -> {e.displayName} is now Unblocked");
        }

        private static void OnPlayerBlocked(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnPlayerBlocked e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerBlocked -> {e.displayName} is now Blocked");
        }

        private static void OnPlayerAvatarModerationChanged(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnPlayerAvatarModeration e)
        {
            switch (e.moderationType)
            {
                case VRChatEvents.OnPlayerAvatarModeration.ModerationType.Safety:
                    Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerAvatarModerationChanged:{e.moderationType} -> {e.displayName}'s Avatar is being protected by default safety settings");
                    break;
                case VRChatEvents.OnPlayerAvatarModeration.ModerationType.Shown:
                    Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerAvatarModerationChanged:{e.moderationType} -> {e.displayName}'s Avatar protection is fully disabled. be careful");
                    break;
                case VRChatEvents.OnPlayerAvatarModeration.ModerationType.Hidden:
                    Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerAvatarModerationChanged:{e.moderationType} -> {e.displayName}'s Avatar is entirely blocked and cannot harm you");
                    break;
            }
        }

        private static void OnRoomLeft(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnRoomLeft e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnRoomLeft -> Successfully left room");
        }

        private static void OnRoomJoined(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnRoomJoin e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnRoomJoined -> Successfully joined room | WorldId: {e.worldId} | InstanceId: {e.roomInstance}");
        }

        private static void OnPlayerLeft(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnPlayerLeft e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerLeft -> {e.displayName} has left your room");
        }

        private static void OnPlayerJoined(object? sender, VRChat_Local_API.Objects.VRChatEvents.OnPlayerJoined e)
        {
            Console.WriteLine($"[VRChat] - {DateTime.Now} | OnPlayerJoined -> {e.displayName} has joined your room");
        }
    }
}