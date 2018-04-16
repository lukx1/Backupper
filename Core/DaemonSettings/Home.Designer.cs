namespace DaemonSettings
{
    partial class Home
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxHJmeno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxHHeslo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHServer = new System.Windows.Forms.TextBox();
            this.checkBoxSSL = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxHSSLServer = new System.Windows.Forms.TextBox();
            this.checkBoxBackupSSL = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageIntroduction = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxIServerAdress = new System.Windows.Forms.TextBox();
            this.buttonIUlozit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxIIntroductionKod = new System.Windows.Forms.TextBox();
            this.tabPageHlavni = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPagePokrocile = new System.Windows.Forms.TabPage();
            this.buttonPUlozit = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tabPageSoukromyKlic = new System.Windows.Forms.TabPage();
            this.buttonKOK = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxKUsername = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxKPassword = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabPageIntroduction.SuspendLayout();
            this.tabPageHlavni.SuspendLayout();
            this.tabPagePokrocile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabPageSoukromyKlic.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxHJmeno
            // 
            this.textBoxHJmeno.Location = new System.Drawing.Point(100, 4);
            this.textBoxHJmeno.Name = "textBoxHJmeno";
            this.textBoxHJmeno.Size = new System.Drawing.Size(280, 20);
            this.textBoxHJmeno.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Jmeno";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Heslo";
            // 
            // textBoxHHeslo
            // 
            this.textBoxHHeslo.Location = new System.Drawing.Point(100, 30);
            this.textBoxHHeslo.Name = "textBoxHHeslo";
            this.textBoxHHeslo.Size = new System.Drawing.Size(280, 20);
            this.textBoxHHeslo.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "HTTP Server";
            // 
            // textBoxHServer
            // 
            this.textBoxHServer.Location = new System.Drawing.Point(100, 56);
            this.textBoxHServer.Name = "textBoxHServer";
            this.textBoxHServer.Size = new System.Drawing.Size(280, 20);
            this.textBoxHServer.TabIndex = 4;
            // 
            // checkBoxSSL
            // 
            this.checkBoxSSL.AutoSize = true;
            this.checkBoxSSL.Location = new System.Drawing.Point(9, 82);
            this.checkBoxSSL.Name = "checkBoxSSL";
            this.checkBoxSSL.Size = new System.Drawing.Size(80, 17);
            this.checkBoxSSL.TabIndex = 6;
            this.checkBoxSSL.Text = "Použít SSL";
            this.checkBoxSSL.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "HTTPS Server";
            // 
            // textBoxHSSLServer
            // 
            this.textBoxHSSLServer.Location = new System.Drawing.Point(100, 101);
            this.textBoxHSSLServer.Name = "textBoxHSSLServer";
            this.textBoxHSSLServer.Size = new System.Drawing.Size(280, 20);
            this.textBoxHSSLServer.TabIndex = 7;
            // 
            // checkBoxBackupSSL
            // 
            this.checkBoxBackupSSL.AutoSize = true;
            this.checkBoxBackupSSL.Location = new System.Drawing.Point(9, 129);
            this.checkBoxBackupSSL.Name = "checkBoxBackupSSL";
            this.checkBoxBackupSSL.Size = new System.Drawing.Size(94, 17);
            this.checkBoxBackupSSL.TabIndex = 13;
            this.checkBoxBackupSSL.Text = "Záložní HTTP";
            this.checkBoxBackupSSL.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageIntroduction);
            this.tabControl.Controls.Add(this.tabPageHlavni);
            this.tabControl.Controls.Add(this.tabPageSoukromyKlic);
            this.tabControl.Controls.Add(this.tabPagePokrocile);
            this.tabControl.Location = new System.Drawing.Point(0, 1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(394, 377);
            this.tabControl.TabIndex = 14;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageIntroduction
            // 
            this.tabPageIntroduction.Controls.Add(this.label5);
            this.tabPageIntroduction.Controls.Add(this.textBoxIServerAdress);
            this.tabPageIntroduction.Controls.Add(this.buttonIUlozit);
            this.tabPageIntroduction.Controls.Add(this.label4);
            this.tabPageIntroduction.Controls.Add(this.textBoxIIntroductionKod);
            this.tabPageIntroduction.Location = new System.Drawing.Point(4, 22);
            this.tabPageIntroduction.Name = "tabPageIntroduction";
            this.tabPageIntroduction.Size = new System.Drawing.Size(386, 351);
            this.tabPageIntroduction.TabIndex = 3;
            this.tabPageIntroduction.Text = "Introduction";
            this.tabPageIntroduction.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Adresa serveru";
            // 
            // textBoxIServerAdress
            // 
            this.textBoxIServerAdress.Location = new System.Drawing.Point(98, 152);
            this.textBoxIServerAdress.Name = "textBoxIServerAdress";
            this.textBoxIServerAdress.Size = new System.Drawing.Size(277, 20);
            this.textBoxIServerAdress.TabIndex = 3;
            // 
            // buttonIUlozit
            // 
            this.buttonIUlozit.Location = new System.Drawing.Point(300, 316);
            this.buttonIUlozit.Name = "buttonIUlozit";
            this.buttonIUlozit.Size = new System.Drawing.Size(75, 23);
            this.buttonIUlozit.TabIndex = 2;
            this.buttonIUlozit.Text = "Uložit";
            this.buttonIUlozit.UseVisualStyleBackColor = true;
            this.buttonIUlozit.Click += new System.EventHandler(this.buttonIUlozit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Introduction kód";
            // 
            // textBoxIIntroductionKod
            // 
            this.textBoxIIntroductionKod.Location = new System.Drawing.Point(8, 26);
            this.textBoxIIntroductionKod.Multiline = true;
            this.textBoxIIntroductionKod.Name = "textBoxIIntroductionKod";
            this.textBoxIIntroductionKod.Size = new System.Drawing.Size(367, 110);
            this.textBoxIIntroductionKod.TabIndex = 0;
            // 
            // tabPageHlavni
            // 
            this.tabPageHlavni.Controls.Add(this.button2);
            this.tabPageHlavni.Controls.Add(this.label1);
            this.tabPageHlavni.Controls.Add(this.checkBoxBackupSSL);
            this.tabPageHlavni.Controls.Add(this.textBoxHJmeno);
            this.tabPageHlavni.Controls.Add(this.textBoxHHeslo);
            this.tabPageHlavni.Controls.Add(this.label2);
            this.tabPageHlavni.Controls.Add(this.textBoxHServer);
            this.tabPageHlavni.Controls.Add(this.label3);
            this.tabPageHlavni.Controls.Add(this.label6);
            this.tabPageHlavni.Controls.Add(this.checkBoxSSL);
            this.tabPageHlavni.Controls.Add(this.textBoxHSSLServer);
            this.tabPageHlavni.Location = new System.Drawing.Point(4, 22);
            this.tabPageHlavni.Name = "tabPageHlavni";
            this.tabPageHlavni.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHlavni.Size = new System.Drawing.Size(386, 351);
            this.tabPageHlavni.TabIndex = 0;
            this.tabPageHlavni.Text = "Daemon nastavení";
            this.tabPageHlavni.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(300, 316);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Uložit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonHUlozit_Click);
            // 
            // tabPagePokrocile
            // 
            this.tabPagePokrocile.Controls.Add(this.buttonPUlozit);
            this.tabPagePokrocile.Controls.Add(this.dataGridView);
            this.tabPagePokrocile.Location = new System.Drawing.Point(4, 22);
            this.tabPagePokrocile.Name = "tabPagePokrocile";
            this.tabPagePokrocile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePokrocile.Size = new System.Drawing.Size(386, 351);
            this.tabPagePokrocile.TabIndex = 1;
            this.tabPagePokrocile.Text = "Pokročilé nastavení";
            this.tabPagePokrocile.UseVisualStyleBackColor = true;
            // 
            // buttonPUlozit
            // 
            this.buttonPUlozit.Location = new System.Drawing.Point(300, 316);
            this.buttonPUlozit.Name = "buttonPUlozit";
            this.buttonPUlozit.Size = new System.Drawing.Size(75, 23);
            this.buttonPUlozit.TabIndex = 1;
            this.buttonPUlozit.Text = "Uložit";
            this.buttonPUlozit.UseVisualStyleBackColor = true;
            this.buttonPUlozit.Click += new System.EventHandler(this.buttonPUlozit_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(4, 4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(376, 297);
            this.dataGridView.TabIndex = 0;
            // 
            // tabPageSoukromyKlic
            // 
            this.tabPageSoukromyKlic.Controls.Add(this.textBoxKPassword);
            this.tabPageSoukromyKlic.Controls.Add(this.label9);
            this.tabPageSoukromyKlic.Controls.Add(this.label8);
            this.tabPageSoukromyKlic.Controls.Add(this.textBoxKUsername);
            this.tabPageSoukromyKlic.Controls.Add(this.label7);
            this.tabPageSoukromyKlic.Controls.Add(this.buttonKOK);
            this.tabPageSoukromyKlic.Location = new System.Drawing.Point(4, 22);
            this.tabPageSoukromyKlic.Name = "tabPageSoukromyKlic";
            this.tabPageSoukromyKlic.Size = new System.Drawing.Size(386, 351);
            this.tabPageSoukromyKlic.TabIndex = 4;
            this.tabPageSoukromyKlic.Text = "Klíče";
            this.tabPageSoukromyKlic.UseVisualStyleBackColor = true;
            // 
            // buttonKOK
            // 
            this.buttonKOK.Location = new System.Drawing.Point(300, 316);
            this.buttonKOK.Name = "buttonKOK";
            this.buttonKOK.Size = new System.Drawing.Size(75, 23);
            this.buttonKOK.TabIndex = 0;
            this.buttonKOK.Text = "OK";
            this.buttonKOK.UseVisualStyleBackColor = true;
            this.buttonKOK.Click += new System.EventHandler(this.buttonKOK_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Uživatel";
            // 
            // textBoxKUsername
            // 
            this.textBoxKUsername.Location = new System.Drawing.Point(59, 34);
            this.textBoxKUsername.Name = "textBoxKUsername";
            this.textBoxKUsername.Size = new System.Drawing.Size(317, 20);
            this.textBoxKUsername.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Heslo";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Jméno";
            // 
            // textBoxKPassword
            // 
            this.textBoxKPassword.Location = new System.Drawing.Point(59, 60);
            this.textBoxKPassword.Name = "textBoxKPassword";
            this.textBoxKPassword.PasswordChar = '*';
            this.textBoxKPassword.Size = new System.Drawing.Size(317, 20);
            this.textBoxKPassword.TabIndex = 5;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 374);
            this.Controls.Add(this.tabControl);
            this.Name = "Home";
            this.Text = "Backupper";
            this.Load += new System.EventHandler(this.Home_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageIntroduction.ResumeLayout(false);
            this.tabPageIntroduction.PerformLayout();
            this.tabPageHlavni.ResumeLayout(false);
            this.tabPageHlavni.PerformLayout();
            this.tabPagePokrocile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabPageSoukromyKlic.ResumeLayout(false);
            this.tabPageSoukromyKlic.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHJmeno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxHHeslo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHServer;
        private System.Windows.Forms.CheckBox checkBoxSSL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxHSSLServer;
        private System.Windows.Forms.CheckBox checkBoxBackupSSL;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageHlavni;
        private System.Windows.Forms.TabPage tabPagePokrocile;
        private System.Windows.Forms.TabPage tabPageIntroduction;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxIServerAdress;
        private System.Windows.Forms.Button buttonIUlozit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxIIntroductionKod;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonPUlozit;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TabPage tabPageSoukromyKlic;
        private System.Windows.Forms.TextBox textBoxKPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxKUsername;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonKOK;
    }
}