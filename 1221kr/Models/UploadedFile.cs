using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1221kr.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}