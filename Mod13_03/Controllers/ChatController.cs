using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using System.Web;

namespace Mod13_03.Controllers
{
    public class ChatController : ApiController
    {
        //5, 收到連線的要求，建立WebSocketHandler來負責處理連線
        //   並儲存使用者暱稱
        //GET: api/Chat?username=xxx
        public HttpResponseMessage Get(string username)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(username));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }
        class ChatWebSocketHandler : WebSocketHandler
        {
            //使用者暱稱
            private string _username;
            //聊天室集合物件
            private static WebSocketCollection _chatClients =
                new WebSocketCollection();

            //6, 儲存使用者暱稱在ChatWebSocketHandler物件裡
            public ChatWebSocketHandler(string username)
            {
                _username = username;
            }
            //7, 把ChatWebSocketHandler加入聊天室集合物件
            public override void OnOpen()
            {
                _chatClients.Add(this);
            }
            //9,12, 收到Client送來的訊息，並廣播到整個聊天室
            public override void OnMessage(string message)
            {
                _chatClients.Broadcast(_username + ": " + message);
            }
        }

    }
}
