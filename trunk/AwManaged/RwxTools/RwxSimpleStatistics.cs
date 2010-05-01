using System.Collections.Generic;

namespace AwManaged.RwxTools
{
    using System.IO;

    /// <summary>
    /// Simple statics gathering. Such as number of triangles, vertices, quads, textures used and MD5 calculation on geometry to determine duplicate geometry.
    /// Does not group into clumps etc and is definately not meant for interpreting RWX files.
    /// </summary>
    public class RwxSimpleStatistics
    {
        private readonly FileInfo _rwxFile;
        private readonly int _polygons;
        private readonly int _quads;
        private readonly int _triangles;
        private readonly int _verts;
        private readonly List<RwxMaterial> _materials;

        /// <summary>
        /// Initializes a new instance of the <see cref="RwxSimpleStatistics"/> class.
        /// </summary>
        /// <param name="rwxFile">
        /// The rwx file.
        /// </param>
        public RwxSimpleStatistics(FileInfo rwxFile)
        {
            _materials = new List<RwxMaterial>();
            _rwxFile = rwxFile;
            var reader = _rwxFile.OpenText();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                var linelower = line.ToLower();
                if (linelower.StartsWith("polygon"))
                {
                    _polygons++;
                }
                if (linelower.StartsWith("vertex"))
                {
                    _verts++;
                }
                if (linelower.StartsWith("triangle"))
                {
                    _triangles++;
                }
                if (linelower.StartsWith("quad"))
                {
                    _quads++;
                }
                if (linelower.StartsWith("texture"))
                {
                    if (!linelower.Substring(7).Trim().StartsWith("modes "))
                    {
                        if (!linelower.Substring(7).Trim().StartsWith("mode "))
                        {
                            if (!linelower.Substring(7).Trim().StartsWith("addressmode "))
                            {
                                Materials.Add(new RwxMaterial(linelower.Substring(7).Trim().Split(' ')[0]));
                            }
                        }
                    }
                }
            }

        }

        public FileInfo RwxFile
        {
            get { return _rwxFile; }
        }

        public int Polygons
        {
            get { return _polygons; }
        }

        public int Quads
        {
            get { return _quads; }
        }

        public int Triangles
        {
            get { return _triangles; }
        }

        public int Verts
        {
            get { return _verts; }
        }

        public List<RwxMaterial> Materials
        {
            get { return _materials; }
        }
    }
}
