using System;

namespace BLL.Network
{
    public class Message
    {
        public string UserLogin { get; set; }
        public string SendTime { get; set; }
        public string MessageText { get; set; }


        public override string ToString()
        {
            return UserLogin + "||" + SendTime + "||" + MessageText;
        }
    }
}