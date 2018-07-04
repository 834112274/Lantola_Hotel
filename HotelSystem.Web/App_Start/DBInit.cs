using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelSystem.Web
{
    /// <summary>
    /// 网站数据初始化
    /// </summary>
    public class DBInit
    {
        public static void InitializedData()
        {
            using (DBModelContainer db = new DBModelContainer())
            {
                //初始化一个管理员用户
                var number = from u in db.Users where u.Name=="admin" select u;
                string admin_id = "";
                if (number.Count() == 0)
                {
                    Users user = new Users()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "admin",
                        Password = "e10adc3949ba59abbe56e057f20f883e",
                        Role = "system",
                        RegisterTime = DateTime.Now,
                        RegisterIP = "127.0.0.1",
                        Vail = true,
                        Summary = "系统自动创建超级管理员账号",
                        Status = 1
                    };
                    db.Users.Add(user);
                    admin_id = user.Id;
                }
                else
                {
                    admin_id = number.First().Id;
                }
                //检查更新admin用户权限
                var cur_menus_id = from m in db.UserRole where m.UserType == "system" && m.UsersId == admin_id select m.MenuId;
                var menus = from m in db.Menu where m.Type == "system" select m;
                var new_menus = (from m in menus
                                 where !cur_menus_id.Contains(m.Id)
                                 select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = admin_id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "system" }).ToList();

                db.UserRole.AddRange(new_menus);
                //初始化酒店得分项
                var scoreType = from m in db.ScoreType select m;
                if (scoreType.Count() == 0)
                {
                    List<ScoreType> scores = new List<ScoreType>() {
                        new ScoreType()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "位置",
                            Valid = true
                        },
                        new ScoreType()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "服务及设施",
                            Valid = true
                        },
                        new ScoreType()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "环境和清洁度",
                            Valid = true
                        },
                        new ScoreType()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "客户舒适度",
                            Valid = true
                        }
                    };
                    db.ScoreType.AddRange(scores);
                }

                HotelUserMenuInit(db);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="db"></param>
        private void MenuInit(DBModelContainer db)
        {
            var curMenus = from m in db.Menu select m;
            if (curMenus.Count() == 0)
            {
                //运营后台

                //酒店后台
            }
        }
        /// <summary>
        /// 更新酒店用户菜单，正式移除
        /// </summary>
        private static void HotelUserMenuInit(DBModelContainer db)
        {
            var hotelUsers = (from m in db.HotelUsers where m.ParentId == null || m.ParentId == "" select m).ToList();
            foreach (var huser in hotelUsers) {
                var cur_menus_id = from m in db.UserRole where m.UserType == "hotel" && m.UsersId == huser.Id select m.MenuId;
                var menus = from m in db.Menu where m.Type == "hotel" select m;
                var new_menus = (from m in menus
                                 where !cur_menus_id.Contains(m.Id)
                                 select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = huser.Id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "hotel" }).ToList();

                db.UserRole.AddRange(new_menus);
            }
            
        }
    }
}