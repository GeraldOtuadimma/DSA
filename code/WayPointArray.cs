using System;

namespace WaypointManager
{
    // This class is the array ADT for all the waypoints in the file.
    // The actual array is private, so the rest of the program uses these methods.
    class WayPointArray
    {
        private WayPoint[] wayPoints;
        private int numberOfWayPoints;

        public WayPointArray(int size)
        {
            wayPoints = new WayPoint[size];
            numberOfWayPoints = 0;
        }

        public int Count
        {
            get { return numberOfWayPoints; }
        }

        public bool IsFull()
        {
            return numberOfWayPoints == wayPoints.Length;
        }

        public bool IsEmpty()
        {
            return numberOfWayPoints == 0;
        }

        public bool AddWayPoint(WayPoint newWayPoint)
        {
            if (newWayPoint == null || IsFull())
            {
                return false;
            }

            // Duplicates are checked before this method is called.
            wayPoints[numberOfWayPoints] = newWayPoint;
            numberOfWayPoints++;
            return true;
        }

        public WayPoint FindByNameOrCode(string searchText)
        {
            for (int i = 0; i < numberOfWayPoints; i++)
            {
                if (wayPoints[i].Name.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                    wayPoints[i].Code.Equals(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    return wayPoints[i];
                }
            }

            return null;
        }

        public WayPoint FindByName(string searchName)
        {
            for (int i = 0; i < numberOfWayPoints; i++)
            {
                if (wayPoints[i].Name.Equals(searchName, StringComparison.OrdinalIgnoreCase))
                {
                    return wayPoints[i];
                }
            }

            return null;
        }

        public void DisplayAllWayPoints()
        {
            for (int i = 0; i < numberOfWayPoints; i++)
            {
                wayPoints[i].DisplayWayPoint();
            }
        }

        public void DisplayFirstWayPoints(int amountToDisplay)
        {
            int limit = amountToDisplay;

            if (limit > numberOfWayPoints)
            {
                limit = numberOfWayPoints;
            }

            for (int i = 0; i < limit; i++)
            {
                wayPoints[i].DisplayWayPoint();
            }
        }

        public int DisplayPartialNameMatches(string firstLetters)
        {
            int matches = 0;

            for (int i = 0; i < numberOfWayPoints; i++)
            {
                if (wayPoints[i].Name.StartsWith(firstLetters, StringComparison.OrdinalIgnoreCase))
                {
                    wayPoints[i].DisplayWayPoint();
                    matches++;
                }
            }

            if (matches == 0)
            {
                Console.WriteLine("No waypoints matched those first letters.");
            }

            return matches;
        }

        public int DisplayWayPointsUnderHeight(int maximumHeight)
        {
            int matches = 0;

            for (int i = 0; i < numberOfWayPoints; i++)
            {
                if (wayPoints[i].Height < maximumHeight)
                {
                    wayPoints[i].DisplayWayPoint();
                    matches++;
                }
            }

            if (matches == 0)
            {
                Console.WriteLine("No waypoints were found under that height.");
            }

            return matches;
        }
    }
}
