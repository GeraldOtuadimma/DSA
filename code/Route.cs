using System;

namespace WaypointManager
{
    // A Route is a named linked list of waypoints.
    class Route
    {
        private string routeName;
        private Link list;
        private Link last; // last item in the list, used when adding to the end

        public Route(string routeName)
        {
            this.routeName = routeName;
            list = null;
            last = null;
        }

        public string RouteName
        {
            get { return routeName; }
            set { routeName = value; }
        }

        public bool IsEmpty()
        {
            return list == null;
        }

        public void AddFront(WayPoint item)
        {
            Link newLink = new Link(item, list);
            list = newLink;

            if (last == null)
            {
                last = newLink;
            }
        }

        public void AddEnd(WayPoint item)
        {
            Link newLink = new Link(item);

            if (list == null)
            {
                list = newLink;
                last = newLink;
            }
            else
            {
                last.Next = newLink;
                last = newLink;
            }
        }

        public int NumberOfItems()
        {
            Link temp = list;
            int count = 0;

            while (temp != null)
            {
                count++;
                temp = temp.Next;
            }

            return count;
        }

        public bool IsPresent(string nameOrCode)
        {
            Link temp = list;

            while (temp != null)
            {
                if (Matches(temp.Data, nameOrCode))
                {
                    return true;
                }

                temp = temp.Next;
            }

            return false;
        }

        public void InsertAtPosition(WayPoint item, int position)
        {
            if (position <= 1 || list == null)
            {
                AddFront(item);
                return;
            }

            Link temp = list;
            int currentPosition = 1;

            while (temp.Next != null && currentPosition < position - 1)
            {
                temp = temp.Next;
                currentPosition++;
            }

            Link newLink = new Link(item, temp.Next);
            temp.Next = newLink;

            if (newLink.Next == null)
            {
                last = newLink;
            }
        }

        public bool RemoveWayPoint(string nameOrCode)
        {
            if (list == null)
            {
                return false;
            }

            if (Matches(list.Data, nameOrCode))
            {
                list = list.Next;

                if (list == null)
                {
                    last = null;
                }

                return true;
            }

            Link temp = list;

            while (temp.Next != null)
            {
                if (Matches(temp.Next.Data, nameOrCode))
                {
                    if (temp.Next == last)
                    {
                        last = temp;
                    }

                    temp.Next = temp.Next.Next;
                    return true;
                }

                temp = temp.Next;
            }

            return false;
        }

        public void ReverseRoute()
        {
            Link previous = null;
            Link current = list;
            Link next;

            last = list;

            while (current != null)
            {
                next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }

            list = previous;
        }

        public void DisplayRoute()
        {
            Console.WriteLine("Route: " + routeName + " (" + NumberOfItems() + " waypoints)");

            if (list == null)
            {
                Console.WriteLine("The route is empty.");
                return;
            }

            Link temp = list;
            int position = 1;

            while (temp != null)
            {
                Console.Write(position + ". ");
                temp.Data.DisplayWayPoint();
                temp = temp.Next;
                position++;
            }
        }

        private bool Matches(WayPoint wayPoint, string nameOrCode)
        {
            return wayPoint.Name.Equals(nameOrCode, StringComparison.OrdinalIgnoreCase) ||
                   wayPoint.Code.Equals(nameOrCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}
