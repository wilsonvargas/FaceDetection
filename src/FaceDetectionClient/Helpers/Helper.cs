using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceDetectionClient.Helpers
{
    public static class Helper
    {
        public static string GetFaceAttributes(IList<DetectedFace> faceList)
        {
            string attributes = string.Empty;

            foreach (DetectedFace face in faceList)
            {                
                double? age = face.FaceAttributes.Age;
                string gender = face.FaceAttributes.ToString();
                attributes += gender + " " + age + " - ";
            }

            return attributes;
        }
    }
}