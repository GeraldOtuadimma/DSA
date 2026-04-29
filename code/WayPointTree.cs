using System;

namespace WaypointManager
{
    // Binary search tree for waypoint lookup and ordered display by name.
    class WayPointTree
    {
        private TreeNode root;
        private int numberOfWayPoints;

        public WayPointTree()
        {
            root = null;
            numberOfWayPoints = 0;
        }

        public int Count
        {
            get { return numberOfWayPoints; }
        }

        public bool IsEmpty()
        {
            return root == null;
        }

        public bool InsertItem(WayPoint item)
        {
            return insertItem(item, ref root);
        }

        // Simple wrapper for insert.
        public bool Add(WayPoint item)
        {
            return InsertItem(item);
        }

        private bool insertItem(WayPoint item, ref TreeNode tree)
        {
            if (item == null)
            {
                return false;
            }

            if (tree == null)
            {
                tree = new TreeNode(item);
                numberOfWayPoints++;
                return true;
            }

            int comparison = string.Compare(item.Name, tree.Data.Name, StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
            {
                return insertItem(item, ref tree.Left);
            }
            else if (comparison > 0)
            {
                return insertItem(item, ref tree.Right);
            }
            else
            {
                // Duplicate names are not added.
                return false;
            }
        }

        public WayPoint Find(string name)
        {
            return find(name, root);
        }

        public WayPoint FindByName(string name)
        {
            return Find(name);
        }

        private WayPoint find(string name, TreeNode tree)
        {
            if (tree == null)
            {
                return null;
            }

            int comparison = string.Compare(name, tree.Data.Name, StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
            {
                return tree.Data;
            }
            else if (comparison < 0)
            {
                return find(name, tree.Left);
            }
            else
            {
                return find(name, tree.Right);
            }
        }

        public void InOrder()
        {
            inOrder(root);
        }

        public void DisplayInOrder()
        {
            InOrder();
        }

        private void inOrder(TreeNode tree)
        {
            if (tree != null)
            {
                inOrder(tree.Left);
                tree.Data.DisplayWayPoint();
                inOrder(tree.Right);
            }
        }

        public int CountItems()
        {
            return count(root);
        }

        private int count(TreeNode tree)
        {
            if (tree == null)
            {
                return 0;
            }

            return count(tree.Left) + count(tree.Right) + 1;
        }

        public int Height()
        {
            return height(root);
        }

        private int height(TreeNode tree)
        {
            if (tree == null)
            {
                return 0;
            }

            return Math.Max(height(tree.Left), height(tree.Right)) + 1;
        }
    }
}
