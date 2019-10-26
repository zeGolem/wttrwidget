using System;
using System.Windows.Forms;

namespace wttrwidget
{
    partial class WeatherWidget
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private PictureBox pictureBox;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 504);
            this.Text = "Weather";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Opacity = 0.75;
            this.Load += this.OnFormLoad;
            this.ShowInTaskbar = false;
            this.Activated += this.FormReady;

            this.pictureBox = new PictureBox();

            this.pictureBox.Visible = true;
            this.pictureBox.SetBounds(0, 0, 441, 504);
            this.pictureBox.MouseDown += this.StartMovingForm;
            this.pictureBox.MouseUp += this.StopMovingForm;
            this.pictureBox.MouseMove += this.MoveForm;
            this.Controls.Add(this.pictureBox);
        }
        #endregion
    }
}
