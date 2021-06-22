using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Reflection;

namespace core.Tools
{
    public sealed class GeometryTool
    {
        public static GeometryTool Instance => _Instance.Value;

        private static readonly Lazy<GeometryTool> _Instance =
            new Lazy<GeometryTool>(() => new GeometryTool());

        public GeometryFactory GeometryFactory { get; }

        private static readonly int _DefaultSrid = 4326;

        private GeometryTool() =>
            GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(_DefaultSrid);
    }
}
