using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Network {
    internal class Utils: MonoBehaviour {
        public static byte[] GetHash(string inputString) {
            HashAlgorithm algorithm = SHA1.Create(); // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }


        public static string GetHashString(string inputString) {
            var sb = new StringBuilder();
            foreach (var b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string PostRequest(string url, string jsonData) {
            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            var response = cli.UploadString(new Uri(url), "POST", jsonData);

            return response;
        }

        public static string GetRequest(string url) {
            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            var response = cli.DownloadString(new Uri(url));
            return response;
        }
    }
}