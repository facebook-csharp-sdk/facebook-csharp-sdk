namespace Facebook.Samples.AuthenticationTool
{
    partial class Info
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
            this.lnkName = new System.Windows.Forms.LinkLabel();
            this.picProfilePic = new System.Windows.Forms.PictureBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.chkIsFanOfFacebookSdk = new System.Windows.Forms.CheckBox();
            this.lnkFacebokSdkFan = new System.Windows.Forms.LinkLabel();
            this.lblTotalFriends = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnPostToWall = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picProfilePic)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkName
            // 
            this.lnkName.AutoSize = true;
            this.lnkName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkName.Location = new System.Drawing.Point(68, 9);
            this.lnkName.Name = "lnkName";
            this.lnkName.Size = new System.Drawing.Size(117, 25);
            this.lnkName.TabIndex = 1;
            this.lnkName.TabStop = true;
            this.lnkName.Text = "[lnkName]";
            // 
            // picProfilePic
            // 
            this.picProfilePic.Location = new System.Drawing.Point(12, 12);
            this.picProfilePic.Name = "picProfilePic";
            this.picProfilePic.Size = new System.Drawing.Size(50, 50);
            this.picProfilePic.TabIndex = 2;
            this.picProfilePic.TabStop = false;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(70, 76);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(64, 13);
            this.lblFirstName.TabIndex = 3;
            this.lblFirstName.Text = "lblFirstName";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(70, 98);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(65, 13);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "lblLastName";
            // 
            // chkIsFanOfFacebookSdk
            // 
            this.chkIsFanOfFacebookSdk.AutoSize = true;
            this.chkIsFanOfFacebookSdk.Enabled = false;
            this.chkIsFanOfFacebookSdk.Location = new System.Drawing.Point(73, 146);
            this.chkIsFanOfFacebookSdk.Name = "chkIsFanOfFacebookSdk";
            this.chkIsFanOfFacebookSdk.Size = new System.Drawing.Size(157, 17);
            this.chkIsFanOfFacebookSdk.TabIndex = 5;
            this.chkIsFanOfFacebookSdk.Text = "Is fan of Facebook C# SDK";
            this.chkIsFanOfFacebookSdk.UseVisualStyleBackColor = true;
            // 
            // lnkFacebokSdkFan
            // 
            this.lnkFacebokSdkFan.AutoSize = true;
            this.lnkFacebokSdkFan.Location = new System.Drawing.Point(72, 166);
            this.lnkFacebokSdkFan.Name = "lnkFacebokSdkFan";
            this.lnkFacebokSdkFan.Size = new System.Drawing.Size(200, 13);
            this.lnkFacebokSdkFan.TabIndex = 6;
            this.lnkFacebokSdkFan.TabStop = true;
            this.lnkFacebokSdkFan.Text = "Like us on Facebook at our official page.";
            this.lnkFacebokSdkFan.Visible = false;
            this.lnkFacebokSdkFan.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFacebokSdkFan_LinkClicked);
            // 
            // lblTotalFriends
            // 
            this.lblTotalFriends.AutoSize = true;
            this.lblTotalFriends.Location = new System.Drawing.Point(70, 123);
            this.lblTotalFriends.Name = "lblTotalFriends";
            this.lblTotalFriends.Size = new System.Drawing.Size(67, 13);
            this.lblTotalFriends.TabIndex = 7;
            this.lblTotalFriends.Text = "[total friends]";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(72, 49);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(48, 13);
            this.lblUserId.TabIndex = 8;
            this.lblUserId.Text = "lblUserId";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Message:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(73, 190);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(327, 78);
            this.txtMessage.TabIndex = 10;
            // 
            // btnPostToWall
            // 
            this.btnPostToWall.Location = new System.Drawing.Point(325, 274);
            this.btnPostToWall.Name = "btnPostToWall";
            this.btnPostToWall.Size = new System.Drawing.Size(75, 23);
            this.btnPostToWall.TabIndex = 11;
            this.btnPostToWall.Text = "Post to Wall";
            this.btnPostToWall.UseVisualStyleBackColor = true;
            this.btnPostToWall.Click += new System.EventHandler(this.btnPostToWall_Click);
            // 
            // Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 316);
            this.Controls.Add(this.btnPostToWall);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.lblTotalFriends);
            this.Controls.Add(this.lnkFacebokSdkFan);
            this.Controls.Add(this.chkIsFanOfFacebookSdk);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.picProfilePic);
            this.Controls.Add(this.lnkName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Info";
            this.Text = "Facebook C# SDK Samples";
            ((System.ComponentModel.ISupportInitialize)(this.picProfilePic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkName;
        private System.Windows.Forms.PictureBox picProfilePic;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.CheckBox chkIsFanOfFacebookSdk;
        private System.Windows.Forms.LinkLabel lnkFacebokSdkFan;
        private System.Windows.Forms.Label lblTotalFriends;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnPostToWall;

    }
}