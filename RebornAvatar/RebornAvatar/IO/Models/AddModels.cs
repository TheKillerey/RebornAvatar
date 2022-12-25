using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RebornAvatar;

using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.IO.OBJ;
using System.IO;

namespace RebornAvatar.IO.Models
{
    public static class AddModels
    {
        public static void Layer(OBJFile obj, MapGeometry mgeo, string path, string name, string console)
        {
            //(List<ushort> indices, List<MapGeometryVertex> vertices) = obj.GetMGEOData();
            //MapGeometrySubmesh submesh = new MapGeometrySubmesh(path, 0, (uint)indices.Count, 0, (uint)vertices.Count);
            //MapGeometryModel room = new MapGeometryModel(name, vertices, indices, new List<MapGeometrySubmesh>() { submesh }, layer);
            //console =  $"Adding {name} | Vertices: {vertices.Count} | Indices: {indices.Count} | Layer: {layer}";
            //if (room.Vertices.Count > 0)
            //{
            //    mgeo.AddModel(room);
            //}
            //else
            //{
            //    return;
            //}
        }
    }
}
