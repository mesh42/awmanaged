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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AwManaged.ConsoleServices;
using AwManaged.Core;
using AwManaged.Scene;
using AWManaged.Security;
using NetMatters;

namespace AwManaged.Tests
{
    public class Program
    {
        #region Imports 

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        #endregion

        #region WIN32API

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }


        private const int SW_RESTORE = 9;

        internal const uint SC_CLOSE = 0xF060;
        internal const uint MF_GRAYED = 0x00000001;
        internal const uint MF_BYCOMMAND = 0x00000000;
        internal const uint MF_ENABLED = 0x00000000;
        internal const uint MF_DISABLED = 0x00000002;
        internal const uint SC_MINIMIZE = 0xF020;
        internal const uint SC_MAXIMIZE = 0xF030;
        internal const uint SC_RESTORE = 0xF120;
        internal const int WM_SYSCOMMAND = 0x0112;
        internal const int SW_SHOWMAXIMIZED = 3;

        #endregion

        static void ProcessCommandLine(string commandLine)
        {

            if (commandLine != string.Empty)
            {
                if (commandLine == "boot")
                {
                    ConsoleHelpers.GetPromptTarget = null;
                    ConsoleHelpers.ParseCommandLine = null;
                    var bot = new BotEngineExample();
                    return;
                }
            }
            else
            {
                ConsoleHelpers.WriteLine("?Error");
            }
            ConsoleHelpers.ReadLine();
        }

        static string GetPrompt()
        {
            return "[BOT OS>: ";
        }

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorSize = 100;
            Console.WindowWidth = 120;
            Console.WindowHeight = 48;
            Console.BufferHeight = 48;

            WINDOWPLACEMENT wp = new WINDOWPLACEMENT();

            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr hSystemMenu = GetSystemMenu(hMenu, false);
            ShowWindow(hMenu, SW_SHOWMAXIMIZED);
            EnableMenuItem(hSystemMenu, SC_RESTORE, MF_GRAYED);
            RemoveMenu(hSystemMenu, SC_RESTORE, MF_BYCOMMAND);
            EnableMenuItem(hSystemMenu, SC_CLOSE, MF_GRAYED);
            RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND);

            wp.length = Marshal.SizeOf(wp);
            GetWindowPlacement(hMenu, ref wp);
            wp.ptMaxPosition.X = 10;
            wp.ptMaxPosition.Y = 20;
            SetWindowPlacement(hMenu, ref wp);


            if (IsIconic(hMenu))
            {
                ShowWindow(hMenu, SW_RESTORE);
            }
            SetForegroundWindow(hMenu);

            if (args!=null && args.Length > 0)
            {
                if (args[0] == "/autoboot")
                {
                    var bot = new BotEngineExample();
                }
                else
                {

                    ConsoleHelpers.GetPromptTarget = GetPrompt;
                    ConsoleHelpers.ParseCommandLine = ProcessCommandLine;
                    ConsoleHelpers.WriteLine("Bot operating system.");
                    ConsoleHelpers.ReadLine();
                }
            }
            else
            {

                //GetWindowPlacement(hMenu, ref wp);
                ConsoleHelpers.GetPromptTarget = GetPrompt;
                ConsoleHelpers.ParseCommandLine = ProcessCommandLine;
                ConsoleHelpers.WriteLine("Bot operating system.");
                ConsoleHelpers.ReadLine();
            }

            //// TODO: update this with your own privileges.
            //var bot = new BotEngineExample();
            //var buildStats = new List<BuildStat>();


            //var sn1 = bot.SceneNodes;
            //var sn2 = bot.SceneNodes;
            
            //var model1 = sn1.Models[0];
            //var model2 = sn2.Models[0];

            //model1.Action = "sdkfjdlfjsdklfdjldkfjs";
            //model2.Action = " dslkfjfkasdfjaklsdfjlksfj";
            
            //var diff = Differential.Properties(model1,model2);

            //foreach (var model in bot.SceneNodes.Models)
            //{
            //    var buildStat = buildStats.Find(p => p.Avatar.Citizen == model.Owner);
            //    if (buildStat == null)
            //    {
            //        buildStat = new BuildStat(new Avatar() {Citizen = model.Owner}, 0);
            //        buildStats.Add(buildStat);
            //    }
            //    buildStat.ObjectCount++;
            //}
            //buildStats.Sort((p1, p2) => p2.ObjectCount.CompareTo(p1.ObjectCount));
            ////Console.WriteLine("VRT: " + bot.VrtTime());
            //Console.Read();
        }
        private class BuildStat
        {
            public Avatar Avatar { get; set; }
            public int ObjectCount { get; set; }

            public BuildStat(Avatar avatar, int objectCount)
            {
                Avatar = avatar;
                ObjectCount = objectCount;
            }
        }
    }
}