﻿//  Author: Weiqing Chen <kevincwq@gmail.com>
//
//  Copyright (c) 2015 Weiqing Chen
//
//  Adapted from GeoJSON.Net https://github.com/jbattermann/GeoJSON.Net
//  Copyright © 2014 Jörg Battermann & Other Contributors

using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    /// Defines the <see cref="!:http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GeoLineString : GeoJSONObject, IGeoObject
    {
        /// <summary>
        /// Gets the Positions.
        /// </summary>
        /// <value>The Positions.</value>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(LineStringConverter))]
        public List<IGeoEntity> Coordinates { get; set; }

        protected bool Equals(GeoLineString other)
        {
            return base.Equals(other) && Coordinates.SequenceEqual(other.Coordinates);
        }

        [JsonConstructor]
        protected internal GeoLineString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLineString" /> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public GeoLineString(IEnumerable<IGeoEntity> coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException("coordinates");
            }

            var coordsList = coordinates.ToList();

            if (coordsList.Count < 2)
            {
                throw new ArgumentOutOfRangeException(
                    "coordinates",
                    "According to the GeoJSON v1.0 spec a LineString must have at least two or more positions.");
            }

            Coordinates = coordsList;
            Type = GeoJSONObjectType.LineString;
        }

        public static bool operator !=(GeoLineString left, GeoLineString right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(GeoLineString left, GeoLineString right)
        {
            return Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GeoLineString)obj);
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance has its first and last coordinate at the same position and thereby is closed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClosed()
        {
            var firstCoordinate = Coordinates[0] as IGeoEntity;

            if (firstCoordinate != null)
            {
                var lastCoordinate = Coordinates[Coordinates.Count - 1] as IGeoEntity;

                return firstCoordinate.Y == lastCoordinate.Y
                       && firstCoordinate.X == lastCoordinate.X
                       && firstCoordinate.Z == lastCoordinate.Z;
            }

            return Coordinates[0].Equals(Coordinates[Coordinates.Count - 1]);
        }

        /// <summary>
        /// Determines whether this LineString is a
        /// <see cref="!:http://geojson.org/geojson-spec.html#linestring">LinearRing</see>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if it is a linear ring; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLinearRing()
        {
            return Coordinates.Count >= 4 && IsClosed();
        }
    }
}