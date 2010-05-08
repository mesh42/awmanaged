// /* **********************************************************************************
//  *
//  * Copyright (c) Sky Sanders. All rights reserved.
//  * 
//  * This source code is subject to terms and conditions of the Microsoft Public
//  * License (Ms-PL). A copy of the license can be found in the license.htm file
//  * included in this distribution.
//  *
//  * You must not remove this notice, or any other, from this software.
//  *
//  * **********************************************************************************/
using SharedMemory;using System;
using System.Windows.Forms;
using Cassini.CommandLine;

namespace CassiniDev
{
    /// <summary>
    /// 12/29/09 sky: Implemented more robust command line argument parser (CommandLineParser.cs)
    /// 12/29/09 sky: Implemented hosts file modification mode allowing this executable to be run in an elevated process
    ///               to add/remove hosts file entry corresponding to specified hostname, if desired.
    /// 12/29/09 sky: Implemented a MVP pattern with service locator and abstract factory for testing and to simplify 
    ///               gui and console with same codebase
    /// 
    /// Issues:
    /// FormView.Stop() doesn't seem to kill the server host? works in console.
    /// Is not a critical issue yet - starting another server on same port works just fine.
    /// I think it may have to do with vshost.exe persistance as killing vs stops it.
    /// 
    /// 
    /// /a:"E:\Projects\cassinidev\trunk\TestWebApp" /v:"/" /h:mycomputer /ah+ /im:Specific /i:192.168.1.102 /v6- /pm:Specific /p:8082 /prs:0 /pre:0
    /// /a:"E:\Projects\cassinidev\trunk\TestWebApp"
    /// </summary>
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            CommandLineArguments sargs = new CommandLineArguments();
#if GUI
            if (!Parser.ParseArguments(args, sargs))
            {
                string usage = Parser.ArgumentsUsage(typeof (CommandLineArguments), 120);
                MessageBox.Show(usage);
                Environment.Exit(-1);
                return;
            }
            switch (sargs.RunMode)
            {
                case RunMode.Server:
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    using (IPresenter presenter = ServiceFactory.CreatePresenter())
                    {
                        IView view = ServiceFactory.CreateFormsView();
                        presenter.InitializeView(view, sargs);
                        Application.Run((Form) view);
                    }
                    break;
                case RunMode.Hostsfile:
                    SetHostsFile(sargs);
                    break;
            }
#endif
#if CONSOLE
            if (!Parser.ParseArgumentsWithUsage(args, sargs))
            {
                Environment.Exit(-1);
            }
            else
            {
                switch (sargs.RunMode)
                {
                    case RunMode.Server:
                        using (IPresenter presenter = ServiceFactory.CreatePresenter())
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(sargs.ApplicationPath))
                                {
                                    throw new CassiniException("ApplicationPath is null.", ErrorField.ApplicationPath);
                                }
                                IView view = ServiceFactory.CreateConsoleView();
                                presenter.InitializeView(view, sargs);
                                Console.WriteLine("started: {0}\r\nPress Enter key to exit....", presenter.Server.RootUrl);
                                Console.ReadLine();
                                view.Stop();
                            }
                            catch (CassiniException ex)
                            {
                                Console.WriteLine("error:{0} {1}",
                                                        ex.Field == ErrorField.None
                                                            ? ex.GetType().Name
                                                            : ex.Field.ToString(), ex.Message);
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine("error:{0}", ex2.Message);
                                Console.WriteLine(Parser.ArgumentsUsage(typeof(CommandLineArguments)));
                            }
                        }
                        break;
                    case RunMode.Hostsfile:
                        SetHostsFile(sargs);
                        break;
                }
            }
#endif
        }

        private static void SetHostsFile(CommandLineArguments sargs)
        {
            try
            {
                if (sargs.AddHost)
                {
                    ServiceFactory.Rules.AddHostEntry(sargs.IPAddress, sargs.HostName);
                }
                else
                {
                    ServiceFactory.Rules.RemoveHostEntry(sargs.IPAddress, sargs.HostName);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Environment.Exit(-1);
            }
            catch
            {
                Environment.Exit(-2);
            }
        }
    }
}