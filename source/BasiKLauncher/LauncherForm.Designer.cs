namespace BasiK.BasiKLauncher
{
    partial class LauncherForm
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
            this.gameControlsPanel = new BasiK.BasiKLauncher.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // gameControlsPanel
            // 
            this.gameControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameControlsPanel.Location = new System.Drawing.Point(0, 0);
            this.gameControlsPanel.Name = "gameControlsPanel";
            this.gameControlsPanel.Size = new System.Drawing.Size(588, 342);
            this.gameControlsPanel.TabIndex = 0;
            // 
            // LWBPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(588, 342);
            this.Controls.Add(this.gameControlsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "LWBPForm";
            this.ShowIcon = false;
            this.Text = "BasiK - BasiKLauncher";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel gameControlsPanel;

    }
}

