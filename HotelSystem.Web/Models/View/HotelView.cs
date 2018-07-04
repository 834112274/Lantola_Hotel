using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Models.View
{
    public class HotelView
    {
        public HotelInfo hotelInfo;
        public double minPrice;
        public double maxPrice;

        /// <summary>
        /// 省份
        /// </summary>
        public string provice { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string county { get; set; }

        /// <summary>
        /// 酒店设施
        /// </summary>
        public string facilities { get; set; }

        /// <summary>
        /// 搜索关键字（地址OR名称）
        /// </summary>
        public string k { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime start { get; set; }

        /// <summary>
        /// 退房日期
        /// </summary>
        public DateTime end { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        public int star { get; set; }

        /// <summary>
        /// 页
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize { get; set; }

        public HotelView()
        {
            pageIndex = 1;
            pageIndex = 10;
            start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            end = start.AddDays(1);
            hotelInfo = new HotelInfo();
        }

        public static List<RoomView> GetRooms(string id, DateTime start, DateTime end)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                int d = (end - start).Days;
                var rooms = from r in DbContext.Room.Include("RoomImages") where r.HotelInfoId == id select r;
                List<RoomView> roomViews = new List<RoomView>();
                foreach (var r in rooms)
                {
                    var price = from p in (from t in DbContext.Price
                                           where t.PriceType.Room.HotelInfoId == id && t.Date >= start && t.Date < end && t.Status == 1
                                           group t by t.PriceTypeId into g
                                           select new { g.Key, days = g.Count(), avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice) })
                                join t in DbContext.PriceType on p.Key equals t.Id
                                where p.days >= d && t.RoomId == r.Id
                                select new PriceView { priceType = t, avg = p.avg, days = p.days, sum = p.sum };
                    if (price.Count() > 0)
                    {
                        RoomView rw = new RoomView()
                        {
                            room = r,
                            PriceViews = price.ToList()
                        };
                        roomViews.Add(rw);
                    }
                }
                if (roomViews.Count() > 0)
                {
                    return roomViews;
                }
                return new List<RoomView>();
            }
        }

        public static RoomView Single(string id, string priceTypeId, int roomCount, DateTime start, DateTime end, out int days)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                int d = days = (end - start).Days;
                var Rooms = from p in (from t in DbContext.Price
                                       where t.PriceType.Room.HotelInfoId == id && t.Date >= start && t.Date < end && t.Status == 1
                                       group t by t.PriceTypeId into g
                                       select new { g.Key, days = g.Count(), avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice) })
                            join t in DbContext.PriceType on p.Key equals t.Id
                            where p.days >= d && t.Id == priceTypeId
                            select new RoomView
                            {
                                room = t.Room,
                                priceView = new PriceView
                                {
                                    priceType = t,
                                    price = t.Price.Where(m => m.Date >= start && m.Date < end).OrderBy(m => m.Date).ToList(),
                                    avg = p.avg,
                                    days = p.days,
                                    sum = p.sum * roomCount
                                }
                            };
                if (Rooms.Count() > 0)
                {
                    return Rooms.First();
                }
                return null;
            }
        }

        public List<HotelView> Search()
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var hotel = from m in DbContext.HotelInfo
                            select m;
                //搜索关键字
                if (!string.IsNullOrEmpty(k))
                {
                    hotel = from m in hotel
                            where m.Adress.Contains(k) || m.Name.Contains(k)
                            select m;
                }
                //省份
                if (!string.IsNullOrEmpty(provice))
                {
                    hotel = from m in hotel
                            where m.City.Province.ProvinceName.Contains(provice)
                            select m;
                }
                //城市
                if (!string.IsNullOrEmpty(city))
                {
                    hotel = from m in hotel
                            where m.City.CityName.Contains(city)
                            select m;
                }
                //区县
                if (!string.IsNullOrEmpty(county))
                {
                    hotel = from m in hotel
                            where m.District.DistrictName.Contains(county)
                            select m;
                }
                //星级
                if (star > 0)
                {
                    hotel = from m in hotel
                            where m.Star == star
                            select m;
                }
                //设施
                if (!string.IsNullOrEmpty(facilities))
                {
                    hotel = from m in hotel
                            where m.HotelPolicy.Where(t => facilities.Contains(t.Value)).Count() > 0
                            select m;
                }
                //入住退房日期
                if (start > DateTime.Now.AddDays(-1))
                {
                    var d = (end - start).Days;
                    var price = from t in DbContext.Price
                                where t.Date >= start && t.Date < end && t.Status == 1
                                group t by t.PriceTypeId into g
                                select new { g.Key, days = g.Count(), avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice), min = g.Min(t => t.UnitPrice), max = g.Max(t => t.UnitPrice) };
                    var priceType = from p in price
                                    join t in DbContext.PriceType on p.Key equals t.Id
                                    where p.days >= d
                                    select new { id = t.Room.HotelInfoId, min = p.min, max = p.max };
                    var room = from m in priceType
                               group m by m.id into g
                               select new { id = g.Key, min = g.Min(t => t.min), max = g.Max(t => t.max) };

                    var hv = from r in room
                             join m in hotel on r.id equals m.Id
                             select new HotelView()
                             {
                                 hotelInfo = m,
                                 maxPrice = r.max,
                                 minPrice = r.min
                             };

                    return hv.OrderByDescending(m => m.minPrice).ToPagedList(pageIndex, 15);
                }
                else
                {
                    return new List<HotelView>();
                }
            }
        }
    }
}