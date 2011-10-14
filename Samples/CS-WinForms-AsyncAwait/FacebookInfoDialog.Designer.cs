namespace CS_WinForms
{
    partial class FacebookInfoDialog
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
            this.btnProgressAndCancellation = new System.Windows.Forms.Button();
            this.lnkFacebokSdkFan = new System.Windows.Forms.LinkLabel();
            this.btnDeleteLastMessage = new System.Windows.Forms.Button();
            this.btnPostVideo = new System.Windows.Forms.Button();
            this.bntPostPicture = new System.Windows.Forms.Button();
            this.btnPostToWall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblTotalFriends = new System.Windows.Forms.Label();
            this.chkCSharpSdkFan = new System.Windows.Forms.CheckBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.lnkName = new System.Windows.Forms.LinkLabel();
            this.picProfile = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProgressAndCancellation
            // 
            this.btnProgressAndCancellation.Location = new System.Drawing.Point(171, 339);
            this.btnProgressAndCancellation.Name = "btnProgressAndCancellation";
            this.btnProgressAndCancellation.Size = new System.Drawing.Size(178, 23);
            this.btnProgressAndCancellation.TabIndex = 29;
            this.btnProgressAndCancellation.Text = "Upload Progress and Cancellation Sample";
            this.btnProgressAndCancellation.UseVisualStyleBackColor = true;
            this.btnProgressAndCancellation.Click += new System.EventHandler(this.btnProgressAndCancellation_Click);
            // 
            // lnkFacebokSdkFan
            // 
            this.lnkFacebokSdkFan.AutoSize = true;
            this.lnkFacebokSdkFan.Location = new System.Drawing.Point(83, 174);
            this.lnkFacebokSdkFan.Name = "lnkFacebokSdkFan";
            this.lnkFacebokSdkFan.Size = new System.Drawing.Size(200, 13);
            this.lnkFacebokSdkFan.TabIndex = 28;
            this.lnkFacebokSdkFan.TabStop = true;
            this.lnkFacebokSdkFan.Text = "Like us on Facebook at our official page.";
            this.lnkFacebokSdkFan.Visible = false;
            // 
            // btnDeleteLastMessage
            // 
            this.btnDeleteLastMessage.Enabled = false;
            this.btnDeleteLastMessage.Location = new System.Drawing.Point(256, 275);
            this.btnDeleteLastMessage.Name = "btnDeleteLastMessage";
            this.btnDeleteLastMessage.Size = new System.Drawing.Size(131, 23);
            this.btnDeleteLastMessage.TabIndex = 27;
            this.btnDeleteLastMessage.Text = "Delete Last Message";
            this.btnDeleteLastMessage.UseVisualStyleBackColor = true;
            this.btnDeleteLastMessage.Click += new System.EventHandler(this.btnDeleteLastMessage_Click);
            // 
            // btnPostVideo
            // 
            this.btnPostVideo.Location = new System.Drawing.Point(256, 310);
            this.btnPostVideo.Name = "btnPostVideo";
            this.btnPostVideo.Size = new System.Drawing.Size(103, 23);
            this.btnPostVideo.TabIndex = 26;
            this.btnPostVideo.Text = "Post Video";
            this.btnPostVideo.UseVisualStyleBackColor = true;
            this.btnPostVideo.Click += new System.EventHandler(this.btnPostVideo_Click);
            // 
            // bntPostPicture
            // 
            this.bntPostPicture.Location = new System.Drawing.Point(147, 310);
            this.bntPostPicture.Name = "bntPostPicture";
            this.bntPostPicture.Size = new System.Drawing.Size(103, 23);
            this.bntPostPicture.TabIndex = 25;
            this.bntPostPicture.Text = "Post Picture";
            this.bntPostPicture.UseVisualStyleBackColor = true;
            this.bntPostPicture.Click += new System.EventHandler(this.bntPostPicture_Click);
            // 
            // btnPostToWall
            // 
            this.btnPostToWall.Location = new System.Drawing.Point(119, 275);
            this.btnPostToWall.Name = "btnPostToWall";
            this.btnPostToWall.Size = new System.Drawing.Size(131, 23);
            this.btnPostToWall.TabIndex = 24;
            this.btnPostToWall.Text = "Post To Wall";
            this.btnPostToWall.UseVisualStyleBackColor = true;
            this.btnPostToWall.Click += new System.EventHandler(this.btnPostToWall_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Message";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(86, 201);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(354, 62);
            this.txtMessage.TabIndex = 22;
            // 
            // lblTotalFriends
            // 
            this.lblTotalFriends.AutoSize = true;
            this.lblTotalFriends.Location = new System.Drawing.Point(83, 127);
            this.lblTotalFriends.Name = "lblTotalFriends";
            this.lblTotalFriends.Size = new System.Drawing.Size(81, 13);
            this.lblTotalFriends.TabIndex = 21;
            this.lblTotalFriends.Text = "[lblTotalFriends]";
            // 
            // chkCSharpSdkFan
            // 
            this.chkCSharpSdkFan.AutoSize = true;
            this.chkCSharpSdkFan.Enabled = false;
            this.chkCSharpSdkFan.Location = new System.Drawing.Point(86, 154);
            this.chkCSharpSdkFan.Name = "chkCSharpSdkFan";
            this.chkCSharpSdkFan.Size = new System.Drawing.Size(280, 17);
            this.chkCSharpSdkFan.TabIndex = 20;
            this.chkCSharpSdkFan.Text = "Is fan of the official Facebok C# SDK facebook page.";
            this.chkCSharpSdkFan.UseVisualStyleBackColor = true;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(83, 98);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(71, 13);
            this.lblLastName.TabIndex = 19;
            this.lblLastName.Text = "[lblLastName]";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(83, 74);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(70, 13);
            this.lblFirstName.TabIndex = 18;
            this.lblFirstName.Text = "[lblFirstName]";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(83, 48);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(54, 13);
            this.lblUserId.TabIndex = 17;
            this.lblUserId.Text = "[lblUserId]";
            // 
            // lnkName
            // 
            this.lnkName.AutoSize = true;
            this.lnkName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkName.Location = new System.Drawing.Point(82, 11);
            this.lnkName.Name = "lnkName";
            this.lnkName.Size = new System.Drawing.Size(95, 24);
            this.lnkName.TabIndex = 16;
            this.lnkName.TabStop = true;
            this.lnkName.Text = "[lnkName]";
            // 
            // picProfile
            // 
            this.picProfile.Location = new System.Drawing.Point(16, 11);
            this.picProfile.Name = "picProfile";
            this.picProfile.Size = new System.Drawing.Size(50, 50);
            this.picProfile.TabIndex = 15;
            this.picProfile.TabStop = false;
            // 
            // FacebookInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 379);
            this.Controls.Add(this.btnProgressAndCancellation);
            this.Controls.Add(this.lnkFacebokSdkFan);
            this.Controls.Add(this.btnDeleteLastMessage);
            this.Controls.Add(this.btnPostVideo);
            this.Controls.Add(this.bntPostPicture);
            this.Controls.Add(this.btnPostToWall);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblTotalFriends);
            this.Controls.Add(this.chkCSharpSdkFan);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.lnkName);
            this.Controls.Add(this.picProfile);
            this.Name = "FacebookInfoDialog";
            this.Text = "CS-WinForms-AsyncAwait - Facebook C# SDK Samples";
            this.Load += new System.EventHandler(this.FacebookInfoDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProgressAndCancellation;
        private System.Windows.Forms.LinkLabel lnkFacebokSdkFan;
        private System.Windows.Forms.Button btnDeleteLastMessage;
        private System.Windows.Forms.Button btnPostVideo;
        private System.Windows.Forms.Button bntPostPicture;
        private System.Windows.Forms.Button btnPostToWall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblTotalFriends;
        private System.Windows.Forms.CheckBox chkCSharpSdkFan;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.LinkLabel lnkName;
        private System.Windows.Forms.PictureBox picProfile;

    }
}