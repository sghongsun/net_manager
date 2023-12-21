using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Manager.Common
{
    public class MessageConfig
    {   
        public static ContentResult AlertMessage(string strAlert, string Location)
        {
            ContentResult result = new ContentResult();

            if (Location.Equals("ajax"))
            {
                result.Content = "FAIL|||||" + strAlert;
            }
            else
            {
                if (strAlert.IsEmpty())
                {
                    result.Content = "<script>" + Location + "</script>";
                }
                else
                {
                    result.Content = "<script>alert('" + strAlert + "');" + Location + "</script>";
                }
            }            

            return result;
        }
    }
}