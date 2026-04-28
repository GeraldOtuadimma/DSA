namespace WaypointManager
{
    // One node in the binary search tree.
    class TreeNode
    {
        private WayPoint data;
        public TreeNode Left, Right;

        public TreeNode(WayPoint item)
        {
            data = item;
            Left = null;
            Right = null;
        }

        public WayPoint Data
        {
            set { data = value; }
            get { return data; }
        }
    }
}
