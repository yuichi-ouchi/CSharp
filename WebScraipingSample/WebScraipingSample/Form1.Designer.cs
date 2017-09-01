namespace WebScraipingSample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.UrlText = new System.Windows.Forms.TextBox();
            this.GetButton = new System.Windows.Forms.Button();
            this.TitleText = new System.Windows.Forms.TextBox();
            this.HtmlText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StatusLable = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL";
            // 
            // UrlText
            // 
            this.UrlText.Location = new System.Drawing.Point(27, 41);
            this.UrlText.Name = "UrlText";
            this.UrlText.Size = new System.Drawing.Size(581, 19);
            this.UrlText.TabIndex = 1;
            // 
            // GetButton
            // 
            this.GetButton.Location = new System.Drawing.Point(533, 80);
            this.GetButton.Name = "GetButton";
            this.GetButton.Size = new System.Drawing.Size(75, 23);
            this.GetButton.TabIndex = 2;
            this.GetButton.Text = "開始";
            this.GetButton.UseVisualStyleBackColor = true;
            this.GetButton.Click += new System.EventHandler(this.GetButton_Click);
            // 
            // TitleText
            // 
            this.TitleText.Location = new System.Drawing.Point(27, 143);
            this.TitleText.Name = "TitleText";
            this.TitleText.Size = new System.Drawing.Size(581, 19);
            this.TitleText.TabIndex = 3;
            // 
            // HtmlText
            // 
            this.HtmlText.Location = new System.Drawing.Point(27, 205);
            this.HtmlText.Multiline = true;
            this.HtmlText.Name = "HtmlText";
            this.HtmlText.Size = new System.Drawing.Size(581, 297);
            this.HtmlText.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "タイトル";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "HTML";
            // 
            // StatusLable
            // 
            this.StatusLable.AutoSize = true;
            this.StatusLable.Location = new System.Drawing.Point(29, 67);
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Size = new System.Drawing.Size(0, 12);
            this.StatusLable.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 527);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StatusLable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HtmlText);
            this.Controls.Add(this.TitleText);
            this.Controls.Add(this.GetButton);
            this.Controls.Add(this.UrlText);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "WebScraipingSample";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UrlText;
        private System.Windows.Forms.Button GetButton;
        private System.Windows.Forms.TextBox TitleText;
        private System.Windows.Forms.TextBox HtmlText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label StatusLable;
        private System.Windows.Forms.Button button1;
    }
}

