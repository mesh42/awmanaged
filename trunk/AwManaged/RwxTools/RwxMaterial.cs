using SharedMemory;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwManaged.RwxTools
{
    public class RwxMaterial
    {
        private readonly string _texture;

        public RwxMaterial(string texture)
        {
            _texture = texture;
        }

        public string Texture
        {
            get { return _texture; }
        }
    }
}