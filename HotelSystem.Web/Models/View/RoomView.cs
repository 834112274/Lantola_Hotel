using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelSystem.Web.Models.View
{
    public class RoomView
    {
        public Room room;
        public PriceView priceView;
        public List<PriceView> PriceViews;
        public RoomView()
        {
            this.PriceViews = new List<View.PriceView>();
        }
        
    }
}