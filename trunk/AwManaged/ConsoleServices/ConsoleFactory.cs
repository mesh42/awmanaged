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
using AwManaged.ConsoleServices.Interfaces;

namespace AwManaged.ConsoleServices
{
    public class ConsoleFactory<T>
        where T : IConsoleSystem, new()
    {
        public T GetInstance()
        {
            return new T();
        }
    }
}
