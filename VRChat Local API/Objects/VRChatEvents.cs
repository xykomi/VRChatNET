using Newtonsoft.Json;
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

        public class SocketMessageBase
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("content")]
            public string Content;
        }

        public class OnNotificationRecieved
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("type")]
            public string Type;

            [JsonProperty("senderUserId")]
            public string SenderUserId;

            [JsonProperty("senderUsername")]
            public string SenderUsername;

            [JsonProperty("receiverUserId")]
            public string ReceiverUserId;

            [JsonProperty("message")]
            public string Message;

            [JsonProperty("details")]
            public WorldDetails Details;

            [JsonProperty("created_at")]
            public DateTime CreatedAt;
        }

        public class OnFriendLocationUpdate
        {
            [JsonProperty("userId")]
            public string UserId;

            [JsonProperty("location")]
            public string Location;

            [JsonProperty("travelingToLocation")]
            public string TravelingToLocation;

            [JsonProperty("worldId")]
            public string WorldId;

            [JsonProperty("canRequestInvite")]
            public bool CanRequestInvite;

            [JsonProperty("user")]
            public User User;
        }

        public class OnFriendOffline
        {
            [JsonProperty("userId")]
            public string UserId;
        }

        public class OnFriendOnline
        {
            [JsonProperty("userId")]
            public string UserId;

            [JsonProperty("location")]
            public string Location;

            [JsonProperty("travelingToLocation")]
            public string TravelingToLocation;

            [JsonProperty("worldId")]
            public string WorldId;

            [JsonProperty("canRequestInvite")]
            public bool CanRequestInvite;

            [JsonProperty("user")]
            public User User;
        }

        public class OnFriendActive
        {
            [JsonProperty("userId")]
            public string UserId;

            [JsonProperty("user")]
            public User User;
        }

        public class OnFriendAdd
        {
            public string userId { get; set; }
            public User user { get; set; }
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
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("displayName")]
            public string DisplayName;

            [JsonProperty("userIcon")]
            public string UserIcon;

            [JsonProperty("bio")]
            public string Bio;

            [JsonProperty("bioLinks")]
            public List<string> BioLinks;

            [JsonProperty("profilePicOverride")]
            public string ProfilePicOverride;

            [JsonProperty("statusDescription")]
            public string StatusDescription;

            [JsonProperty("currentAvatarImageUrl")]
            public string CurrentAvatarImageUrl;

            [JsonProperty("currentAvatarThumbnailImageUrl")]
            public string CurrentAvatarThumbnailImageUrl;

            [JsonProperty("currentAvatarTags")]
            public List<object> CurrentAvatarTags;

            [JsonProperty("state")]
            public string State;

            [JsonProperty("tags")]
            public List<string> Tags;

            [JsonProperty("developerType")]
            public string DeveloperType;

            [JsonProperty("last_login")]
            public DateTime LastLogin;

            [JsonProperty("last_platform")]
            public string LastPlatform;

            [JsonProperty("allowAvatarCopying")]
            public bool AllowAvatarCopying;

            [JsonProperty("status")]
            public string Status;

            [JsonProperty("date_joined")]
            public string DateJoined;

            [JsonProperty("isFriend")]
            public bool IsFriend;

            [JsonProperty("friendKey")]
            public string FriendKey;

            [JsonProperty("last_activity")]
            public DateTime LastActivity;
        }

        public class WorldDetails
        {
            [JsonProperty("worldId")]
            public string WorldId;

            [JsonProperty("worldName")]
            public string WorldName;
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
