using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MathServerConnector.Interfaces;
using MathServerConnector.Models;
using Newtonsoft.Json;

namespace MathServerConnector
{
    public class ImageDataUploader : IImageDataUploader
    {
        private readonly IMathWebClient _client;
        private const int Chance = 10;
        private readonly Random _random;

        public ImageDataUploader(IMathWebClient client)
        {
            _client = client;
            _random = new Random();
        }

        private void UploadImage(IEnumerable<byte> pixels, string predictionName)
        {
            try
            {
                var uploadData = new ImageUploadData
                {
                    Pixels = pixels.Select(p => (int) p),
                    SegmentName = predictionName
                };
                var stringData = JsonConvert.SerializeObject(uploadData);
                _client.PostData("/api/images", stringData);
            }
            catch (Exception)
            {
            }
        }

        public void UploadImageData(byte[] pixels, string predictionName)
        {
            var rnd = _random.Next(100);
            if (rnd > Chance)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(o => UploadImage(pixels, predictionName));
        }
    }
}