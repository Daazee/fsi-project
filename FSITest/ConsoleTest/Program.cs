using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            //await Reset();
            await SendAirtme();
            string signatureMethodHeader = "SHA256";
            string password = "";
            string username = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string signatureString = username + date + password;
            string signatureHeader = ComputeSha256Hash(signatureString);
            string authString = username + ":" + password;
            string authHeader = Base64Encoded(authString);
            string contentType = "application/json";
            string orgCode = username;
            string organizationCodeEncoded = Base64Encoded(orgCode);


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sandboxapi.fsi.ng/nibss/bvnr");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OrganisationCode", organizationCodeEncoded);
                client.DefaultRequestHeaders.Add("Sandbox-Key", "6fee89c336c046c4f0473d004906e3a7");
                client.DefaultRequestHeaders.Add("Authorization", authHeader);
                client.DefaultRequestHeaders.Add("SIGNATURE", signatureHeader);
                client.DefaultRequestHeaders.Add("SIGNATURE_METH", signatureMethodHeader);
                //client.DefaultRequestHeaders.Add("Content-Type", contentType);

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                client.DefaultRequestHeaders.Add("Accept", contentType);

                var bvn = new BVNModel();
                bvn.BVN = "22147742421";
                var json = JsonConvert.SerializeObject(bvn);
                var aesEncrypted = encrypt(json);
                var toHexa = ToHexadecimal(aesEncrypted);
                var data = new StringContent(toHexa);

                HttpResponseMessage response = await client.PostAsync("https://sandboxapi.fsi.ng/nibss/bvnr/VerifySingleBVN", data);

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

      
        public static async Task<AirtimeResponseModel> SendAirtme()
        {
            AirtimeResponseModel responseModel = new AirtimeResponseModel();
            string baseUrl = "https://sandboxapi.fsi.ng";
            string sendAirtime = "/atlabs/airtime/send";
            string url = baseUrl + sendAirtime;
           
            string contentType = "application/json";
            
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://sandboxapi.fsi.ng/nibss/bvnr");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Sandbox-Key", "6fee89c336c046c4f0473d004906e3a7");

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                client.DefaultRequestHeaders.Add("Accept", contentType);

                var requestObject = new AirtimeRequestModel();

                var recipient = new AirtimeRecipient();
                var recipients = new List<AirtimeRecipient>();
                recipient.amount = "100";
                recipient.currencyCode = "NGN";
                recipient.phoneNumber = "+2347036135901";

                requestObject.recipients = new List<AirtimeRecipient>();

                requestObject.recipients.Add(recipient);
                var data = new StringContent(JsonConvert.SerializeObject(requestObject));
                var response = await client.PostAsync(url, data);

                string result = response.Content.ReadAsStringAsync().Result;

                responseModel = JsonConvert.DeserializeObject<AirtimeResponseModel>(result);

                return responseModel;
            }

        }
    }

    public class AirtimeRecipient
    {
        public string phoneNumber { get; set; }
        public string amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class AirtimeRequestModel
    {
        public List<AirtimeRecipient> recipients { get; set; }
    }

    public class AirtimeResponse
    {
        public string phoneNumber { get; set; }
        public string errorMessage { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string requestId { get; set; }
        public string discount { get; set; }
    }

    public class AirtimeResponseModel
    {
        public string errorMessage { get; set; }
        public int numSent { get; set; }
        public string totalAmount { get; set; }
        public string totalDiscount { get; set; }
        public List<AirtimeResponse> responses { get; set; }
    }

}
