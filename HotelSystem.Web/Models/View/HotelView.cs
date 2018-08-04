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
        public double score;
        public int commentCount;
        public List<Policy> policy;

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
        public List<string> facilities { get; set; }

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
        /// <summary>
        /// 评分排序
        /// </summary>
        public string sortScore { get; set; }
        /// <summary>
        /// 价格排序
        /// </summary>
        public string sortPrice { get; set; }
        /// <summary>
        /// 热门排序
        /// </summary>
        public string sortHot { get; set; }

        public HotelView()
        {
            pageIndex = 1;
            pageSize = 15;
            start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            end = start.AddDays(1);
            hotelInfo = new HotelInfo();
            city = "上海";
        }

        public static List<RoomView> GetRooms(string id, DateTime start, DateTime end)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                int d = (end - start).Days;
                var stocks = from m in DbContext.Stock where m.Date >= start && m.Date < end && m.Room.HotelInfoId == id select new { m.RoomId, m.Date, SurplusStock = m.SurplusStock > 0 ? 1 : 0 };
                var rooms = from r in DbContext.Room.Include("RoomImages") where r.HotelInfoId == id select r;
                List<RoomView> roomViews = new List<RoomView>();
                foreach (var r in rooms)
                {
                    var priceList = from t in DbContext.Price
                                    join m in stocks on new { t.PriceType.RoomId, t.Date } equals new { m.RoomId, m.Date }
                                    where t.PriceType.Room.HotelInfoId == id && t.Date >= start && t.Date < end && t.Status == 1
                                    select new { t.PriceTypeId, t.UnitPrice, m.SurplusStock };
                    var price = from p in (from t in priceList
                                           group t by t.PriceTypeId into g
                                           where g.Count() >= d
                                           select new { g.Key, avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice), enough = g.Sum(m => m.SurplusStock) })
                                join t in DbContext.PriceType on p.Key equals t.Id
                                where t.RoomId == r.Id
                                select new PriceView { priceType = t, avg = p.avg, days = d, sum = p.sum, enough = p.enough >= d };
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
                //检查库存
                var stocks = from m in DbContext.Stock where m.SurplusStock > roomCount&&  m.Date >= start && m.Date < end && m.Room.HotelInfoId == id select m;
                if (stocks.Count() < d)
                {
                    return null;
                }
                var Rooms = from p in (from t in DbContext.Price
                                       where t.PriceType.Room.HotelInfoId == id && t.Date >= start && t.Date < end && t.Status == 1
                                       group t by t.PriceTypeId into g
                                       where g.Count() >= d
                                       select new { g.Key, avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice) })
                            join t in DbContext.PriceType on p.Key equals t.Id
                            where t.Id == priceTypeId
                            select new RoomView
                            { 
                                room = t.Room,
                                priceView = new PriceView
                                {
                                    priceType = t,
                                    price = t.Price.Where(m => m.Date >= start && m.Date < end).OrderBy(m => m.Date).ToList(),
                                    avg = p.avg,
                                    days = d,
                                    sum = p.sum * roomCount,enough=true
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
                var hotel = from m in DbContext.HotelInfo.Include("HotelImages").Include("City").Include("District").Include("HotelPolicy")
                            select m;
                //搜索关键字
                if (!string.IsNullOrEmpty(k))
                {
                    hotel = from m in hotel
                            where m.Adress.Contains(k) || m.Name.Contains(k)
                            select m;
                }
                //省份
                //if (!string.IsNullOrEmpty(provice))
                //{
                //    hotel = from m in hotel
                //            where m.City.Province.ProvinceName.Contains(provice)
                //            select m;
                //}
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
                if (facilities!=null&& facilities.Count>0)
                {
                   
                    var ids = from m in DbContext.Policy where facilities.Contains(m.Name) select m.Id;
                    var hotels = (from m in DbContext.HotelPolicy where ids.Contains(m.PolicyId) select m.HotelInfoId).Distinct();
                    hotel = from m in hotel
                            where hotels.Contains(m.Id)
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

                    var hotelIds = from m in hotel select m.Id;
                    var policy = (from m in DbContext.HotelPolicy where hotelIds.Contains(m.HotelInfoId) select new { id=m.HotelInfoId, m.Policy } ).ToList();
                    
                    var hv = from m in hotel.ToList()
                             join r in room.ToList() on m.Id equals r.id
                             select new HotelView()
                             {
                                 hotelInfo = m,
                                 maxPrice = r.max,
                                 minPrice = r.min,
                                 policy= policy.Where(p => p.id == m.Id).Select(p=> p.Policy).ToList(),
                             };
                    //得分
                    var scores = (from m in DbContext.Score group m by m.HotelInfoId into g select new { g.Key, score = g.Average(m => m.Value) }).ToList();
                    foreach(var s in scores)
                    {
                        var h= hv.Where(m => m.hotelInfo.Id == s.Key);
                        if (h.Count() > 0)
                        {
                            h.First().score = s.score;
                        }
                    }
                    //评论数
                    var comments= (from m in DbContext.Comment group m by m.Order.HotelInfoId into g select new { g.Key, value = g.Count() }).ToList();
                    foreach (var s in comments)
                    {
                        var h = hv.Where(m => m.hotelInfo.Id == s.Key);
                        if (h.Count() > 0)
                        {
                            h.First().commentCount = s.value;
                        }
                    }
                    
                    bool isSort = false;
                    if (!string.IsNullOrEmpty( sortPrice))
                    {
                        hv = hv.OrderBy(m => m.minPrice);
                        isSort = true;
                    }
                    if (!string.IsNullOrEmpty(sortScore))
                    {
                        hv = hv.OrderByDescending(m => m.score);
                        isSort = true;
                    }
                    if (!isSort)
                    {
                        hv = hv.OrderByDescending(m => m.commentCount);
                    }
                    return hv.ToPagedList(pageIndex, pageSize);
                }
                else
                {
                    return new List<HotelView>();
                }
            }
        }
    }
}