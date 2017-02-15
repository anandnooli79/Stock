using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class DataController : Controller
    {
        [HttpPost]
        public JsonResult SaveFiles(string description)
        {
            string Message, fileName, actualFilename;
            Message = fileName = actualFilename = string.Empty;
            bool flag = false;
            if(Request.Files!=null)
            {
                var file = Request.Files[0];
                actualFilename = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;
                try
                {
                    file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), fileName));
                    flag = true;
                }
                catch(Exception)
                {
                    Message = "File Upload faile! Please try again";
                }
            }
            else
            {
                Message = "No files to upload!";
            }
            return new JsonResult { Data = new { Message = Message, Status = flag, FileName = fileName } };
        }

        [HttpPost]
        public async Task<JsonResult> SaveFilesNew(string FileDescription,string FilePath)
        {
            string Message, fileName, actualFilename;
            Message = fileName = actualFilename = string.Empty;
            bool flag = false;
            PicturesDb db = new PicturesDb();
            db.Pictures.Add(new Entities.Picture() { FileDescription = FileDescription, FilePath = FilePath });
            await db.SaveChangesAsync();
            return await Task.FromResult<JsonResult>(new JsonResult { Data = new { Message = Message, Status = flag } });
        }

        [HttpPost]
        public async Task<JsonResult> UploadStock(string skuId, int discountPrecent, string stockDescription ,
            string stockImage, bool isInStock, string stockName, double stockPrice)
        {
            string Message, fileName, actualFilename;
            Message = fileName = actualFilename = string.Empty;
            bool flag = false;
            PicturesDb db = new PicturesDb();
            db.AvailableStock.Add(new Entities.Stock() {
                SKUId = skuId,
                DiscountPercent = discountPrecent,
                StockDescription = stockDescription,
                StockImage = stockImage,
                IsInStock = isInStock,
                StockName = stockName, StockPrice= stockPrice
            });

            await db.SaveChangesAsync();
            Message = "success";
            flag = true;
            return await Task.FromResult<JsonResult>(new JsonResult { Data = new { Message = Message, Status = flag } });
        }

        [HttpGet]
        public async Task<JsonResult> GetFiles()
        {
            string Message, fileName, actualFilename;
            Message = fileName = actualFilename = string.Empty;
            PicturesDb db = new PicturesDb();
            var pictures =db.AvailableStock.ToList();
            List<UploadedFile> uploadedFiles = new List<UploadedFile>();
            foreach(var picture in pictures)
            {
                uploadedFiles.Add(new UploadedFile() { FileDescription = picture.StockDescription, FilePath = picture.StockImage });
            }

            return await Task.FromResult<JsonResult>(new JsonResult { Data = new { UploadedFiles = uploadedFiles, Message = Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue });
        }

        public class UploadedFile
        {
            public string FileDescription { get; set; }
            public string FilePath { get; set; }
        }

        public class UploadedStock
        {
            public string SKUId { get; set; }

            public string StockName { get; set; }

            public string StockDescription { get; set; }

            public string StockImage { get; set; }

            public double StockPrice { get; set; }

            public int DiscountPercent { get; set; }

            public bool IsInStock { get; set; }
        }
    }
}