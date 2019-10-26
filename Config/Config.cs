using System;
using System.Collections.Generic;
using System.Drawing;

namespace wttrwidget.Config
{
    /*
        I know this code could be written more efficiently, but, hey, this WORKS
        (If you want to suggest any modifications, feel free to do so...)
     */
    public struct Bounds {
        public Bounds(Point location, Size size) {
            Location = location;
            Size = size;
        }
        public Point Location;
        public Size Size;
    }
    public class Config
    {
        private Point FormStartLocation;
        private Size FormStartSize;

        public Config(float version, bool ignoreVersion = false) {
            if (version != Program.CURRENT_VERSION && !ignoreVersion) {
                throw new NotSupportedException("Config file version not supported");
            }
        }
        public Bounds GetBounds() => new Bounds(this.FormStartLocation, this.FormStartSize);
        public void SetStartBounds(Bounds newBounds) {
            this.FormStartLocation = newBounds.Location;
            this.FormStartSize = newBounds.Size;
        }

        public void SetStartLocation(Point newLocation) => this.FormStartLocation = newLocation;
        public void SetStartSize(Size newSize) => this.FormStartSize = newSize;

        public void LoadConfig(Dictionary<string, string> configFile) {
            Point formStartLocation = new Point();
            var configStartLocation = configFile["FormStartLocation"].Split(',');
            formStartLocation.X = int.Parse(configStartLocation[0]);
            formStartLocation.Y = int.Parse(configStartLocation[1]);
            this.FormStartLocation = formStartLocation;

            Size formStartSize = new Size();
            var configStartSize = configFile["FormStartSize"].Split(',');
            formStartSize.Width = int.Parse(configStartSize[0]);
            formStartSize.Height = int.Parse(configStartSize[1]);
            this.FormStartSize = formStartSize;
        }

        public Dictionary<string, string> GetConfigAsDict() {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("FormStartLocation", $"{this.FormStartLocation.X},{this.FormStartLocation.Y}");
            dict.Add("FormStartSize", $"{this.FormStartSize.Width},{this.FormStartSize.Height}");

            return dict;
        }
    }
}