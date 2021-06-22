using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using core.Tools;

namespace core.Extensions
{
    public static class GeometryExtension
    {
        public static Geometry WktToGeometry(this string _wkt)
        {
            return new WKTReader(GeometryTool.Instance.GeometryFactory).Read(_wkt);
        }

        public static string ToWKt(this Geometry _geometry)
        {
            return new WKTWriter().Write(_geometry);
        }

        public static string ToLineWKt(this List<List<double>> _coordinates)
        {
            var _result = new List<Coordinate>();

            _coordinates.ForEach(x =>
            {
                _result.Add(new Coordinate(x.ElementAt(0), x.ElementAt(1)));
            });

            return WKTWriter.ToLineString(_result.ToArray());
        }

        public static Geometry WkbToGeometry(this string _wkb)
        {
            return new WKBReader().Read(Encoding.UTF8.GetBytes(_wkb));
        }

        public static string ToWKb(this Geometry _geometry)
        {
            return Encoding.UTF8.GetString(new WKBWriter().Write(_geometry));
        }
    }
}
