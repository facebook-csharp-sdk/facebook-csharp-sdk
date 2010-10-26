namespace Facebook.Samples.AuthenticationTool {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.login = new System.Windows.Forms.Button();
            this.permissions = new System.Windows.Forms.CheckedListBox();
            this.accessToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.appId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.profiles = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.userId = new System.Windows.Forms.TextBox();
            this.loadProfiles = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(512, 439);
            this.webBrowser1.TabIndex = 0;
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(839, 484);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(75, 23);
            this.login.TabIndex = 1;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // permissions
            // 
            this.permissions.FormattingEnabled = true;
            this.permissions.Location = new System.Drawing.Point(727, 28);
            this.permissions.Name = "permissions";
            this.permissions.Size = new System.Drawing.Size(187, 409);
            this.permissions.TabIndex = 2;
            // 
            // accessToken
            // 
            this.accessToken.Location = new System.Drawing.Point(94, 457);
            this.accessToken.Name = "accessToken";
            this.accessToken.Size = new System.Drawing.Size(820, 20);
            this.accessToken.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Access Token";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(724, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Extended Permissions";
            // 
            // appId
            // 
            this.appId.Location = new System.Drawing.Point(534, 28);
            this.appId.Name = "appId";
            this.appId.Size = new System.Drawing.Size(187, 20);
            this.appId.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(531, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "AppId";
            // 
            // profiles
            // 
            this.profiles.FormattingEnabled = true;
            this.profiles.Location = new System.Drawing.Point(534, 150);
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(187, 289);
            this.profiles.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(531, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Profiles";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(531, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "UserId";
            // 
            // userId
            // 
            this.userId.Location = new System.Drawing.Point(534, 68);
            this.userId.Name = "userId";
            this.userId.Size = new System.Drawing.Size(187, 20);
            this.userId.TabIndex = 10;
            // 
            // loadProfiles
            // 
            this.loadProfiles.Location = new System.Drawing.Point(613, 94);
            this.loadProfiles.Name = "loadProfiles";
            this.loadProfiles.Size = new System.Drawing.Size(108, 23);
            this.loadProfiles.TabIndex = 12;
            this.loadProfiles.Text = "Load Profiles";
            this.loadProfiles.UseVisualStyleBackColor = true;
            this.loadProfiles.Click += new System.EventHandler(this.loadProfiles_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 518);
            this.Controls.Add(this.loadProfiles);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.userId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.profiles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.appId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accessToken);
            this.Controls.Add(this.permissions);
            this.Controls.Add(this.login);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.CheckedListBox permissions;
        private System.Windows.Forms.TextBox accessToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox appId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox profiles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox userId;
        private System.Windows.Forms.Button loadProfiles;
    }
}

