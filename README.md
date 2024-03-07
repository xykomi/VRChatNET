<a href="Ω"><img src="http://readme-typing-svg.herokuapp.com?font=VT323&size=90&duration=2000&pause=1000&color=F70000&center=true&random=false&width=1100&height=140&lines=%E2%98%A6+VRChat+Local+API+%E2%98%A6;%E2%98%A6+By+Smoke+%E2%98%A6" alt="Ω" /></a>

# VRChat Local API

A simple API that allows interaction with VRChat's local logs & websocket events.

## Features

- Receive real-time WebSocket events even when the game is not running.
- Monitor local log file changes to capture additional events and data when the game is running.
- Supports a variety of events including player joins, leaves, notifications, friend updates, and more.

## Supported Events

- OnPlayerJoined
- OnPlayerLeft
- OnPlayerBlocked
- OnPlayerUnBlocked
- OnPlayerAvatarModerationChanged: Safety, Hidden, Shown
- OnRoomJoined
- OnRoomLeft
- OnNotificationReceived
- OnFriendLocationUpdate
- OnFriendOffline
- OnFriendOnline
- OnFriendActive
- OnFriendAdd
- OnResponseNotification
- OnUserLocation

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
