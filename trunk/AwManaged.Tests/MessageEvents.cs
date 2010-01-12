// Stephen Toub

using System;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace NetMatters
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private readonly Message _message;
        public MessageReceivedEventArgs(Message message) { _message = message; }
        public Message Message { get { return _message; } }
    }

    public static class MessageEvents
    {
        private static object _lock = new object();
        private static MessageWindow _window;
        private static IntPtr _windowHandle;
        private static SynchronizationContext _context;

        public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public static void WatchMessage(int message)
        {
            EnsureInitialized();
            _window.RegisterEventForMessage(message);
        }

        public static IntPtr WindowHandle
        {
            get
            {
                EnsureInitialized();
                return _windowHandle;
            }
        }

        private static void EnsureInitialized()
        {
            lock (_lock)
            {
                if (_window == null)
                {
                    _context = AsyncOperationManager.SynchronizationContext;
                    using (ManualResetEvent mre = new ManualResetEvent(false))
                    {
                        Thread t = new Thread((ThreadStart)delegate
                        {
                            _window = new MessageWindow();
                            _windowHandle = _window.Handle;
                            mre.Set();
                            Application.Run();
                        });
                        t.Name = "MessageEvents message loop";
                        t.IsBackground = true;
                        t.Start();

                        mre.WaitOne();
                    }
                }
            }
        }

        private class MessageWindow : Form
        {
            private ReaderWriterLock _lock = new ReaderWriterLock();
            private Dictionary<int, bool> _messageSet = new Dictionary<int, bool>();

            public void RegisterEventForMessage(int messageID)
            {
                _lock.AcquireWriterLock(Timeout.Infinite);
                _messageSet[messageID] = true;
                _lock.ReleaseWriterLock();
            }

            protected override void WndProc(ref Message m)
            {
                _lock.AcquireReaderLock(Timeout.Infinite);
                bool handleMessage = _messageSet.ContainsKey(m.Msg);
                _lock.ReleaseReaderLock();

                if (handleMessage)
                {
                    MessageEvents._context.Post(delegate(object state)
                    {
                        EventHandler<MessageReceivedEventArgs> handler = MessageEvents.MessageReceived;
                        if (handler != null) handler(null, new MessageReceivedEventArgs((Message)state));
                    }, m);
                }

                base.WndProc(ref m);
            }
        }
    }
}