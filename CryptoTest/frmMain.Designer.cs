namespace CryptoTest
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtPost = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnGenerateKeyPair = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtprivateKey = new System.Windows.Forms.TextBox();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.cmbProviderType = new System.Windows.Forms.ComboBox();
            this.cmbEncryptType = new System.Windows.Forms.ComboBox();
            this.lblprivateKey = new System.Windows.Forms.Label();
            this.lbltxtSecret = new System.Windows.Forms.Label();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制替换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtPost);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtResponse);
            this.splitContainer1.Size = new System.Drawing.Size(1344, 701);
            this.splitContainer1.SplitterDistance = 192;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtPost
            // 
            this.txtPost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPost.Font = new System.Drawing.Font("宋体", 11F);
            this.txtPost.Location = new System.Drawing.Point(0, 75);
            this.txtPost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPost.Multiline = true;
            this.txtPost.Name = "txtPost";
            this.txtPost.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPost.Size = new System.Drawing.Size(1344, 117);
            this.txtPost.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnVerify);
            this.panel2.Controls.Add(this.btnDecrypt);
            this.panel2.Controls.Add(this.btnGenerateKeyPair);
            this.panel2.Controls.Add(this.btnSign);
            this.panel2.Controls.Add(this.btnEncrypt);
            this.panel2.Controls.Add(this.txtprivateKey);
            this.panel2.Controls.Add(this.txtSecretKey);
            this.panel2.Controls.Add(this.cmbProviderType);
            this.panel2.Controls.Add(this.cmbEncryptType);
            this.panel2.Controls.Add(this.lblprivateKey);
            this.panel2.Controls.Add(this.lbltxtSecret);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1344, 75);
            this.panel2.TabIndex = 0;
            this.toolTip1.SetToolTip(this.panel2, "POST 信息");
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerify.Font = new System.Drawing.Font("宋体", 11F);
            this.btnVerify.Location = new System.Drawing.Point(1279, 36);
            this.btnVerify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(61, 32);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "验签";
            this.toolTip1.SetToolTip(this.btnVerify, "公钥验签");
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecrypt.Font = new System.Drawing.Font("宋体", 11F);
            this.btnDecrypt.Location = new System.Drawing.Point(1279, 2);
            this.btnDecrypt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(61, 32);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "解密";
            this.toolTip1.SetToolTip(this.btnDecrypt, "测试解密");
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnGenerateKeyPair
            // 
            this.btnGenerateKeyPair.Font = new System.Drawing.Font("宋体", 10F);
            this.btnGenerateKeyPair.Location = new System.Drawing.Point(136, 40);
            this.btnGenerateKeyPair.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerateKeyPair.Name = "btnGenerateKeyPair";
            this.btnGenerateKeyPair.Size = new System.Drawing.Size(124, 32);
            this.btnGenerateKeyPair.TabIndex = 2;
            this.btnGenerateKeyPair.Text = "生成密钥";
            this.toolTip1.SetToolTip(this.btnGenerateKeyPair, "获取公钥 私钥");
            this.btnGenerateKeyPair.UseVisualStyleBackColor = true;
            this.btnGenerateKeyPair.Click += new System.EventHandler(this.btnGenerateKeyPair_Click);
            // 
            // btnSign
            // 
            this.btnSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSign.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSign.Location = new System.Drawing.Point(1215, 38);
            this.btnSign.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(60, 32);
            this.btnSign.TabIndex = 2;
            this.btnSign.Text = "签名";
            this.toolTip1.SetToolTip(this.btnSign, "私钥签名");
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncrypt.Font = new System.Drawing.Font("宋体", 11F);
            this.btnEncrypt.Location = new System.Drawing.Point(1215, 4);
            this.btnEncrypt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(60, 32);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "加密";
            this.toolTip1.SetToolTip(this.btnEncrypt, "测试加密");
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtprivateKey
            // 
            this.txtprivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtprivateKey.Font = new System.Drawing.Font("宋体", 11F);
            this.txtprivateKey.Location = new System.Drawing.Point(329, 40);
            this.txtprivateKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtprivateKey.Name = "txtprivateKey";
            this.txtprivateKey.Size = new System.Drawing.Size(873, 28);
            this.txtprivateKey.TabIndex = 4;
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSecretKey.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSecretKey.Location = new System.Drawing.Point(329, 6);
            this.txtSecretKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(873, 28);
            this.txtSecretKey.TabIndex = 4;
            // 
            // cmbProviderType
            // 
            this.cmbProviderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProviderType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbProviderType.FormattingEnabled = true;
            this.cmbProviderType.Items.AddRange(new object[] {
            "对称加密",
            "哈希加密",
            "非对称加密"});
            this.cmbProviderType.Location = new System.Drawing.Point(4, 10);
            this.cmbProviderType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProviderType.Name = "cmbProviderType";
            this.cmbProviderType.Size = new System.Drawing.Size(123, 25);
            this.cmbProviderType.TabIndex = 3;
            // 
            // cmbEncryptType
            // 
            this.cmbEncryptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncryptType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbEncryptType.FormattingEnabled = true;
            this.cmbEncryptType.Items.AddRange(new object[] {
            "SM4-ECB",
            "SM4-CBC",
            "SM3",
            "SM2"});
            this.cmbEncryptType.Location = new System.Drawing.Point(136, 9);
            this.cmbEncryptType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbEncryptType.Name = "cmbEncryptType";
            this.cmbEncryptType.Size = new System.Drawing.Size(123, 25);
            this.cmbEncryptType.TabIndex = 3;
            // 
            // lblprivateKey
            // 
            this.lblprivateKey.AutoSize = true;
            this.lblprivateKey.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblprivateKey.Location = new System.Drawing.Point(268, 48);
            this.lblprivateKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblprivateKey.Name = "lblprivateKey";
            this.lblprivateKey.Size = new System.Drawing.Size(49, 20);
            this.lblprivateKey.TabIndex = 1;
            this.lblprivateKey.Text = "私钥";
            // 
            // lbltxtSecret
            // 
            this.lbltxtSecret.AutoSize = true;
            this.lbltxtSecret.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbltxtSecret.Location = new System.Drawing.Point(268, 12);
            this.lbltxtSecret.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbltxtSecret.Name = "lbltxtSecret";
            this.lbltxtSecret.Size = new System.Drawing.Size(49, 20);
            this.lbltxtSecret.TabIndex = 1;
            this.lbltxtSecret.Text = "公钥";
            // 
            // txtResponse
            // 
            this.txtResponse.ContextMenuStrip = this.contextMenuStrip1;
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.Font = new System.Drawing.Font("宋体", 11F);
            this.txtResponse.Location = new System.Drawing.Point(0, 0);
            this.txtResponse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(1344, 504);
            this.txtResponse.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制替换ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // 复制替换ToolStripMenuItem
            // 
            this.复制替换ToolStripMenuItem.Name = "复制替换ToolStripMenuItem";
            this.复制替换ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.复制替换ToolStripMenuItem.Text = "复制替换";
            this.复制替换ToolStripMenuItem.Click += new System.EventHandler(this.复制替换ToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10F);
            this.button1.Location = new System.Drawing.Point(4, 40);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 32);
            this.button1.TabIndex = 5;
            this.button1.Text = "生成PDF";
            this.toolTip1.SetToolTip(this.button1, "获取公钥 私钥");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 701);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1359, 738);
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CryptoTest";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtPost;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.ComboBox cmbEncryptType;
        private System.Windows.Forms.Label lbltxtSecret;
        private System.Windows.Forms.TextBox txtprivateKey;
        private System.Windows.Forms.Label lblprivateKey;
        private System.Windows.Forms.Button btnGenerateKeyPair;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制替换ToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmbProviderType;
        private System.Windows.Forms.Button button1;
    }
}

