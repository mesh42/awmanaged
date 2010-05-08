using SharedMemory;using System;
using System.Drawing;
using System.Windows.Forms;
using AwManaged.ConsoleServices.Interfaces;

namespace AwManaged.ConsoleServices
{
    /// <summary>
    /// Provides a console system in a rich textbox
    /// </summary>
	public sealed class WinFormConsole : IConsoleSystem
	{
        private RichTextBox _textBox;

        //private System.Drawing.Color Convert(ConsoleColor color)
        //{
        //    Color ret;

        //    Color.FromKnownColor(KnownColor.)

        //    switch (color)
        //    {
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Blue:
        //            break;
        //        case ConsoleColor.Cyan:
        //            break;
        //        case ConsoleColor.DarkBlue:
        //            break;
        //        case ConsoleColor.DarkCyan:
        //            break;
        //        case ConsoleColor.
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //        case ConsoleColor.Black:
        //            break;
        //    }
        //}


        public WinFormConsole(RichTextBox textBox)
        {
            _textBox = textBox;
        }


        #region IConsoleSystem Members

        public System.ConsoleColor BackgroundColor
        {
            get; set;
        }

        public string Title
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool IsPromptEnabled
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ReadLine()
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(ConsoleMessageType type, string text)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(string text)
        {
            throw new System.NotImplementedException();
        }

        public void Write(ConsoleMessageType type, string text)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string text)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
