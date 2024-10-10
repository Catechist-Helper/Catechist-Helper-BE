using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatechistHelper.Infrastructure.Utils
{
    public class ImageUtil
    {
        public static class FormFileHelper
        {
            public static IFormFile ToFormFile(byte[] fileContents, string fileName, string contentType)
            {
                var stream = new MemoryStream(fileContents);
                return new FormFile(stream, 0, stream.Length, null, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
            }
        }
    }
}
