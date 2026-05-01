namespace WaypointManager
{
    // A Link is one node in the route linked list.
    // It stores a WayPoint instead of an int.
    class Link
    {
        private WayPoint data;
        private Link next;

        public Link(WayPoint item)
        {
            data = item;
            next = null;
        }

        public Link(WayPoint item, Link list)
        {
            data = item;
            next = list;
        }

        public WayPoint Data
        {
            set { data = value; }
            get { return data; }
        }

        public Link Next
        {
            set { next = value; }
            get { return next; }
        }
    }
}
