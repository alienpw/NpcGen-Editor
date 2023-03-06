namespace NpcGenDataEditorByLuka
{
    partial class TesteMapa
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
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox_path = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_path)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_path
            // 
            this.pictureBox_path.BackColor = System.Drawing.Color.White;
            this.pictureBox_path.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox_path.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox_path.MinimumSize = new System.Drawing.Size(512, 512);
            this.pictureBox_path.Name = "pictureBox_path";
            this.pictureBox_path.Size = new System.Drawing.Size(929, 557);
            this.pictureBox_path.TabIndex = 3;
            this.pictureBox_path.TabStop = false;
            this.pictureBox_path.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_path_MouseClick);
            this.pictureBox_path.MouseMove += new System.Windows.Forms.MouseEventHandler(this.path_tooltip);
            // 
            // TesteMapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(675, 554);
            this.Controls.Add(this.pictureBox_path);
            this.KeyPreview = true;
            this.Name = "TesteMapa";
            this.Text = "TesteMapa";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TesteMapa_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_path)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBox_path;
    }
}