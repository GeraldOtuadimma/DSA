using System;

namespace WaypointManager
{
    // This simple array stores more than one route.
    class RouteArray
    {
        private Route[] routes;
        private int numberOfRoutes;

        public RouteArray(int size)
        {
            routes = new Route[size];
            numberOfRoutes = 0;
        }

        public int Count
        {
            get { return numberOfRoutes; }
        }

        public bool IsFull()
        {
            return numberOfRoutes == routes.Length;
        }

        public bool AddRoute(Route newRoute)
        {
            if (IsFull())
            {
                return false;
            }

            routes[numberOfRoutes] = newRoute;
            numberOfRoutes++;
            return true;
        }

        public Route GetRouteAt(int index)
        {
            if (index < 0 || index >= numberOfRoutes)
            {
                return null;
            }

            return routes[index];
        }

        public Route FindRouteByName(string routeName)
        {
            for (int i = 0; i < numberOfRoutes; i++)
            {
                if (routes[i].RouteName.Equals(routeName, StringComparison.OrdinalIgnoreCase))
                {
                    return routes[i];
                }
            }

            return null;
        }

        public void DisplayRoutes()
        {
            if (numberOfRoutes == 0)
            {
                Console.WriteLine("No routes have been created.");
                return;
            }

            for (int i = 0; i < numberOfRoutes; i++)
            {
                Console.WriteLine((i + 1) + ". " + routes[i].RouteName + " (" + routes[i].NumberOfItems() + " waypoints)");
            }
        }
    }
}
