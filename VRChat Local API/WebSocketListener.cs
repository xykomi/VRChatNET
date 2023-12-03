using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChat_Local_API.Objects;
using WebSocketSharp;

namespace VRChat_Local_API
{
    public class WebSocketListener
    {
        private WebSocket? webSocket { get; set; } = null;
        private string authCookie { get; set; } = string.Empty;

        //public event EventHandler<FriendAddObject> OnFriendAdded = null!;
        //public event EventHandler<FriendDeleteObject> OnFriendDelete = null!;

        //public void Initialize(string authcookie)
        //{
        //    if (authCookie == string.Empty)
        //        authCookie = authcookie;

        //    new Thread(SocketThread).Start();
        //}

        private void SocketThread()
        {
            if (authCookie == string.Empty)
                throw new Exception("Please provide a valid authcookie.");

            webSocket = new WebSocket("wss://pipeline.vrchat.cloud/?authToken=" + authCookie);
            webSocket.OnOpen += WebSocket_OnOpen;
            webSocket.OnClose += WebSocket_OnClose;
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.OnError += WebSocket_OnError;
            webSocket.Connect();
        }

        private void WebSocket_OnError(object? sender, WebSocketSharp.ErrorEventArgs e)
        {
            
        }

        private void WebSocket_OnClose(object? sender, CloseEventArgs e)
        {
            
        }

        private void WebSocket_OnOpen(object? sender, EventArgs e)
        {
            
        }

        private void WebSocket_OnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == string.Empty)
                return;

            WebSocketDataObject webSocketDataObject = JsonConvert.DeserializeObject<WebSocketDataObject>(e.Data);
            switch (webSocketDataObject.type)
            {
                case "friend-add":
                    FriendAddObject? friendAddObject = JsonConvert.DeserializeObject<FriendAddObject>(webSocketDataObject.content);
                    //OnFriendAdded?.Invoke(this, friendAddObject);
                    break;

                case "friend-delete":
                    FriendDeleteObject? friendDeleteObject = JsonConvert.DeserializeObject<FriendDeleteObject>(webSocketDataObject.content);
                    //OnFriendDelete?.Invoke(this, friendDeleteObject);
                    break;
            }
        }
    }
}
