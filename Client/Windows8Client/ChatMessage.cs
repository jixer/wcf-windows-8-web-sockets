﻿using System;
using System.Runtime.Serialization;

namespace Myers.WebSockDemo.Contract
{
    public class ChatMessage
    {
        public string From { get; set; }
        public string MessageText { get; set; }
    }
}
