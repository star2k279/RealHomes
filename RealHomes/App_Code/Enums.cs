using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Controllers
{
    public static class Enums
    {
        public enum DevelopmentLoad
        {
            FreeHold=0,
            LeaseHold=1
        }

        public enum Currency
        {
            AED=1,
            EUR=2,
            GBP=3,
            PKR=4,
            USD=5,
            IRR=6,
            INR=7
        }

        public enum ServiceType
        {
            ForSale = 1,
            ForRent = 2,
            ShortStay = 3
        }

        public enum propertyStatus
        {
            Ready=1,
            InProcess=2
        }
    }
}