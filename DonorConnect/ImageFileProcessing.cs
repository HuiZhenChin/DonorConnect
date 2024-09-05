using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DonorConnect
{
    public class ImageFileProcessing
    {
        public static string ProcessImages(string encryptedBase64Images)
        {
            if (string.IsNullOrEmpty(encryptedBase64Images))
            {
                return string.Empty;
            }

            // decrypt the base64 images string
            string decryptedBase64Images = DecryptAndSaveFiles(encryptedBase64Images);

            // split the decrypted string into individual base64 strings
            string[] fileEntries = decryptedBase64Images.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder imagesBuilder = new StringBuilder();
            imagesBuilder.AppendLine("<div class='file-grid'>");

            foreach (string fileEntry in fileEntries)
            {
                // check if the file is a PDF
                if (fileEntry.Contains("pdf:"))
                {
                    string base64Pdf = fileEntry.Split(new[] { "pdf:" }, StringSplitOptions.None)[1];
                    imagesBuilder.AppendLine($"<div class='file-item'><embed src='data:application/pdf;base64,{base64Pdf}' type='application/pdf' width='100%' height='400px' /></div>");
                }
                else
                {
                    imagesBuilder.AppendLine($"<div class='file-item'><img src='data:image/png;base64,{fileEntry}' alt='Image' class='img-fluid' /></div>");
                }
            }

            imagesBuilder.AppendLine("</div>");
            return imagesBuilder.ToString();
        }


        public static string ProcessFiles(string encryptedBase64File)
        {
            if (string.IsNullOrEmpty(encryptedBase64File))
            {
                return string.Empty;
            }

            // decrypt the base64 files string
            string decryptedBase64Files = DecryptAndSaveFiles(encryptedBase64File);

            // split the decrypted string into individual base64 file entries
            string[] base64FileArray = decryptedBase64Files.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder filesBuilder = new StringBuilder();

            foreach (string base64File in base64FileArray)
            {
                // split each entry into file type and base64 content
                string[] fileParts = base64File.Split(new char[] { ':' }, 2);
                if (fileParts.Length == 2)
                {
                    string fileName = fileParts[0]; // get the filename
                    string base64Content = fileParts[1].Trim();

                    string fileExtension = fileName.Split('.').Last().ToLower();  // get the file extension

                    string mimeType;
                    switch (fileExtension)
                    {
                        case "pdf":
                            mimeType = "application/pdf";
                            break;
                        case "docx":
                            mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        default:
                            continue; // skip unknown file types
                    }

                    // URL for the file
                    string fileDataUrl = $"data:{mimeType};base64,{base64Content}";

                    // download link for the file
                    filesBuilder.AppendLine($"<a href='{fileDataUrl}' download='{fileName}'>Download {fileName}</a><br />");
                }
            }

            return filesBuilder.ToString();
        }


        public static string DecryptAndSaveFiles(string encryptedImg)
        {
            var fileEntries = encryptedImg.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder decryptedFilesBuilder = new StringBuilder();

            foreach (var fileEntry in fileEntries)
            {
                var fileParts = fileEntry.Split(new[] { ':' }, 2);
                if (fileParts.Length != 2)
                {
                    throw new ArgumentException("Invalid encrypted file data format.");
                }

                string fileName = fileParts[0];  // extract filename
                string encryptedBase64String = fileParts[1];

                string decryptedBase64String = DecryptImages(encryptedBase64String);

                // check the file type and convert to its correct format
                if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    decryptedFilesBuilder.AppendLine($"{fileName}:{decryptedBase64String}");
                }
                else if (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    decryptedFilesBuilder.AppendLine($"{fileName}:{decryptedBase64String}");
                }
                else if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                         fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                         fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    // handle image formats (png, jpg, jpeg)
                    decryptedFilesBuilder.AppendLine(decryptedBase64String);
                }
                else
                {
                    // if file type is unknown, skip it or handle it as a general case
                    decryptedFilesBuilder.AppendLine(decryptedBase64String);
                }
            }

            return decryptedFilesBuilder.ToString();
        }



        public static string DecryptImages(string encryptedBase64String)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes("telleveryoneilovedonorconnectDc!");
            byte[] ivBytes = Encoding.UTF8.GetBytes("16ByteInitVector");
            byte[] cipherBytes = Convert.FromBase64String(encryptedBase64String);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string ConvertToBase64(IList<HttpPostedFile> postedFiles)
        {
            if (postedFiles == null || postedFiles.Count == 0)
            {
                return string.Empty;
            }

            List<string> encryptedFiles = new List<string>();

            foreach (HttpPostedFile uploadedFile in postedFiles)
            {
                using (BinaryReader reader = new BinaryReader(uploadedFile.InputStream))
                {
                    byte[] fileBytes = reader.ReadBytes((int)uploadedFile.InputStream.Length);
                    string base64String = Convert.ToBase64String(fileBytes);
                    string fileName = uploadedFile.FileName;

                    // encrypt the base64 string using AES 256, only encrypt the image string, filename no need
                    string encryptedBase64String = EncryptStringAES(base64String);

                    encryptedFiles.Add($"{fileName}:{encryptedBase64String}");
                }
            }

            return string.Join(",", encryptedFiles);
        }

        public static string EncryptStringAES(string imgString)
        {
            // 32-byte encryption key for AES-256
            byte[] keyBytes = Encoding.UTF8.GetBytes("telleveryoneilovedonorconnectDc!");
            // 16-byte IV 
            byte[] ivBytes = Encoding.UTF8.GetBytes("16ByteInitVector");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(imgString);
                        }
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }
    }
}