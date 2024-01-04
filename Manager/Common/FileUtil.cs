using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Manager.Common
{
    public class FileUtil
    {
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
                System.UInt32 pBC,
                [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
                [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
                System.UInt32 cbSize,
                [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
                System.UInt32 dwMimeFlags,
                out System.UInt32 ppwzMimeOut,
                System.UInt32 dwReserverd
            );
        public static string getMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
                //throw new FileNotFoundException(filename + " not found");
                return filename + " not found";

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                return "unknown/unknown";
            }
        }

        public static string getMimeFromUploadFile(HttpPostedFileBase uploadFile)
        {
            BinaryReader br = new BinaryReader(uploadFile.InputStream);
            byte[] binData = br.ReadBytes(256);           
            
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, binData, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                return "unknown/unknown";
            }
        }

        public static bool validFileMimeType(HttpPostedFileBase uploadFile, string mimeType)
        {
            List<string> typeList = new List<string>();
            List<string> extentionList = new List<string>();
            switch(mimeType)
            {
                case "images":
                    typeList.Add("image/jpeg");
                    typeList.Add("image/pjpeg");
                    typeList.Add("image/gif");
                    typeList.Add("image/png");
                    typeList.Add("image/x-png");
                    typeList.Add("image/bmp");
                    typeList.Add("image/x-windows-bmp");

                    extentionList.Add("jpeg");
                    extentionList.Add("jpg");
                    extentionList.Add("gif");
                    extentionList.Add("png");
                    extentionList.Add("bmp");
                    break;
                case "xls":
                    typeList.Add("application/octet-stream");
                    typeList.Add("application/x-zip-compressed");

                    extentionList.Add("xls");
                    extentionList.Add("xlsx");
                    break;
                default:
                    typeList.Add("image/jpeg");
                    typeList.Add("image/pjpeg");
                    typeList.Add("image/gif");
                    typeList.Add("image/png");
                    typeList.Add("image/x-png");
                    typeList.Add("image/bmp");
                    typeList.Add("image/x-windows-bmp");
                    typeList.Add("application/octet-stream");
                    typeList.Add("application/x-zip-compressed");
                    typeList.Add("application/pdf");

                    extentionList.Add("jpeg");
                    extentionList.Add("jpg");
                    extentionList.Add("gif");
                    extentionList.Add("png");
                    extentionList.Add("bmp");
                    extentionList.Add("xls");
                    extentionList.Add("xlsx");
                    extentionList.Add("ppt");
                    extentionList.Add("pptx");
                    extentionList.Add("pdf");
                    extentionList.Add("zip");
                    break;
            }

            string mime = getMimeFromUploadFile(uploadFile);
            string ext = uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf(".")).ToLower().Replace(".", "");

            return typeList.Contains(mime) && extentionList.Contains(ext);
        }

        public static string uploadFile(HttpPostedFileBase uploadFile, string subDir) 
        { 
            if (uploadFile == null)
            {
                return null;
            }

            string saveFileName = generateSaveFileName(uploadFile.FileName);
            string today = DateTime.Now.ToString("yyyyMMdd");
            string uploadPath;
            if (subDir.IsEmpty())
            {
                uploadPath = SetInfo.fileUploadroot + "/" + today;
            }
            else
            {
                uploadPath = SetInfo.fileUploadroot + "/" + subDir + "/" + today;
            }
            makeDir(uploadPath);

            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(uploadPath + "/" + saveFileName));

            return uploadPath + "/" + saveFileName;
        }

        public static string generateSaveFileName(string fileName)
        {
            string uuid = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            string ext = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
            return uuid + ext;
        }
                
        public static void makeDir(string path)
        {
            path = HttpContext.Current.Server.MapPath(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        public static bool deleteFile(string fileName)
        {
            fileName = HttpContext.Current.Server.MapPath(fileName);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                return true;
            }
            return false;
        }

        public static bool FileExsists(string fileName)
        {
            fileName = HttpContext.Current.Server.MapPath(fileName);
            return File.Exists(fileName);
        }

        public static string createThumbnail(string fileName, int width, int height)
        {
            string path = fileName.Substring(0, fileName.LastIndexOf("/"));
            string orginFileName = fileName.Substring(fileName.LastIndexOf("/") + 1);
            string thumbnailPath = path + "/thumb";
            string thumbnailPathName = thumbnailPath + "/" + generateSaveFileName(orginFileName);
            makeDir(thumbnailPath);

            System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(fileName));
            System.Drawing.Image thumbnail = new Bitmap(width, height);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            int originWidth = image.Width;
            int originHeight = image.Height;

            double ratioX = (double)width / originWidth;
            double ratioY = (double)height / originHeight;
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            int newWidth = Convert.ToInt32(originWidth * ratio);
            int newHeight = Convert.ToInt32(originHeight * ratio);

            int posX = Convert.ToInt32((width - (originWidth * ratio)) / 2);
            int posY = Convert.ToInt32((height - (originHeight * ratio)) / 2);

            graphic.Clear(Color.White);
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);

            System.Drawing.Imaging.ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            EncoderParameters encoderParameters;
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            thumbnail.Save(HttpContext.Current.Server.MapPath(thumbnailPathName), info[1], encoderParameters);

            image.Dispose();
            thumbnail.Dispose();
            graphic.Dispose();

            return thumbnailPathName;
        }
    }
}