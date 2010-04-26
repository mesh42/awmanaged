using System;
using System.Diagnostics;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Core.Patterns
{
    /// <summary>
    /// A Simple transaction context, not atomic save, not persistable accross domain app boundaries.
    /// </summary>
    public class SimpleTransaction<T> : ITransaction 
        where T : ICloneableT<T>, ITransactionItem, IIntId
    {
        private readonly BotEngine _engine;
        public event TransactionEventCompletedDelegate OnTransactionCompleted;

        internal ProtectedList<T> _transactionList;
        private bool _isCommitting;
        private Stopwatch _sw;
        private int _totalCommits;
        internal bool _finished;

        public void Completed()
        {
            _finished = false;

            foreach (var item in _transactionList)
            {
                item.Hash = 0; // no hashing needed anymore. this is to prevent
            }
            if (OnTransactionCompleted != null)
            {

                OnTransactionCompleted(_engine, new EventTransactionCompletedArgs(this));
            }

        }

        public bool ContainsHash(int hash)
        {
            int count = _transactionList.FindAll(p => p.Hash == hash).Count;
            var o = _transactionList.Find(p => p.Hash == hash);
            return (_transactionList.Find(p => p.Hash == hash) != null);
        }

        public SimpleTransaction(BotEngine engine)
        {
            _sw = new Stopwatch();
            _transactionList = new ProtectedList<T>();
            _engine = engine;
            TransactionId = Guid.NewGuid().GetHashCode();
            Commits = 0;
        }

        internal void Commit()
        {
            _isCommitting = true;
            _sw.Start();
        }

        public void Add(T item)
        {
            if (_isCommitting)
                throw new Exception("You can't add an item to a transaction that is comitting.");
            item.Hash = Guid.NewGuid().GetHashCode();
            item.TransactionId = TransactionId;
            _transactionList.InternalAdd(item.Clone());
            _totalCommits++;
        }
        
        internal bool Commit(T item)
        {
            var result = _transactionList.Find(p => p.Id == item.Id);
            if (result != null)
            {
                result.IsComitted = true;
                Commits++;
                if (CommitsPending == 0)
                {
                    elapsedMs = _sw.ElapsedMilliseconds;
                    //_sw.Stop();
                    _isCommitting = false;
                    _finished = true;   

                }
                return true;
            }
            return false;
        }

        public SimpleTransaction<T> Clone()
        {
            return (SimpleTransaction<T>)MemberwiseClone();
        }


        public int CommitsPending
        {
            get { return _totalCommits-Commits; }
        }

        public int Commits
        {
            get; internal set;
        }

        public int TransactionId
        {
            get; internal set;
        }

        #region ITransaction Members


        public long elapsedMs
        {
            get; private set;
        }

        #endregion
    }
}