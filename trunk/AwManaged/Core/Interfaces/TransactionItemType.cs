namespace AwManaged.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public enum TransactionItemType
    {
        /// <summary>
        /// The object is not in a transaction.
        /// </summary>
        NaN = 0,
        /// <summary>
        /// The object was obtained using a world scan issued by the bot.
        /// </summary>
        Scan = 1,
        /// <summary>
        /// The object was obtained using the bot add command
        /// </summary>
        Add = 2,
        /// <summary>
        /// 
        /// </summary>
        Remove = 3,
        /// <summary>
        /// 
        /// </summary>
        Change=4
    }
}