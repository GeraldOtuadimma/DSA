using System;

namespace WaypointManager
{
    // One WayPoint object stores one line from the waypoint CSV file.
    class WayPoint
    {
        private string name;
        private string code;
        private string latitude;
        private string longitude;
        private int height;
        private string description;

        public WayPoint(string name, string code, string latitude, string longitude, int height, string description)
        {
            this.name = name;
            this.code = code;
            this.latitude = latitude;
            this.longitude = longitude;
            this.height = height;
            this.description = description;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public void DisplayWayPoint()
        {
            Console.WriteLine(GetDisplayText());
        }

        public string GetDisplayText()
        {
            return "{" + name + ", " + code + ", pos[" + longitude + "," + latitude + "],h:" + height + "m, " + description + "}";
        }
    }
}
