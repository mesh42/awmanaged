﻿/* **********************************************************************************
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
using System.Collections.Generic;
using System.Linq;
using AwManaged.Core;

namespace AWManaged.Security
{
    [Serializable]
    public enum RoleType
    {
        bot_operator,student,debugger
    }

    [Serializable]
    public class CitizenRole
    {
        public int Citizen;
        public RoleType Role { get; set; }

        public CitizenRole()
        {
            
        }

        public CitizenRole(RoleType role, int citizen)
        {
            Citizen = citizen;
            Role = role;
        }
    }

    [Serializable]
    public class Authorization
    {
        public List<CitizenRole> Matrix { get; set; }

        public Authorization()
        {
            Matrix = new List<CitizenRole>();            
        }



        public bool IsInRole(RoleType role, int citizen)
        {
            var citizenRole =  from p in Matrix where (p.Role == role && p.Citizen == citizen) select p;
            if (citizenRole.Count()>0)
                return true;
            return false;
        }
    }
}
