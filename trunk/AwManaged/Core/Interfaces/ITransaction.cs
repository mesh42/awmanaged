using AwManaged.EventHandling.BotEngine;

namespace AwManaged.Core.Interfaces
{
    public interface ITransaction
    {
        int CommitsPending { get;  }
        int Commits { get;  }
        int TransactionId { get;  }
        long elapsedMs { get; }
        event TransactionEventCompletedDelegate OnTransactionCompleted;
    }
}