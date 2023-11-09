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

        public class OnRoomLeft
        {
            public DateTime dateTime { get; set; } = DateTime.Now;
        }

        public class OnRoomJoin
        {
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string worldId { get; set; } = string.Empty;
            public int roomInstance { get; set; } = 0;
        }

    }
}
