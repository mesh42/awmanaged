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
namespace AwManaged.Storage.Interfaces
{
    public interface IDb4OConnection 
    {
        string HostAddress { get; }
        int HostPort { get; }
        string File { get; }
        string User { get; }
        string Password { get; }
    }
}