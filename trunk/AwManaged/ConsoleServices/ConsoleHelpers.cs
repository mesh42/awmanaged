/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.Collections.Generic;

namespace AwManaged.ConsoleServices
{
    /// <summary>
    /// Cheesy console static methods, which i cooked up quickly. far form perfect but enough for now.
    /// </summary>
    public class ConsoleHelpers
    {
        static ConsoleHelpers()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
        }

        /// <summary>
        /// Disable special keys.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ConsoleCancelEventArgs"/> instance containing the event data.</param>
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            switch (e.SpecialKey)
            {
                case ConsoleSpecialKey.ControlC:
                    e.Cancel = true;
                    break;
                default:
                    e.Cancel = false;
                    break;
            }
        }

        public static ConsoleColor Prompt = ConsoleColor.White;

        public delegate string GetPrompt();
        public static GetPrompt GetPromptTarget { get; set; }

        public static ParseCommandLineDelegate ParseCommandLine;
        public delegate void ParseCommandLineDelegate(string commandLine);

        private static List<Char> keyBuffer = new List<char>();

        private static bool _isReadlineMode;

        private static string KeyBufferToString() {
            string commandLine = string.Empty;
            foreach (char item in keyBuffer)
                commandLine += item;
            return commandLine;
        }

        public static bool IsPromptEnabled;

        static System.Threading.Thread t;

        public static void ReadLine()
        {
            if (GetPromptTarget != null)
            {
                var oldColor = Console.ForegroundColor;
                Console.CursorLeft = 0;
                Console.ForegroundColor = Prompt;
                Console.Write(GetPromptTarget());
                //Console.ForegroundColor = oldColor;
            }
            t = new System.Threading.Thread(readLine);
            t.Start();
        }

        internal static void ScrollDown()
        {
            if (Console.CursorTop == Console.WindowHeight-1)
            {
                Console.MoveBufferArea(0, 1, Console.WindowWidth, Console.WindowHeight - 1, 0, 0);
                return;
            }
            Console.CursorTop++;

        }



        internal static void readLine()
        {
            _isReadlineMode = true;
            while (1 == 1)
            {

                var keyInfo = Console.ReadKey(true);
                if (keyInfo.KeyChar == 13 && KeyBufferToString() != string.Empty)
                {
                    string commandLine = string.Empty;
                    commandLine = KeyBufferToString();
                    keyBuffer.Clear();
                    ScrollDown();
                    _isReadlineMode = false;
                    ParseCommandLine(commandLine);
                    return;
                }
                if (keyInfo.KeyChar != 13 && (keyInfo.KeyChar > 31 && keyInfo.KeyChar < 127))
                {
                    keyBuffer.Add(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
                else
                {
                    if (keyInfo.KeyChar == 8)
                    {
                        if (keyBuffer.Count > 0)
                        {
                            keyBuffer.RemoveAt(keyBuffer.Count - 1);
                            Console.Write(keyInfo.KeyChar);
                            Console.Write(" ");
                            Console.CursorLeft--;
                        }
                    }
                }
            }
        }

        public static void WriteLine(ConsoleColor color, string text)
        {
            for (int i=0;i<text.Length;i=i+Console.WindowWidth)
            {
                if (text.Length - i < Console.WindowWidth)
                    _writeLine(color, text.Substring(i, text.Length - i));
                else 
                    Write(color,text.Substring(i, Console.WindowWidth));
            }

            if (Console.CursorTop > 0 /* && _isReadlineMode*/)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = Prompt;
                if (GetPromptTarget != null)
                    Console.Write(GetPromptTarget() + KeyBufferToString());
                Console.ForegroundColor = oldColor;
            }

        }

        private static void _writeLine(ConsoleColor color, string text)
        {
            // move prompt to next line.
            if (Console.CursorTop > 0)
            {
                Console.Write("".PadRight(Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            Console.ForegroundColor = color;
            Console.WriteLine(text.PadRight(Console.WindowWidth - text.Length)); // Todo handle width > 120
        }

        public static void WriteLine(string text)
        {
            //if (t != null) t.Suspend();
            WriteLine(ConsoleColor.Gray, text);
            //if (t != null) t.Resume();
        }

        public static void Write(ConsoleColor color, string text)
        {
            int left = Console.CursorLeft;
            // move prompt to next line.
            if (Console.CursorTop > 0)
            {
                Console.Write("".PadRight(Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            Console.ForegroundColor = color;
            Console.Write(text.PadRight(Console.WindowWidth - text.Length)); // Todo handle width > 120
            if (Console.CursorTop > 0 && _isReadlineMode)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = Prompt;
                if (GetPromptTarget != null)
                    Console.Write(GetPromptTarget() + KeyBufferToString());
                Console.ForegroundColor = oldColor;
            }
        }

        public static void Write(string text)
        {
            int left = Console.CursorLeft;
            // move prompt to next line.
            if (Console.CursorTop > 0)
            {
                Console.Write("".PadRight(Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text.PadRight(Console.WindowWidth - text.Length));
            if (Console.CursorTop > 0 && _isReadlineMode)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = Prompt;
                if (GetPromptTarget != null)
                    Console.Write(GetPromptTarget() + KeyBufferToString());
                Console.ForegroundColor = oldColor;
            }
        }
    }
}