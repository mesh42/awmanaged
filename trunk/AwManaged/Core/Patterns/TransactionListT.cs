using System;
using System.Collections.Generic;
using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Patterns
{
    public delegate void CallbackCapacityReached<T>(TransactionListT<T> sender, EventArgs e)
        where T : ITransactionItem, ICloneableT<T>, IIdentifiable;

    public delegate void CallbackTransactionCompleted<T>(TransactionListT<T> sender, EventArgs e)
        where T : ITransactionItem, ICloneableT<T>, IIdentifiable;

    public class TransactionListT<T>
        where T : ITransactionItem, ICloneableT<T>,IIdentifiable
    {
        public event CallbackCapacityReached<T> TransactionCapacityReached;
        public event CallbackCapacityReached<T> TransactionCompleted;

        private readonly Delegate _callbackCapacityReached;
        private readonly Delegate _callbackTransactionCompleted;
        private readonly int _capacity;

        /// <summary>
        /// The maximum capacity for each transaction.
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get { return _capacity; }
        }

        public Guid TransactionId { get; internal set; }
        private List<ProtectedList<T>> _transactionItems;
        
        //public ProtectedList<T>[] TransactionItems { get { return _transactionItems[].Clone(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionListT&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="callbackCapacityReached">The callback capacity reached.</param>
        /// <param name="callbackTransactionCompleted">The callback transaction completed.</param>
        /// <param name="capacity">The capacity per sub transaction.</param>
        public TransactionListT(CallbackCapacityReached<T> callbackCapacityReached, CallbackTransactionCompleted<T> callbackTransactionCompleted, int capacity)
        {
            _callbackCapacityReached = callbackCapacityReached;
            _callbackTransactionCompleted = callbackTransactionCompleted;
            _capacity = capacity;
            _transactionItems = new List<ProtectedList<T>>();
        }

        public void Add(T transactionItem)
        {

        }
    }
}