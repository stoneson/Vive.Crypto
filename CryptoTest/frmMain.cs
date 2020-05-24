using HCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vive.Crypto;

namespace CryptoTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbEncryptType.SelectedIndexChanged += new System.EventHandler(this.cmbEncryptType_SelectedIndexChanged);
            cmbProviderType.SelectedIndexChanged += CmbProviderType_SelectedIndexChanged;
            cmbProviderType.SelectedIndex = 0;
            CmbProviderType_SelectedIndexChanged(sender, e);

        }
        private void refreshData()
        {
            txtPost.Text = INIHelper.Read(cmbEncryptType.Text+"_PostData.dat");
            txtSecretKey.Text = INIHelper.IniReadValue(cmbEncryptType.Text + "_txtSecretKey");
            txtprivateKey.Text = INIHelper.IniReadValue(cmbEncryptType.Text + "_txtprivateKey");
        }
        private void saveData()
        {
            INIHelper.Write(cmbEncryptType.Text + "_PostData.dat", txtPost.Text);
            INIHelper.IniWriteValue(cmbEncryptType.Text + "_txtSecretKey", txtSecretKey.Text);
            INIHelper.IniWriteValue(cmbEncryptType.Text + "_txtprivateKey", txtprivateKey.Text);
        }
        private void CmbProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbEncryptType.Items.Clear();
            if (cmbProviderType.Text == "对称加密")
            {
                this.cmbEncryptType.Items.AddRange(new object[] {
                                        "SM4-ECB",
                                        "SM4-CBC",
                                        "SM4JAVA-ECB",
                                        "SM4JAVA-CBC",
                                        "SM4JS-ECB",
                                        "SM4JS-CBC",
                                        "AES128",
                                        "AES192",
                                        "AES256",
                                        "DES",
                                        "TripleDES128",
                                        "TripleDES192",
                });

                btnDecrypt.Enabled = true;
                btnEncrypt.Enabled = true;

                lbltxtSecret.Text = "Key";
                lblprivateKey.Text = "IV";
                txtprivateKey.Visible = lblprivateKey.Visible = true;
                btnSign.Visible = btnVerify.Visible = false;
            }
            else if (cmbProviderType.Text == "哈希加密")
            {
                this.cmbEncryptType.Items.AddRange(new object[] {
                                        "HMACMD5",
                                        "HMACSHA1",
                                        "HMACSHA256",
                                        "HMACSHA384",
                                        "HMACSHA512",
                                        "MD4",
                                        "MD5",
                                        "SM3",
                                        "SHA1",
                                        "SHA256",
                                        "SHA384",
                                        "SHA512",
                });
                btnDecrypt.Enabled = false;
                btnEncrypt.Enabled = false;

                lbltxtSecret.Text = "Key";
                lblprivateKey.Text = "IV";
                txtprivateKey.Visible = lblprivateKey.Visible = false;
                btnSign.Visible = btnVerify.Visible = true;
            }
            else //非对称加密
            {
                this.cmbEncryptType.Items.AddRange(new object[] {
                                        "RSA",
                                        "RSA2",
                                        "SM2",
                });
                btnDecrypt.Enabled = true;
                btnEncrypt.Enabled = true;

                lbltxtSecret.Text = "公钥";
                lblprivateKey.Text = "私钥";
                txtprivateKey.Visible = lblprivateKey.Visible = true;
                btnSign.Visible = btnVerify.Visible = true;
            }
            cmbEncryptType.SelectedIndex = 0;
            cmbEncryptType_SelectedIndexChanged(sender, e);
        }
        private void cmbEncryptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSecretKey.Enabled = true;
            txtprivateKey.Enabled = true;
            btnGenerateKeyPair.Visible = true;
            if (cmbProviderType.Text == "对称加密")
            {
                txtprivateKey.Enabled = !(cmbEncryptType.Text.StartsWith("SM4") && cmbEncryptType.Text.Contains("ECB"));
            }
            else if (cmbProviderType.Text == "哈希加密")
            {
                txtSecretKey.Enabled = cmbEncryptType.Text.StartsWith("HMAC");
                txtprivateKey.Enabled = false;
                btnGenerateKeyPair.Visible = txtSecretKey.Enabled;
            }
            refreshData();
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            btnEncrypt.Enabled = false;
            txtResponse.Text = "";
            try
            {
                var response = Encrypt(txtPost.Text);
                if (!string.IsNullOrEmpty(response))
                    txtResponse.Text = response;
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            btnEncrypt.Enabled = true;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            btnDecrypt.Enabled = false;
            txtResponse.Text = "";
            try
            {
                var response = Decrypt(txtPost.Text);
                if (!string.IsNullOrEmpty(response))
                    txtResponse.Text = response;
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            btnDecrypt.Enabled = true;
        }
        private string Encrypt(String plainText)
        {
            var response = "";
            try
            {
                if (cmbProviderType.Text == "对称加密")
                {
                    var symmetricProviderType = cmbEncryptType.Text.Split('-')[0];
                    var symmetricProvider = CryptoFactory.CreateSymmetric(symmetricProviderType);
                    response = symmetricProvider.Encrypt(plainText, txtSecretKey.Text, cmbEncryptType.Text.Contains("ECB") || txtprivateKey.Text.IsNullOrEmpty() ? null : txtprivateKey.Text);
                }
                else //非对称加密
                {
                    var asymmetricProviderType = cmbEncryptType.Text;
                    var asymmetricProvide = CryptoFactory.CreateAsymmetric(asymmetricProviderType);
                    response = asymmetricProvide.Encrypt(plainText, txtSecretKey.Text);
                }
                saveData();
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            return response;
        }
        private string Decrypt(String cipherText)
        {
            var response = "";
            try
            {
                if (cmbProviderType.Text == "对称加密")
                {
                    var symmetricProviderType = cmbEncryptType.Text.Split('-')[0];
                    var symmetricProvider = CryptoFactory.CreateSymmetric(symmetricProviderType);
                    response = symmetricProvider.Decrypt(cipherText, txtSecretKey.Text, (cmbEncryptType.Text.Contains("ECB") || txtprivateKey.Text.IsNullOrEmpty() ? null : txtprivateKey.Text));
                }
                else //非对称加密
                {
                    var asymmetricProviderType = cmbEncryptType.Text;
                    var asymmetricProvide = CryptoFactory.CreateAsymmetric(asymmetricProviderType);
                    response = asymmetricProvide.Decrypt(cipherText, txtprivateKey.Text);
                }
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            return response;
        }
        /// <summary>
        /// 获取公钥 私钥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateKeyPair_Click(object sender, EventArgs e)
        {
            btnGenerateKeyPair.Enabled = false;
            try
            {
                if (cmbProviderType.Text == "对称加密")
                {
                    var symmetricProviderType = cmbEncryptType.Text.Split('-')[0];
                    var symmetricProvider = CryptoFactory.CreateSymmetric(symmetricProviderType);

                    var keypair = symmetricProvider.CreateKey();

                    txtSecretKey.Text = keypair.Key;
                    if (txtprivateKey.Enabled)
                        txtprivateKey.Text = keypair.IV;
                    else 
                        txtprivateKey.Text = "";
                }
                else if (cmbProviderType.Text == "哈希加密")
                {
                    if (cmbEncryptType.Text.StartsWith("HMAC"))
                        txtSecretKey.Text = Vive.Crypto.Core.Internals.RandomStringGenerator.Generate(32);
                    else 
                        txtSecretKey.Text = "";
                    txtprivateKey.Text = "";
                }
                else //非对称加密
                {
                    var asymmetricProviderType = cmbEncryptType.Text;
                    var asymmetricProvide = CryptoFactory.CreateAsymmetric(asymmetricProviderType);
                    var keypair = asymmetricProvide.CreateKey();

                    txtSecretKey.Text = keypair.PublickKey;
                    txtprivateKey.Text = keypair.PrivateKey;
                }

                saveData();
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            btnGenerateKeyPair.Enabled = true;
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSign_Click(object sender, EventArgs e)
        {
            btnSign.Enabled = false;
            try
            {
                var signData = "";
                if (cmbProviderType.Text == "哈希加密")
                {
                    var hashingProviderType = cmbEncryptType.Text;
                    var hashingProvider = CryptoFactory.CreateHashing(hashingProviderType);
                    signData = hashingProvider.Signature(txtPost.Text, txtSecretKey.Text);
                }
                else //非对称加密
                {
                    var asymmetricProviderType = cmbEncryptType.Text;
                    var asymmetricProvide = CryptoFactory.CreateAsymmetric(asymmetricProviderType);
                    asymmetricProvide.OutType = Vive.Crypto.Core.OutType.Hex;
                    signData = asymmetricProvide.SignData(txtPost.Text, txtprivateKey.Text);
                }
                txtResponse.Text = signData;
                //txtResponse.Text = "验名成功";

                INIHelper.Write(cmbEncryptType.Text + "_SignData.dat", signData);
                saveData();
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            btnSign.Enabled = true;
        }
        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerify_Click(object sender, EventArgs e)
        {
            btnSign.Enabled = false;
            try
            {
                var signData = INIHelper.Read(cmbEncryptType.Text + "_SignData.dat");
                if (signData.IsNullOrEmpty())
                {
                    txtResponse.Text = "无验名信息，请先签名";
                    return;
                }
                var ret = false;
                if (cmbProviderType.Text == "哈希加密")
                {
                    var hashingProviderType = cmbEncryptType.Text;
                    var hashingProvider = CryptoFactory.CreateHashing(hashingProviderType);
                    ret = hashingProvider.Verify(signData, txtPost.Text, txtSecretKey.Text);
                }
                else //非对称加密
                {
                    var asymmetricProviderType = cmbEncryptType.Text;
                    var asymmetricProvide = CryptoFactory.CreateAsymmetric(asymmetricProviderType);
                    asymmetricProvide.OutType = Vive.Crypto.Core.OutType.Hex;
                    ret = asymmetricProvide.VerifyData(txtPost.Text, signData, txtSecretKey.Text);
                }
                if (ret)
                    txtResponse.Text = "验签成功";
                else
                    txtResponse.Text = "验签失败";
            }
            catch (Exception ex) { txtResponse.Text = ex.ToString(); }
            btnSign.Enabled = true;
        }
        //===========================================================================================================================================================
        private void 复制替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtPost.Text = txtResponse.Text;
        }

    }
}
