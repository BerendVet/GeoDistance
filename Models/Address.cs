using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoDistance.Models
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }


        public static double CalculateDistance(double latFrom, double lngFrom, double latTo, double lngTo)
        {
            var d1 = latFrom * (Math.PI / 180.0);
            var num1 = lngFrom * (Math.PI / 180.0);
            var d2 = latTo * (Math.PI / 180.0);
            var num2 = lngTo * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}