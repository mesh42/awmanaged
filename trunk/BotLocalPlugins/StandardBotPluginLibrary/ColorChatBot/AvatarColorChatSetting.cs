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
using System.Drawing;
using Db4objects.Db4o.Config.Attributes;

namespace StandardBotPluginLibrary.ColorChatBot
{
    /// <summary>
    /// Storage class for storing color chat settings.
    /// </summary>
    public class AvatarColorChatSetting
    {
        [Indexed] private int _citizen;
        private bool _isBold;
        private bool _isItalic;        
        private Color _color;

        public AvatarColorChatSetting(int citizen, Color color)
        {
            _citizen = citizen;
            _isBold = false;
            _isItalic = false;
            _color = color;
        }

        public AvatarColorChatSetting(int citizen, bool isBold, bool isItalic, Color color)
        {
            _citizen = citizen;
            _isBold = isBold;
            _isItalic = isItalic;
            _color = color;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool IsItalic
        {
            get { return _isItalic; }
            set { _isItalic = value; }
        }

        public bool IsBold
        {
            get { return _isBold; }
            set { _isBold = value; }
        }

        public int Citizen
        {
            get { return _citizen; }
            set { _citizen = value; }
        }
    }
}
