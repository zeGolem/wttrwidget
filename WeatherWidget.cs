using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace wttrwidget
{
    public partial class WeatherWidget : Form
    {
        private bool ShouldFormMove = false;
        private Point LastMousePosition = new Point(0, 0);
        public static Config.Config WidgetConfig;
        public static Config.ConfigParser WidgetConfigParser;

        public WeatherWidget()
        {
            InitializeComponent();
        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            // Load config
            string configFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\wttrwidget_conf\";
            if (!Directory.Exists(configFolderPath)) {
                Directory.CreateDirectory(configFolderPath);
            }
            WidgetConfigParser = new Config.ConfigParser(configFolderPath + "userconfig.conf");
            WidgetConfig = WidgetConfigParser.ParseToConfig();

            Config.Bounds bounds = WidgetConfig.GetBounds();
            this.Location = bounds.Location;
            this.Size = bounds.Size;

            // Load image
            string tempImgPath = Path.GetTempPath() + @"tmp_wttrwidget_img.png";
            try
            {
                File.Delete(tempImgPath);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Could not delete file :\n" + exception.Message);
            }
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile("http://fr.wttr.in/83149_nQ.png", tempImgPath);
            }
            catch (System.Exception)
            {
                if (File.Exists(tempImgPath)) {
                    MessageBox.Show("Unable to download image, using latest one available.",
                                    "Website could not be reached", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } else {
                    MessageBox.Show("Unable to download image, no other images available, throwing...",
                                    "Error: Could not load image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            this.pictureBox.Image = Image.FromFile(tempImgPath);

        }

        // Prevent Form from being on top of other windows
        private void FormReady(object sender, EventArgs e)
        {
            this.SendToBack(); // Puts the Form away nicely
                               // (Found here : https://stackoverflow.com/a/27313035/10957941, The *ONLY* usefull answer of the thread !)
                               // (It took me about an hour to find this, to anyone saying "you should use user32.dll", FUCK YOU, this is WAY easier and prettier.)
                               // (The same applies if your solution is to subclass the form.)
            Console.WriteLine("INFO>>Focus -> Form sent to back");
        }

        // Allows the Form to be moved using the Shift key modifier
        private void StopMovingForm(object sender, MouseEventArgs e)
        {
            this.ShouldFormMove = false;
            WidgetConfig.SetStartLocation(this.Location);
            WidgetConfigParser.WriteConfig(WidgetConfig);
        }

        private void StartMovingForm(object sender, MouseEventArgs e)
        {
            this.ShouldFormMove = true;
            this.LastMousePosition = e.Location;
        }
        private void MoveForm(object sender, MouseEventArgs e)
        {
            if (this.ShouldFormMove && Control.ModifierKeys == Keys.Shift)
            {
                Point formCurrPos = this.Location;
                Point mouseCurrPos = e.Location;

                int mouseDeltaX = this.LastMousePosition.X - mouseCurrPos.X;
                int mouseDeltaY = this.LastMousePosition.Y - mouseCurrPos.Y;

                this.Location = new Point(this.Location.X - mouseDeltaX, this.Location.Y - mouseDeltaY);
                this.Update();

            }
        }


    }
}
