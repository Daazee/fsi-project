﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace FSITest.Utils
{
    public class AESEncryptDescrypt
    {
        private static String key = "aesEncryptionKey";
        private static String initVector = "encryptionIntVec";

        private static AesManaged CreateAes()
        {
            var aes = new AesManaged();
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
    }
}