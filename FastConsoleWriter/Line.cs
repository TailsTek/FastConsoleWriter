using System;
using System.Collections.Generic;
using System.Text;

namespace FastConsoleWriter
{
    public struct Line
    {
        public bool IsLine;
        public string Text;

        public Line(string text, bool isline = false)
        {
            Text = text;
            IsLine = isline;
        }
    }
}
