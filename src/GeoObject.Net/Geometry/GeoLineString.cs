﻿//  Author: Weiqing Chen <kevincwq@gmail.com>
//
//  Copyright (c) 2015 Weiqing Chen
//
//  Adapted from GeoJSON.Net https://github.com/jbattermann/GeoJSON.Net
//  Copyright © 2014 Jörg Battermann & Other Contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GeoObject.Net.Geometry
{
    /// <summary>
    /// Defines the <see cref="!:http://geojson.org/geojson-spec.html#linestring">LineString</see> type.
    /// </summary>
    [DataContract]
    public class GeoLineString : GeoObject
    {
        /// <summary>
        /// Gets the Positions.
        /// </summary>
        /// <value>The Positions.</value>
        //[JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        //[JsonConverter(typeof(LineStringConverter))]
        [IgnoreDataMember]
        public List<IGeoEntity> Entities { get; private set; }

        /// <summary>
        /// Gets the coordinates
        /// </summary>
        [DataMember(Name = "coordinates", IsRequired = true)]
        public List<List<double>> Coordinates
        {
            get
            {
                List<List<double>> coordinates = new List<List<double>>();
                foreach (var entity in Entities)
                {
                    coordinates.Add(entity.GetCoordinates());
                }
                return coordinates;
            }

            set
            {
                if (Entities == null)
                    Entities = new List<IGeoEntity>(value.Count);
                foreach (var coords in value)
                {
                    this.Entities.Add(new GeoEntity(coords));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLineString"/> class.
        /// </summary>
        internal GeoLineString(List<List<double>> coordinates)
        {
            this.Coordinates = coordinates;
            this.Type = GeoObjectType.LineString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        public GeoLineString(IEnumerable<IGeoEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            if (entities.Count() < 2)
            {
                throw new ArgumentOutOfRangeException("entities", "According to the GeoJSON v1.0 spec a LineString must have at least two or more positions.");
            }

            this.Entities = new List<IGeoEntity>(entities);
            this.Type = GeoObjectType.LineString;
        }

        public static bool operator !=(GeoLineString left, GeoLineString right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(GeoLineString left, GeoLineString right)
        {
            return Equals(left, right);
        }

        protected bool Equals(GeoLineString other)
        {
            return base.Equals(other) && Entities.SequenceEqual(other.Entities);
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
            return Entities.GetHashCode();
        }

        /// <summary>
        /// Determines whether this instance has its first and last coordinate at the same position and thereby is closed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClosed()
        {
            return this.Entities[0].Equals(this.Entities.Last());
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
            return Entities.Count >= 4 && IsClosed();
        }

        protected override Envelope ComputeBoxInternal()
        {
            if (Entities == null)
                return null;

            var env = new Envelope();
            foreach (var e in Entities)
            {
                env.ExpandToInclude(e);
            }
            return env;
        }
    }
}