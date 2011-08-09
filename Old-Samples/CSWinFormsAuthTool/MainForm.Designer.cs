namespace Facebook.Samples.AuthenticationTool
{
    partial class MainForm
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
            this.btnFacebookLogin = new System.Windows.Forms.Button();
            this.btnFacebookLoginDifferent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFacebookLogin
            // 
            this.btnFacebookLogin.Location = new System.Drawing.Point(64, 69);
            this.btnFacebookLogin.Name = "btnFacebookLogin";
            this.btnFacebookLogin.Size = new System.Drawing.Size(125, 52);
            this.btnFacebookLogin.TabIndex = 0;
            this.btnFacebookLogin.Text = "Login To Facebook";
            this.btnFacebookLogin.UseVisualStyleBackColor = true;
            this.btnFacebookLogin.Click += new System.EventHandler(this.btnFacebookLogin_Click);
            // 
            // btnFacebookLoginDifferent
            // 
            this.btnFacebookLoginDifferent.Location = new System.Drawing.Point(247, 69);
            this.btnFacebookLoginDifferent.Name = "btnFacebookLoginDifferent";
            this.btnFacebookLoginDifferent.Size = new System.Drawing.Size(125, 52);
            this.btnFacebookLoginDifferent.TabIndex = 1;
            this.btnFacebookLoginDifferent.Text = "Login to Facebook as different user";
            this.btnFacebookLoginDifferent.UseVisualStyleBackColor = true;
            this.btnFacebookLoginDifferent.Click += new System.EventHandler(this.btnFacebookLoginDifferent_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 152);
            this.Controls.Add(this.btnFacebookLoginDifferent);
            this.Controls.Add(this.btnFacebookLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Facebook C# SDK Samples - http://facebooksdk.codeplex.com";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFacebookLogin;
        private System.Windows.Forms.Button btnFacebookLoginDifferent;
    }
}