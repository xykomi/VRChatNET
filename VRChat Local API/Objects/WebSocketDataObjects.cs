using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChat_Local_API.Objects
{
    public class WebSocketDataObject
    {
        public string type { get; set; }
        public string content { get; set; }
    }

    public class FriendAddObject
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
        public List<string> currentAvatarTags { get; set; }
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

    public class FriendDeleteObject
    {
        public string userId { get; set; }
    }
}
