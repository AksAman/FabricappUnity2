using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace helloVoRld
{
    [Obsolete("This class is not supposed to use anywhere", error: true)]
    public static class Encryptor
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        private static string Password => Globals.RequestHeader.Value;

        private static byte[] Salt { get; } = new byte[32];

        public static void GenerateRandomSalt()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(Salt);
                }
            }
        }

        private static void FileEncrypt(MemoryStream DataStrean, string outputLocation)
        {
            //generate random salt
            GenerateRandomSalt();

            //create output file name
            FileStream fsCrypt = new FileStream(outputLocation, FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);

            //Set Rijndael symmetric encryption algorithm   
            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, Salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(Salt, 0, Salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = DataStrean.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                // Close up
                DataStrean.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
        }

        private static void FileDecrypt(string inputFile, string outputFile)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(Salt, 0, Salt.Length);

            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128
            };
            var key = new Rfc2898DeriveBytes(passwordBytes, Salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }
        }

        public static void EncryptWrite(string FileLoc, byte[] Input)
        {
            FileEncrypt(new MemoryStream(Input), FileLoc);
        }

        public static byte[] DecryptLoad(string FileLoc)
        {
            FileDecrypt(FileLoc, FileLoc + ".dec");
            byte[] arr = File.ReadAllBytes(FileLoc + ".dec");
            File.Delete(FileLoc + ".dec");
            return arr;
        }
    }
}
