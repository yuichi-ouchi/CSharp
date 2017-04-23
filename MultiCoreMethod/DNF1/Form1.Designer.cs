namespace DNF1
{
    partial class DNF1_Form
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
            this.components = new System.ComponentModel.Container();
            this.textBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.StartCbButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox.Location = new System.Drawing.Point(0, 58);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(284, 203);
            this.textBox.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(179, 7);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(93, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // StartCbButton
            // 
            this.StartCbButton.Location = new System.Drawing.Point(179, 33);
            this.StartCbButton.Name = "StartCbButton";
            this.StartCbButton.Size = new System.Drawing.Size(93, 23);
            this.StartCbButton.TabIndex = 2;
            this.StartCbButton.Text = "START(CB)";
            this.StartCbButton.UseVisualStyleBackColor = true;
            this.StartCbButton.Click += new System.EventHandler(this.StartCbButton_Click);
            // 
            // DNF1_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.StartCbButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.textBox);
            this.Name = "DNF1_Form";
            this.Text = ".NET 1.X (A)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button startButton;
        internal System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button StartCbButton;
    }
}

