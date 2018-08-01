using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelSystem.Web.Models.View
{
    public class PriceView
    {
        public PriceType priceType;
        public List<Price> price;
        public double avg;
        public double sum;
        public int days;
        public bool enough;
    }
}