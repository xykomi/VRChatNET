<a href="Ω"><img src="http://readme-typing-svg.herokuapp.com?font=VT323&size=90&duration=2000&pause=1000&color=F70000&center=true&random=false&width=1100&height=140&lines=%E2%98%A6+VRChat+Local+API+%E2%98%A6;%E2%98%A6+By+Smoke+%E2%98%A6" alt="Ω" /></a>

A simple API that allows interaction with VRChat's local logs & websocket events

Current supported events:

+ OnPlayerJoined
+ OnPlayerLeft
+ OnPlayerBlocked
+ OnPlayerUnBlocked
+ OnPlayerAvatarModerationChanged: Safety, Hidden, Shown
+ OnRoomJoined
+ OnRoomLeft
+ OnNotificationRecieved
+ OnFriendLocationUpdate
+ OnFriendOffline
+ OnFriendOnline
+ OnFriendActive
+ OnFriendAdd
+ OnResponseNotification
+ OnUserLocation

## Usage/Examples

```csharp
     using VRChat_Local_API;
     using VRChat_Local_API.Objects;

     vRChat = new VRChat();

     string credentials = System.IO.File.ReadAllText($"F:\\VRChatAccount.txt"); -> username:password:auth_cookie
        
     vRChat.OnFriendOnline += OnFriendOnline;

     vRChat.Initialize(credentials.Split(':')[2]);

     private void OnFriendOnline(object? sender, VRChatEvents.OnFriendOnline args)
     {
         Console.WriteLine($"[{DateTime.Now}] - {args.User.DisplayName} has come online");
     }

```
