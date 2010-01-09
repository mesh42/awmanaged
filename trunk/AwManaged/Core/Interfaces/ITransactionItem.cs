namespace AwManaged.Core.Interfaces
{
    public interface ITransactionItem
    {
        /// <summary>
        /// Gets or sets the hash for tracking object changes.
        /// </summary>
        /// <value>The hash.</value>
        int Hash { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this transaction item is comitted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is comitted; otherwise, <c>false</c>.
        /// </value>
        bool IsComitted { get; set; }
        TransactionItemType TransactionItemType { get; }
        /// <summary>
        /// Gets the transaction id if this transactionItem is part of a transaction.
        /// </summary>
        /// <value>The transaction id.</value>
        int TransactionId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is creating from this runtime.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is bot runtime creation; otherwise, <c>false</c>.
        /// </value>
        bool IsRuntimeTransaction { get; set; }
    }
}
