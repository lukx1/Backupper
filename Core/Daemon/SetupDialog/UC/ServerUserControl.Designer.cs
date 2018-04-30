namespace SetupDialog
{
    partial class ServerUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.buttonDalsi = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(6, 65);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(444, 20);
            this.textBoxServer.TabIndex = 7;
            // 
            // buttonDalsi
            // 
            this.buttonDalsi.Location = new System.Drawing.Point(382, 104);
            this.buttonDalsi.Name = "buttonDalsi";
            this.buttonDalsi.Size = new System.Drawing.Size(75, 23);
            this.buttonDalsi.TabIndex = 6;
            this.buttonDalsi.Text = "Další";
            this.buttonDalsi.UseVisualStyleBackColor = true;
            this.buttonDalsi.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Zadejte adresu zálohovacího serveru";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Zálohovací server";
            // 
            // ServerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxServer);
            this.Controls.Add(this.buttonDalsi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "ServerUserControl";
            this.Size = new System.Drawing.Size(460, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Button buttonDalsi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
