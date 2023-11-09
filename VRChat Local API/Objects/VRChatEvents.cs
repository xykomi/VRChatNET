using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChat_Local_API.Objects
{
    public class VRChatEvents
    {
        public class OnPlayerJoined
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string displayName { get; set; } = string.Empty;
        }

        public class OnPlayerLeft
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string displayName { get; set; } = string.Empty;
        }

        public class OnPlayerBlocked
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string displayName { get; set; } = string.Empty;
        }

        public class OnPlayerUnBlocked
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string displayName { get; set; } = string.Empty;
        }

        public class OnPlayerAvatarModeration
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string displayName { get; set; } = string.Empty;
            public ModerationType moderationType { get; set; } = ModerationType.Safety;
            public enum ModerationType
            {
                Shown = 0,
                Hidden = 1,
                Safety = 2
            }
        }

        public class OnRoomLeft
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
        }

        public class OnRoomJoin
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string worldId { get; set; } = string.Empty;
            public int roomInstance { get; set; } = 0;
        }
    }
}
