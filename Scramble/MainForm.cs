using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scramble
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Decrypt
        public static string Decrypt(string cipherText)
        {
            try
            {
                if (!string.IsNullOrEmpty(cipherText))
                {
                    byte[] keyBytes = new byte[]
                    {
                        0x57,0x00,0xE5,0x13,0x7A,0x17,0x61,0xB5,
                        0x75,0x90,0x05,0xD0,0x3C,0x17,0xF7,0x3A,
                        0xD3,0x56,0xC6,0x21,0x1F,0x52,0x02,0xEA,
                        0xD1,0x50,0x5F,0x2E,0x14,0xE7,0xC2,0x03
                    };

                    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                    RijndaelManaged symmetricKey = new RijndaelManaged();
                    symmetricKey.Mode = CipherMode.CBC;
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8"));
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
            catch { }
            return cipherText;
        }
        #endregion

        #region Encrypt
        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] cipherTextBytes;
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] initVectorBytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
                byte[] saltValueBytes = Encoding.ASCII.GetBytes("A!@$k233ie&");
                PasswordDeriveBytes password = new PasswordDeriveBytes("@@feklLLiE$", saltValueBytes, "SHA1", 5);
                byte[] keyBytes = password.GetBytes(32);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                    }
                }
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        private void textBoxPlainText_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBoxEncryptedText_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBoxPlainText_TextChanged(object sender, EventArgs e)
        {
            textBoxEncryptedText.Text = Encrypt(textBoxPlainText.Text);
        }

        private void textBoxEncryptedText_TextChanged(object sender, EventArgs e)
        {
            textBoxPlainText.Text = Decrypt(textBoxEncryptedText.Text);
        }
    }
}
