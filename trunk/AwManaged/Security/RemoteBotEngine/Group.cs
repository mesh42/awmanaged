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
using AwManaged.Security.RemoteBotEngine.Interfaces;

namespace AwManaged.Security.RemoteBotEngine
{
    public class Group : IHaveAuthorization
    {

        #region IHaveAuthorization Members

        System.Guid IHaveAuthorization.Id
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
