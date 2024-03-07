<a href="Ω"><img src="http://readme-typing-svg.herokuapp.com?font=VT323&size=90&duration=2000&pause=1000&color=F70000&center=true&random=false&width=1100&height=140&lines=%E2%98%A6+VRChat+Local+API+%E2%98%A6;%E2%98%A6+By+Smoke+%E2%98%A6" alt="Ω" /></a>

# VRChat Local API

A simple API that allows interaction with VRChat's local logs & websocket events.

## Features

- Receive real-time WebSocket events even when the game is not running.
- Monitor local log file changes to capture additional events and data when the game is running.
- Supports a variety of events including player joins, leaves, notifications, friend updates, and more.

## Supported Events
+ 🌐 WebSocket -> Indicates the requirement for an auth_cookie 
+ 📝 LogFile -> Indicates the requirement for VRChat to be running
- 
+ 📝 OnPlayerJoined: This event is triggered when a player joins the current session or room in VRChat.
+ 📝 OnPlayerLeft: This event is triggered when a player leaves the current session or room in VRChat.
+ 📝 OnPlayerBlocked: This event is triggered when a player is blocked by the local user in VRChat.
+ 📝 OnPlayerUnBlocked: This event is triggered when a player is unblocked by the local user in VRChat.
+ 📝 OnPlayerAvatarModerationChanged: This event is triggered when the moderation status of a player's avatar changes, being show, hidden, or protected by safety settings.
+ 📝 OnRoomJoined: This event is triggered when the user joins a new room or world in VRChat.
+ 📝 OnRoomLeft: This event is triggered when the user leaves a room or world in VRChat.
+ 🌐 OnNotificationReceived: This event is triggered when the user receives a notification or message in VRChat.
+ 🌐 OnFriendOffline: This event is triggered when a friend goes offline in VRChat.
+ 🌐 OnFriendOnline: This event is triggered when a friend comes online in VRChat.
+ 🌐 OnFriendActive: This event is triggered when a friend becomes active (e.g., logs in to the website) in VRChat.
+ 🌐 OnFriendAdd: This event is triggered when a new friend is added by the user in VRChat.
+ 🌐 OnResponseNotification: This event is triggered when the user receives a response notification in VRChat.
+ 🌐 OnUserLocation: This event is triggered when the user's location or status changes in VRChat.

## Installation
To use the VRChat Local API in your project, you can either download the source code and include it directly or install it via NuGet Package Manager.

### NuGet Installation
```bash
dotnet add package VRChat_Local_API
```
## Usage/Examples
```csharp
// Initialize VRChat instance with authentication cookie
var vrChat = new VRChat("YOUR_AUTH_COOKIE");

// Subscribe to events
vrChat.OnPlayerJoined += (sender, args) =>
{
    // Handle player joined event
};

// Initialize and start listening for events
vrChat.Initialize();

// Shutdown when done
vrChat.Shutdown();
```
