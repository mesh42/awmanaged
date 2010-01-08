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
using System.Drawing;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The media command enables media player functionality within a scene for playback of local- and web-based media files, as well as broadcast streams. All file formats which are supported by your installed version of Windows Media Player will be supported in Activeworlds, except play-lists. However, this does not guarantee all users will be able to support all listed formats with a clean installation of Windows Media Player. Windows Media metafiles (.asx, .wax, .wvx, .wmx, .wpl) are not supported.
    /// Note: Windows Media Player 9 is recommended for best performance. Some media content cannot be displayed correctly on older players, due to missing format decompression support
    /// The create trigger only works with the media command, if "Disable create url" is not checked in the world features settings. Other options that affect the media command are in the General tab and in the Downloads tab of the browser option settings.
    /// </summary>
    public sealed class ACMedia : IActionCommand
    {
        private string _url;
        private string _infoText;
        private string _name;
        private MediaPlayType _playType;
        private Color _color;
        private ResType _res;
        private MediaSoundFx _fx;
        private bool _set;
        private float _radius;
        private float _radoff;
        private byte _vol;
        private int _loop;
        private bool _osd;
        private bool _ext;
        private bool _nostop;

        public ACMedia(){}

        public ACMedia(string url, string infoText, string name, MediaPlayType playType,  Color color, ResType res, MediaSoundFx fx, bool set, float radius, float radof, byte vol, int loop, bool osd, bool ext, bool nostop)
        {
            _url = url;
            _infoText = infoText;
            _name = name;
            _playType = playType;
            _color = color;
            _res = res;
            _fx = fx;
            _set = set;
            _radius = radius;
            _radoff = radof;
            _vol = vol;
            _loop = loop;
            _osd = osd;
            _ext = ext;
            _nostop = nostop;
        }

        /// <summary>
        /// Set changes the attributes of an already playing media on the fly, without pausing or stopping the stream. Note, the resolution cannot be changed on the fly.
        /// </summary>
        /// <value><c>true</c> if set; otherwise, <c>false</c>.</value>
        /// todo: interpret this bool flag
        public bool Set
        {
            get { return _set; }
            set { _set = value; }
        }

        /// <summary>
        /// nostop disables the mouse click or bump trigger to stop running media. Objects using the bump trigger always have this option set, to avoid subsequent on/off triggering of the media command. In conjunction with the activate trigger, this disables the toggle-switch-function, which is applied on media signs by default. When used with the create trigger the nostop argument has no effect.
        /// </summary>
        /// <value><c>true</c> if nostop; otherwise, <c>false</c>.</value>
        /// todo: interpret this bool flag
        public bool Nostop
        {
            get { return _nostop; }
            set { _nostop = value; }
        }

        /// <summary>
        /// ext forces the video to open an external window. If the media only contains audio, no external window is displayed. Valid values are on/off, yes/ no, or true/false (default off).
        /// </summary>
        /// <value><c>true</c> if ext; otherwise, <c>false</c>.</value>
        /// todo: interpret this bool flag
        public bool Ext
        {
            get { return _ext; }
            set { _ext = value; }
        }

        /// <summary>
        /// osd (on-screen display) specifies if a TV-like on-screen-display should be displayed on top of the video frames. Valid values are on/off, yes/ no, or true/false (default on).
        /// </summary>
        /// <value><c>true</c> if osd; otherwise, <c>false</c>.</value>
        /// todo: interpret this bool flag
        public bool Osd
        {
            get { return _osd; }
            set { _osd = value; }
        }

        /// <summary>
        /// Loop specifies the count of loops to play the specified media file. Valid values are in the range from 1 to 2,147,483,647. Default is 1.
        /// </summary>
        /// <value>The loop.</value>
        [ACItemBinding("loop", CommandInterpretType.NameValuePairs)]
        public int Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        /// <summary>
        /// Vol specifies the relative volume of the played sound, in percent value. Valid values are in the range from 0 to 100, where 0% is silence (mute) and 100% is full volume. Default is 100%.
        /// </summary>
        /// <value>The vol.</value>
        [ACItemBinding("vol", CommandInterpretType.NameValuePairs)]
        public byte Vol
        {
            get { return _vol; }
            set { _vol = value; }
        }

        /// <summary>
        /// Radoff specifies the maximum distance in meters the media stream will play. The default radoff is 30 meters. radoff cannot be smaller than radius. Outside the given radoff an ongoing media is stopped.
        /// </summary>
        /// <value>The radof.</value>
        [ACItemBinding("radoff", CommandInterpretType.NameValuePairs)]
        public float Radoff
        {
            get { return _radoff; }
            set { _radoff = value; }
        }

        /// <summary>
        /// Radius specifies the maximum distance in meters the sound can be heard. The default radius is 30 meters.
        /// </summary>
        /// <value>The radius.</value>
        [ACItemBinding("radius", CommandInterpretType.NameValuePairs)]
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// FX specifies a sound effect applied to the media.
        /// </summary>
        /// <value>The fx.</value>
        [ACItemBinding("fx", CommandInterpretType.NameValuePairs)]
        public MediaSoundFx Fx
        {
            get { return _fx; }
            set { _fx = value; }
        }

        /// <summary>
        /// Res specifies resolution used to the displayed video frames in pixel, if the media contains a video. Valid values are 64, 128, 256 and 512 pixels (default 256).
        /// Note that the proportions of the video (height and width) only depends on the sign's proportions and not on the used resolution.
        /// </summary>
        /// <value>The res.</value>
        [ACItemBinding("res", CommandInterpretType.NameValuePairs)]
        public ResType Res
        {
            get { return _res; }
            set
            {
                if ((int)value > 512 || (int)value <64 )
                    throw new Exception("Valid values are 64, 128, 256 and 512 pixels (default 256).");
                _res = value;
            }
        }

        /// <summary>
        /// Color specifies the color to use for the sign text, and bcolor specifies the sign's background color. Both arguments are optional. The default color scheme is white text on a blue background. The colors can either be specified as one of many preset word values or as a "raw" hexadecimal value giving the red/green/blue component values (the same format as used for the BGCOLOR= HTML tag).
        /// </summary>
        /// <value>The color.</value>
        [ACItemBinding("color", CommandInterpretType.NameValuePairs)]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        [ACItemBinding(CommandInterpretType.Flag)]
        public MediaPlayType PlayType
        {
            get { return _playType; }
            set { _playType = value; }
        }

        /// <summary>
        /// Name specifies the name of the same owner's object to place the media on.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name", CommandInterpretType.NameValuePairs)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Info text is text for the sign object. It will be displayed while the media is loading and after the media has finished. If omitted, the info text defaults to the contents of the object's description field (this is also the most common form of the sign command.) If specified, the into text must be enclosed in double quotes. Optionally, if the url or path includes spaces, the into text must be used to take the url or path, and the url option must be omitted.
        /// </summary>
        /// <value>The info text.</value>
        // todo: interpret this.
        public string InfoText
        {
            get { return _infoText; }
            set { _infoText = value; }
        }

        /// <summary>
        /// The URL command, the only required argument, specifies the url, web-address or path to the media file. If the url is empty and a name is specified, it will stop running media on all objects of the triggered object's owner within the current view range. The url-prefix defaults to "http://" if no other protocol is specified.
        /// </summary>
        /// <value>The URL.</value>
        [ACItemBinding("url", CommandInterpretType.NameValuePairs)]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "media"; }
        }

        public string LiteralPart { get; set; }

        #endregion
    }
}
