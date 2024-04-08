using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatLibrary.Objects
{
    public class VRCEvents
    {
        public class SocketMessageBase
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("content")]
            public string Content;
        }

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

        public class OnNotificationReceived
        {
            public string id { get; set; }
            public string type { get; set; }
            public string senderUserId { get; set; }
            public string senderUsername { get; set; }
            public string receiverUserId { get; set; }
            public string message { get; set; }
            public WorldDetails details { get; set; }
            public DateTime created_at { get; set; }
        }

        public class OnFriendLocationUpdate
        {
            public string userId { get; set; }
            public string location { get; set; }
            public string travelingToLocation { get; set; }
            public string worldId { get; set; }
            public bool canRequestInvite { get; set; }
            public User user { get; set; }
        }

        public class OnFriendOffline
        {
            public string userId { get; set; }
        }

        public class OnFriendOnline
        {
            public string userId { get; set; }
            public string location { get; set; }
            public string travelingToLocation { get; set; }
            public string worldId { get; set; }
            public bool canRequestInvite { get; set; }
            public User user { get; set; }
        }

        public class OnFriendActive
        {
            public string userId { get; set; }
            public User user { get; set; }
        }

        public class OnFriendAdd
        {
            public string userId { get; set; }
            public User user { get; set; }
        }

        public class OnFriendRemoved
        {
            public string data { get; set; } = string.Empty;
            public DateTime dateTime { get; set; } = DateTime.Now;
            public string userId { get; set; } = string.Empty;
        }

        public class OnResponseNotification
        {
            public string notificationId { get; set; }
            public string receiverId { get; set; }
            public string responseId { get; set; }
        }

        public class OnUserLocation
        {
            public string userId { get; set; }
            public string location { get; set; }
            public string instance { get; set; }
            public string worldId { get; set; }
            public User user { get; set; }
            public World world { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string displayName { get; set; }
            public string userIcon { get; set; }
            public string bio { get; set; }
            public List<string> bioLinks { get; set; }
            public string profilePicOverride { get; set; }
            public string statusDescription { get; set; }
            public string currentAvatarImageUrl { get; set; }
            public string currentAvatarThumbnailImageUrl { get; set; }
            public List<object> currentAvatarTags { get; set; }
            public string state { get; set; }
            public List<string> tags { get; set; }
            public string developerType { get; set; }
            public DateTime last_login { get; set; }
            public string last_platform { get; set; }
            public bool allowAvatarCopying { get; set; }
            public string status { get; set; }
            public string date_joined { get; set; }
            public bool isFriend { get; set; }
            public string friendKey { get; set; }
            public DateTime last_activity { get; set; }
        }

        public class WorldDetails
        {
            public string worldId { get; set; }
            public string worldName { get; set; }
        }

        public class PastDisplayName
        {
            public string displayName { get; set; }
            public DateTime updated_at { get; set; }
            public bool reverted { get; set; }
        }

        public class SteamDetails
        {
        }

        public class World
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string authorId { get; set; }
            public string authorName { get; set; }
            public string releaseStatus { get; set; }
            public bool featured { get; set; }
            public int capacity { get; set; }
            public int recommendedCapacity { get; set; }
            public string imageUrl { get; set; }
            public string thumbnailImageUrl { get; set; }
            public string @namespace { get; set; }
            public int version { get; set; }
            public string organization { get; set; }
            public object previewYoutubeId { get; set; }
            public List<object> udonProducts { get; set; }
            public int favorites { get; set; }
            public int visits { get; set; }
            public int popularity { get; set; }
            public int heat { get; set; }
            public DateTime publicationDate { get; set; }
            public string labsPublicationDate { get; set; }
            public List<object> instances { get; set; }
            public int publicOccupants { get; set; }
            public int privateOccupants { get; set; }
            public int occupants { get; set; }
            public List<string> tags { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }
    }
}
