﻿using Abstractions;
using DataStructures;

namespace Messages
{
    public class HtmlDownloadedUEEvent : IEvent
    {
        public HtmlDownloadedUEEvent()
        {

        }

        public HtmlDownloadedUEEvent(Url url, string html)
        {
            Url = url;
            Html = html;
        }

        public Url Url { get; set; }

        public string Html { get; set; }

        public int Port => 5555;
    }
}
