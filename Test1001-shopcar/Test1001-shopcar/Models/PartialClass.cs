﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test1001_shopcar.Models
{
    public class PatialClass
    {
    }

    //定義Models.Order的部分類別
    public partial class Order
    {
        //取得訂單中的 使用者暱稱
        public string GetUrderName()
        {
            //使用Order類別中的UserId到AspNetUsers資料表中搜尋出UserName
            using (Models.CartsEntities db = new  CartsEntities ())
            {
                var result = (from s in db.AspNetUsers
                              where s.Id == this.Id
                              select s.Username).FirstOrDefault();

                //回傳找到的UserName
                return result;
            }
        }

    }
}