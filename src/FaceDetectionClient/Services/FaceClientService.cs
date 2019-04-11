using FaceDetectionClient.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FaceDetectionClient.Services
{
    public class FaceClientService
    {
        private static readonly FaceClientService instance = new FaceClientService();
        private FaceClient client;
        public static FaceClientService Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<IList<DetectedFace>> GetDetectFaceWitUrl(string imageUrl)
        {
            client = GetClient();
            IList<DetectedFace> faceList = await client.Face.DetectWithUrlAsync(imageUrl, true, false, Settings.FaceAttributes);            
            return faceList;
        }

        public async Task<IList<DetectedFace>> GetDetectFaceWitLocalImage(Stream image)
        {
            client = GetClient();
            IList<DetectedFace> faceList = await client.Face.DetectWithStreamAsync(image, true, false, Settings.FaceAttributes);
            return faceList;
        }

        private FaceClient GetClient()
        {
            if (client == null)
            {
                client = new FaceClient(new ApiKeyServiceClientCredentials(Settings.SubscriptionKey), new System.Net.Http.DelegatingHandler[] { });
                client.Endpoint = Settings.Endpoint;
            }

            return client;
        }
    }
}