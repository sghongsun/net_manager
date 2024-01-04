using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Manager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            foreach (string key in Request.QueryString)
                CheckInput(Request.QueryString[key], key);
            foreach (string key in Request.Form)
                CheckInput(Request.Form[key], key);
            foreach (string key in Request.Cookies)
                CheckInput(Request.Cookies[key].Value, key);
        }

        //���͸� �� ���ڿ�
        public static string[] blackList = {"--",";--",">;","/*","*/","@@",
              "declare","delete","update","insert", "select",
              "exec", "execute",
              "drop","kill","sysobjects","syscolumns"};
        
        //���͸� ������ ������
        //public static string[] nochkPageList = { "" };

        //���͸� ������ �Ķ���� KEY��
        //public static string[] nochkParamList = {  };

        private void CheckInput(string parameterValue, string parameterName)
        {
            CompareInfo comparer = CultureInfo.InvariantCulture.CompareInfo;

            bool chk = true;
            /*
            for (int j = 0; j < nochkParamList.Length; j++)
            {
                if (parameterName.ToLower() == nochkParamList[j].ToLower())
                {
                    chk = false;
                }
            }

            for (int k = 0; k < nochkPageList.Length; k++)
            {
                if (comparer.IndexOf(Request.RawUrl.ToString(), nochkPageList[k], CompareOptions.IgnoreCase) >= 0)
                {
                    chk = false;
                }
            }
            */

            for (int i = 0; i < blackList.Length; i++)
            {
                if (comparer.IndexOf(parameterValue, blackList[i], CompareOptions.IgnoreCase) >= 0 && chk == true)
                {
                    try
                    {
                        //Log�����
                    }
                    catch { }
                    finally
                    {
                        Response.Redirect("/error");
                    }
                }
            }
        }

    }
}
