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
using SharedMemory;using System;

namespace AwManaged.Core.Interfaces
{
    public interface IHaveToCleanUpMyShit : IDisposable
    {
        // just to indicate i am lazy in cleaning up my shit, i don't like mandotory tasks.
        // har har.

        // seriously, i don't like to implement IDisposable directly, revert to previous comment.
    }
}
