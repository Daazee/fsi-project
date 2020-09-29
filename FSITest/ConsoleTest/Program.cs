using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleTest
{
    class Program
    {
        private static String key = "abuMobolaji12345";
        private static String initVector = "encryptionIntVec";

        private static AesManaged CreateAes()
        {
            var aes = new AesManaged();
           // aes.Mode = CipherMode.CBC;
           // aes.Padding = PaddingMode.PKCS7;
            aes.Key = System.Text.Encoding.UTF8.GetBytes(key); //UTF8-Encoding
            aes.IV = System.Text.Encoding.UTF8.GetBytes(initVector);//UT8-Encoding
            return aes;
        }

        public static string encrypt(string text)
        {
            using (AesManaged aes = CreateAes())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(text);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        public static string decrypt(string text)
        {
            using (var aes = CreateAes())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(text)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

            }
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
        static string Base64Encoded(string plain)
        {
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(plain);
            return Convert.ToBase64String(data);
        }
        static string ToHexadecimal(string data)
        {
            char[] values = data.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the integer value to a hexadecimal value in string form.
                sb.Append($"{value:X}");             
            }
            return sb.ToString();
        }
       
        static async Task Main(string[] args)
        {           
            string signatureMethodHeader = "SHA256";
            string password = "Oyinromola2405";
            string username = "11111";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string signatureString = username + date + password;
            string signatureHeader = ComputeSha256Hash(signatureString);
            string authString = username + ":" + password;
            string authHeader = Base64Encoded(authString);
            string contentType = "application/json";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sandboxapi.fsi.ng/nibss/bvnr");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OrganisationCode", "MTExMTE=");
                client.DefaultRequestHeaders.Add("Sandbox-Key", "a4c3ed6df1e9ad30d55de6ec33b60295");
                client.DefaultRequestHeaders.Add("Authorization", authHeader);
                client.DefaultRequestHeaders.Add("SIGNATURE", signatureHeader);
                client.DefaultRequestHeaders.Add("SIGNATURE_METH", signatureMethodHeader);
                client.DefaultRequestHeaders.Add("Content-Type", contentType);
                client.DefaultRequestHeaders.Add("Accept", contentType);

                var bvn = new BVNModel();
                bvn.BVN = "22147742421";
                var json = JsonConvert.SerializeObject(bvn);
                var aesEncrypted = encrypt(json);
                var toHexa = ToHexadecimal(aesEncrypted);
                var data = new StringContent(toHexa);

                HttpResponseMessage response = await client.PostAsync("/VerifySingleBVN", data);

                string result = response.Content.ReadAsStringAsync().Result;
            }


            //String originalString = "password";
            //Console.WriteLine(originalString);
            //String encryptedString = encrypt(originalString);
            //Console.WriteLine(encryptedString);
            //String decryptedString = decrypt(encryptedString);
            //Console.WriteLine(decryptedString);
            Console.ReadKey();
        }
    }
}
