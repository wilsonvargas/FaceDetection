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
                FaceAttributes faceAttributes = face.FaceAttributes;
                double? age = faceAttributes.Age;
                string gender = faceAttributes.Gender.ToString();
                string smile = (faceAttributes.Smile > 0) ? "is smiling" : "is not smiling";
                attributes += gender + " " + age + "   " + smile + " - ";
            }

            return attributes;
        }
    }
}