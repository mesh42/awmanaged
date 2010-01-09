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
using AwManaged.Core.Interfaces;

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void TransactionEventCompletedDelegate(AwManaged.BotEngine sender, EventTransactionCompletedArgs e);

    public sealed class EventTransactionCompletedArgs : MarshalByRefObject
    {
        public EventTransactionCompletedArgs(ITransaction transaction)
        {
            Transaction = transaction;
        }

        public ITransaction Transaction { get;private set; }
    }
}