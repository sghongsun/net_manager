using Manager.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Manager.Common
{
    public class Func
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


        public static string AESEncrypt(string val)
        {
            string retVal = "";
            AES aes = new AES();
            if (val != null && val != "")
            {
                try
                {
                    retVal = aes.Encrypt(val);
                }
                catch
                {
                    retVal = "";
                }
            }
            return retVal;
        }
        public static string AESDecrypt(string val)
        {
            string retVal = "";
            AES aes = new AES();
            if (val != null && val != "")
            {
                try
                {
                    retVal = aes.Decrypt(val);
                }
                catch
                {
                    retVal = "";
                }
            }
            return retVal;
        }


        public static string Base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Encode: " + e.Message);
            }
        }
        public static string Base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Decode: " + e.Message);
            }
        }

        public static string GetUserIP()
        {
            string retIP = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == "")
            {
                retIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                retIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            return retIP;
        }

        public static void SetCookie(string cookieName, string cookieValue, int cookieExpiresDay = 0)
        {
            AES aes = new AES();
            HttpContext.Current.Response.Cookies[cookieName].Value = aes.Encrypt(HttpContext.Current.Server.UrlEncode(cookieValue));
            if (cookieExpiresDay.ToString() != "0")
            {
                HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(cookieExpiresDay);
            }
        }
                
        public static string GetCookie(string cookieName)
        {
            string cookieValue = "";
            if (HttpContext.Current.Request.Cookies[cookieName] != null && HttpContext.Current.Request.Cookies[cookieName].Value != "")
            {
                AES aes = new AES();
                cookieValue = HttpContext.Current.Server.UrlDecode(aes.Decrypt(HttpContext.Current.Request.Cookies[cookieName].Value));
            }

            return cookieValue;
        }   

        public static void SetSession(string sessionName, string sessionValue)
        {
            AES aes = new AES();
            HttpContext.Current.Session[sessionName] = aes.Encrypt(HttpContext.Current.Server.UrlEncode(sessionValue));
        }

        public static string GetSssion(string sessionName)
        {
            string sessionValue = "";
            if (HttpContext.Current.Session[sessionName] != null && (string)HttpContext.Current.Session[sessionName] != "")
            {
                AES aes = new AES();
                sessionValue = HttpContext.Current.Server.UrlDecode(aes.Decrypt((string)HttpContext.Current.Session[sessionName]));
            }
            return sessionName;
        }

        public static string PagingHtml(Search search)
        {
            string html = "";
            string classon = "";

            if (search.pagination.existPrevPage)
            {
                html += "<a href='javascript:void(0)' onclick='movePage(" + (search.pagination.startPage - search.pagesize) + ");'>◀</a>\r\n";
            }
            else
            {
                html += "<a>◀</a>\r\n";
            }

            for (int i = search.pagination.startPage; i <= search.pagination.endPage; i++)
            {
                classon = "";
                if (i == search.page)
                {
                    classon = " class=on";
                }
                if (i < search.pagination.endPage)
                {
                    html += "<span><a href='javascript:void(0);' onclick='movePage(" + i + ");'" + classon + ">" + i + "</a></span>\r\n";
                }
                else
                {
                    html += "<a href='javascript:void(0);' onclick='movePage(" + i + ");'" + classon + ">" + i + "</a>\r\n";
                }
            }
            if(search.pagination.existNextPage)
            {
                html += "<a href='javascript:void(0)' onclick='movePage(" + (search.pagination.endPage + 1) + ");'>▶</a>\r\n";
            }
            else
            {
                html += "<a>▶</a>\r\n";
            }
            return html;
        }

        public static string GetNow()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public static string getRequestQueryStringToString(string Name)
        {
            string strReturnValue = "";

            if (HttpContext.Current.Request.QueryString[Name] != null)
            {
                strReturnValue = HttpContext.Current.Request.QueryString[Name].ToString();
            }

            return strReturnValue;
        }

        public static int getRequestQueryStringToInt(string Name)
        {
            int intReturnValue = 0;

            if (HttpContext.Current.Request.QueryString[Name] != null)
            {
                try
                {
                    intReturnValue = Convert.ToInt32(HttpContext.Current.Request.QueryString[Name].ToString());
                }
                catch
                {
                    intReturnValue = 0;
                }
            }

            return intReturnValue;
        }

        public static string getRequestFormToString(string Name)
        {
            string strReturnValue = "";

            if (HttpContext.Current.Request.Form[Name] != null)
            {
                strReturnValue = HttpContext.Current.Request.Form[Name].ToString();
            }

            return strReturnValue;
        }

        public static int getRequestFormToInt(string Name)
        {
            int intReturnValue = 0;

            if (HttpContext.Current.Request.Form[Name] != null)
            {
                try
                {
                    intReturnValue = Convert.ToInt32(HttpContext.Current.Request.Form[Name].ToString());
                }
                catch
                {
                    intReturnValue = 0;
                }
            }

            return intReturnValue;
        }

        public static String mimetype(String fileName)
        {
            String ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
            switch (ext)
            {
                case "jpg": return "image/jpeg";
                case "gif": return "image/gif";
                case "png": return "image/png";
                case "bmp": return "image/bmp";
                case "txt": return "text/plain";
                case "pdf": return "application/pdf";
                case "xls": return "application/vnd.ms-excel";
                case "xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default: return "application/zip";
            }
        }

        public static string getPageType(HttpContextBase httpContext, string thisPage)
        {
            if (httpContext.Request.Headers["x-requested-with"] != null && httpContext.Request.Headers["x-requested-with"].Equals("XMLHttpRequest"))
            {
                return "ajax";
            }
            else
            {
                if (thisPage.Contains("/popup"))
                {
                    return "popup";
                }
                else
                {
                    return "general";
                }
            }
        }

        public static string getReturnPageType(HttpContextBase httpContext, string thisPage)
        {
            if (httpContext.Request.Headers["x-requested-with"] != null && httpContext.Request.Headers["x-requested-with"].Equals("XMLHttpRequest"))
            {
                return "ajax";
            }
            else
            {
                if (thisPage.Contains("/popup"))
                {
                    return "window.close();";
                }
                else
                {
                    return "history.back();";
                }
            }
        }

        public static string ArrayToStringForComma(string[] array)
        {
            String result = "";

            if (array.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string s in array)
                {
                    sb.Append(s).Append(",");
                }

                result = sb.Remove(sb.Length - 1, 1).ToString();
            }

            return result;
        }

        public static string Left(string str, int len)
        {
            if (str.Length < len)
            {
                len = str.Length;
            }
            return str.Substring(0, len);
        }

        public static string Right(string str, int len)
        {
            if (str.Length < len)
            {
                len = str.Length;
            }
            return str.Substring(str.Length - len);
        }

        public static string MaketoZero(string str, int len)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len + 1; i++)
            {                
                sb.Append("0");
            }
            sb.Append(str);
            str = Right(sb.ToString(), len);
            return str;
        }

        public static string ArrarytoStringForComma(string[] array)
        {
            String result = "";

            if (array.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string s in array)
                {
                    sb.Append(s).Append(",");
                }

                result = sb.Remove(sb.Length - 1, 1).ToString();
            }

            return result;
        }
    }
}