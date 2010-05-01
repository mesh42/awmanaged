using System.Collections.Generic;

namespace RwxStatistics
{
    using System;
    using System.IO;

    /// <summary>
    /// Main class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point, accepting arguments.
        /// </summary>
        /// <param name="args">
        /// The arguments for the statistics tool.
        /// </param>
        private static void Main(string[] args)
        {
            Console.WriteLine(@"RWX statistics gatherer.");
            if (args.Length == 0 || args.Length != 2)
            {
                Console.WriteLine(@"usage: RwxStatistics <path to rws files> <output file name>");
                return;
            }
            var fi = new FileInfo(args[1]);
            var tw = new StreamWriter(fi.FullName);
            tw.WriteLine("Rwx,Polygons,Quads,Triangles,Verts,Textures");

            var directory = new DirectoryInfo(args[0]);
            var tex = new List<string>();
            foreach (var file in directory.GetFiles("*.rwx"))
            {
                var stat = new AwManaged.RwxTools.RwxSimpleStatistics(file);
                string textures = string.Empty;
                int n = 0;
                foreach (var material in stat.Materials)
                {
                    if (material.Texture != string.Empty && material.Texture.ToLower() != "null")
                    {
                        if (!tex.Contains(material.Texture.ToLower()))
                        {
                            tex.Add(material.Texture.ToLower());
                            if (n > 0)
                            {
                                textures += "|";
                            }
                            textures += material.Texture;
                            n++;
                        }
                    }
                }

                tw.WriteLine(stat.RwxFile.Name + "," + stat.Polygons + "," + stat.Quads + "," + stat.Triangles + "," + stat.Verts + "," + textures);
            }
            tw.Flush();
            tw.Close();
        }
    }
}
