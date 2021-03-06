﻿using GeoObject.Net.Geometry;
using NUnit.Framework;

namespace GeoObject.Net.Tests.Geometry
{
    [TestFixture]
    public class WktTests : TestBase
    {
        const string Wkt_Point = "POINT (30 10)";
        const string Wkt_LineString = "LINESTRING (30 10, 10 30, 40 40)";
        const string Wkt_Polygon1 = "POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))";
        const string Wkt_Polygon2 = "POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))";
        const string Wkt_MultiPoint1 = "MULTIPOINT ((10 40), (40 30), (20 20), (30 10))";
        const string Wkt_MultiPoint2 = "MULTIPOINT (10 40, 40 30, 20 20, 30 10)";
        const string Wkt_MultiLineString = "MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))";
        const string Wkt_MultiPolygon1 = "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";
        const string Wkt_MultiPolygon2 = "MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))";
        static readonly string Wkt_GeometryCollection = "GEOMETRYCOLLECTION(" + string.Join(",", Wkt_Point, Wkt_LineString, Wkt_Polygon1, Wkt_Polygon2, Wkt_MultiPoint1, Wkt_MultiPoint2, Wkt_MultiLineString, Wkt_MultiPolygon1, Wkt_MultiPolygon2) + ")";

        [Test]
        public void From_Wkt_Point()
        {
            var geo = Wkt_Point.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.Point);
            var coords = (geo as GeoPoint).Entity;
            Assert.AreEqual(coords.X, 30);
            Assert.AreEqual(coords.Y, 10);
            Assert.IsFalse(coords.Z.HasValue);
        }

        [Test]
        public void From_Wkt_LineString()
        {
            var geo = Wkt_LineString.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.LineString);
            Assert.AreEqual((geo as GeoLineString).Entities.Count, 3);
        }

        [Test]
        public void From_Wkt_Polygon()
        {
            var geo = Wkt_Polygon1.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.Polygon);
            Assert.AreEqual((geo as GeoPolygon).LineStrings.Count, 1);

            geo = Wkt_Polygon2.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.Polygon);
            Assert.AreEqual((geo as GeoPolygon).LineStrings.Count, 2);
        }

        [Test]
        public void From_Wkt_MultiPoint()
        {
            var geo = Wkt_MultiPoint1.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.MultiPoint);
            Assert.AreEqual((geo as GeoMultiPoint).Points.Count, 4);

            geo = Wkt_MultiPoint2.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.MultiPoint);
            Assert.AreEqual((geo as GeoMultiPoint).Points.Count, 4);
        }

        [Test]
        public void From_Wkt_MultiLineString()
        {
            var geo = Wkt_MultiLineString.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.MultiLineString);
            Assert.AreEqual((geo as GeoMultiLineString).LineStrings.Count, 2);
        }

        [Test]
        public void From_Wkt_MultiPolygon()
        {
            var geo = Wkt_MultiPolygon1.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.MultiPolygon);
            Assert.AreEqual((geo as GeoMultiPolygon).Polygons.Count, 2);

            geo = Wkt_MultiPolygon2.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.MultiPolygon);
            Assert.AreEqual((geo as GeoMultiPolygon).Polygons.Count, 2);
            Assert.AreEqual((geo as GeoMultiPolygon).Polygons[1].LineStrings.Count, 2);
        }

        [Test]
        public void From_Wkt_GeometryCollection()
        {
            var geo = Wkt_GeometryCollection.FromWkt();
            Assert.AreEqual(geo.Type, GeoObjectType.GeometryCollection);
            Assert.AreEqual((geo as GeoCollection).Geometries.Count, 9);
        }

        [Test]
        public void To_Wkt()
        {
            foreach (var geom in Geometries)
            {
                var wkt = geom.ToWkt();
                Assert.IsNotNullOrEmpty(wkt);
                var ng = wkt.FromWkt();
                Assert.AreEqual(geom.Type, ng.Type);
            }
        }
    }
}
