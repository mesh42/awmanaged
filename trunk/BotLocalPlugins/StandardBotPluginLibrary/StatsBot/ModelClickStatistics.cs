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
using Db4objects.Db4o.Config.Attributes;

namespace StandardBotPluginLibrary.StatsBot
{
    /// <summary>
    /// This is a simple helper class, for storing statistics on clicked objects.
    /// </summary>
    public class ModelClickStatistics
    {
        [Indexed]
        private int _clicks;
        [Indexed]
        private int _modelId;

        public int Clicks { get { return _clicks; } set { _clicks = value; } }
        public int ModelId { get { return _modelId; } set { _modelId = value; } }
    }
}