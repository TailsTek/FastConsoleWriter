using System;
using System.Collections.Generic;
using System.Text;

namespace FastConsoleWriter
{
    public struct Line
    {
        public bool IsLine;
        public string Text;
        public ConsoleColor Color;

        public Line(string text, ConsoleColor color = ConsoleColor.Gray, bool isline = false)
        {
            Text = text;
            IsLine = isline;
            Color = color;
        }
    }
}
