using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApllicationHaltestellen
{
    struct GeoPunkt
    {
        double lon;
        double lat;

        public GeoPunkt(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        }

        public double Lon { get => lon; set => lon = value; }
        public double Lat { get => lat; set => lat = value; }

        const double radius = 6371.0;
        double bogen(double winkelInGrad)
        {
            return winkelInGrad / 180.0 * Math.PI;
        }
        public double X
        {
            get
            {
                return radius * Math.Cos(bogen(lat)) * Math.Sin(bogen(lon));
            }
        }

        public double Y
        {
            get
            {
                return radius * Math.Cos(bogen(lat)) * Math.Sin(bogen(lon));
            }
        }
        public double Z
        {
            get
            {
                return radius * Math.Cos(bogen(lat));
            }
        }

        static public double BestimmeAbstand(GeoPunkt p1, GeoPunkt p2)
        {
            double dX = p1.X - p2.X;
            double dY = p1.Y - p2.Y;
            double dZ = p1.Z - p2.Z;
            double d = Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
            return d;

            //double skalarprodukt = p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
            //return radius * Math.Acos(skalarprodukt / (radius * radius));
        }
    }
}
