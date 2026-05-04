using System;
using System.IO;
using System.Globalization;

namespace WaypointManager
{
    class Program
    {
        private static WayPointArray allWayPoints;
        private static RouteArray allRoutes;
        private static Route activeRoute;
        private static WayPointTree allWayPointsTree;

        static void Main(string[] args)
        {
            Console.WriteLine("GPS Route Creation Tool");
            Console.WriteLine("Waypoints are stored in an array and indexed in a binary search tree. Routes are stored as linked lists.");
            Console.WriteLine();

            string fileName = FindWaypointFile();

            if (fileName == "")
            {
                Console.WriteLine("UK_waypoints.csv could not be found.");
                Console.WriteLine("Keep the data folder with the project and try again.");
                Console.ReadLine();
                return;
            }

            allWayPoints = ReadFileWayPoints(fileName);
            allRoutes = new RouteArray(20);
            activeRoute = null;

            Console.WriteLine("Loaded " + allWayPoints.Count + " unique waypoints into the array.");
            Console.WriteLine();

            RunTestCode();
            MenuLoop();
        }

        private static string FindWaypointFile()
        {
            string fileName = "UK_waypoints.csv";

            string[] possiblePaths = new string[6];
            possiblePaths[0] = fileName;
            possiblePaths[1] = Path.Combine("data", fileName);
            possiblePaths[2] = Path.Combine("..", "data", fileName);
            possiblePaths[3] = Path.Combine("..", "..", "data", fileName);
            possiblePaths[4] = Path.Combine("..", "..", "..", "data", fileName);
            possiblePaths[5] = Path.Combine("..", "..", "..", "..", "data", fileName);

            for (int i = 0; i < possiblePaths.Length; i++)
            {
                if (File.Exists(possiblePaths[i]))
                {
                    return possiblePaths[i];
                }
            }

            return "";
        }

        private static WayPointArray ReadFileWayPoints(string fileName)
        {
            string[] linesInFile = File.ReadAllLines(fileName);
            WayPointArray store = new WayPointArray(linesInFile.Length);
            allWayPointsTree = new WayPointTree();

            int lineNumber = 0;

            foreach (string line in linesInFile)
            {
                lineNumber++;

                // The first line is the heading row, so it is skipped.
                if (lineNumber != 1 && line != "")
                {
                    string[] featuresInLine = SplitCsvLine(line);

                    if (featuresInLine.Length >= 6)
                    {
                        string name = featuresInLine[0].Trim();
                        string code = featuresInLine[1].Trim();
                        string latitude = featuresInLine[3].Trim();
                        string longitude = featuresInLine[4].Trim();
                        int height = ConvertElevationToMeters(featuresInLine[5].Trim());
                        string description = BuildDescription(featuresInLine);

                        WayPoint newWayPoint = new WayPoint(name, code, latitude, longitude, height, description);

                        // The tree rejects duplicate names before the waypoint is stored in the array.
                        if (allWayPointsTree.InsertItem(newWayPoint))
                        {
                            store.AddWayPoint(newWayPoint);
                        }
                    }
                }
            }

            return store;
        }


        private static string[] SplitCsvLine(string line)
        {
            // This splits a CSV line while keeping commas inside quotes.
            string[] temporaryValues = new string[line.Length + 1];
            int numberOfValues = 0;
            string currentValue = "";
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char currentCharacter = line[i];

                if (currentCharacter == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (currentCharacter == ',' && !insideQuotes)
                {
                    temporaryValues[numberOfValues] = currentValue;
                    numberOfValues++;
                    currentValue = "";
                }
                else
                {
                    currentValue = currentValue + currentCharacter;
                }
            }

            temporaryValues[numberOfValues] = currentValue;
            numberOfValues++;

            string[] values = new string[numberOfValues];
            for (int i = 0; i < numberOfValues; i++)
            {
                values[i] = temporaryValues[i];
            }

            return values;
        }

        private static int ConvertElevationToMeters(string elevationText)
        {
            string lowerElevation = elevationText.ToLower();
            string numberText = "";

            for (int i = 0; i < elevationText.Length; i++)
            {
                if (char.IsDigit(elevationText[i]) || elevationText[i] == '.' || elevationText[i] == '-')
                {
                    numberText = numberText + elevationText[i];
                }
            }

            if (numberText == "")
            {
                return 0;
            }

            double height = Convert.ToDouble(numberText, CultureInfo.InvariantCulture);

            if (lowerElevation.Contains("ft"))
            {
                height = Math.Ceiling(height * 0.3048);
            }

            return Convert.ToInt32(height);
        }

        private static string BuildDescription(string[] featuresInLine)
        {
            string description = "";
            int startPosition = 10;

            // Skip the general phrase "Turn Point" when it appears before the description.
            if (featuresInLine.Length > 10 && featuresInLine[10].Trim().ToLower() == "turn point")
            {
                startPosition = 11;
            }

            for (int i = startPosition; i < featuresInLine.Length; i++)
            {
                if (description != "")
                {
                    description = description + ",";
                }

                description = description + featuresInLine[i].Trim();
            }

            return description;
        }

        private static void RunTestCode()
        {
            SimpleUnitTests.RunAll();

            Console.WriteLine("--- Simple test code ---");

            Console.WriteLine("Test 1: first 3 waypoints in the array");
            allWayPoints.DisplayFirstWayPoints(3);
            Console.WriteLine();

            Console.WriteLine("Test 2: exact array search for Ambleside");
            WayPoint ambleside = allWayPoints.FindByNameOrCode("Ambleside");
            DisplaySearchResult(ambleside);
            Console.WriteLine();

            Console.WriteLine("Test 3: binary search tree search for Ambleside");
            DisplaySearchResult(allWayPointsTree.Find("Ambleside"));
            Console.WriteLine();

            Console.WriteLine("Test 4: partial search using Amb");
            allWayPoints.DisplayPartialNameMatches("Amb");
            Console.WriteLine();

            Console.WriteLine("Test 5: height search under 1 metre");
            allWayPoints.DisplayWayPointsUnderHeight(1);
            Console.WriteLine();

            Console.WriteLine("Test 6: route linked-list methods");
            Route testRoute = new Route("Lake District Test Route");

            WayPoint keswick = allWayPoints.FindByNameOrCode("Keswick");
            WayPoint windermere = allWayPoints.FindByNameOrCode("Windermere");
            WayPoint coniston = allWayPoints.FindByNameOrCode("Coniston");

            if (ambleside != null)
            {
                testRoute.AddEnd(ambleside);
            }

            if (keswick != null)
            {
                testRoute.AddEnd(keswick);
            }

            if (windermere != null)
            {
                testRoute.InsertAtPosition(windermere, 2);
            }

            if (coniston != null)
            {
                testRoute.AddFront(coniston);
            }

            Console.WriteLine("Route after AddFront, AddEnd and InsertAtPosition:");
            testRoute.DisplayRoute();
            Console.WriteLine();

            Console.WriteLine("Route after removing Windermere:");
            testRoute.RemoveWayPoint("Windermere");
            testRoute.DisplayRoute();
            Console.WriteLine();

            Console.WriteLine("Route after reversing:");
            testRoute.ReverseRoute();
            testRoute.DisplayRoute();
            Console.WriteLine();

            allRoutes.AddRoute(testRoute);
            activeRoute = testRoute;

            Route shortRoute = new Route("Second Short Route");
            if (ambleside != null)
            {
                shortRoute.AddEnd(ambleside);
            }
            if (windermere != null)
            {
                shortRoute.AddEnd(windermere);
            }
            allRoutes.AddRoute(shortRoute);

            Console.WriteLine("Test 7: routes stored in RouteArray");
            allRoutes.DisplayRoutes();

            Console.WriteLine("--- End of tests ---");
            Console.WriteLine();
            Console.WriteLine("Press Enter to open the menu.");
            Console.ReadLine();
        }

        private static void MenuLoop()
        {
            bool finished = false;

            while (!finished)
            {
                Console.Clear();
                Console.WriteLine("GPS Route Creation Tool");
                Console.WriteLine("Loaded waypoints: " + allWayPoints.Count);
                Console.WriteLine("Active route: " + GetActiveRouteName());
                Console.WriteLine();
                Console.WriteLine("1. Display all waypoints");
                Console.WriteLine("2. Search waypoint by name or code");
                Console.WriteLine("3. Search by first letters of name");
                Console.WriteLine("4. Search waypoints under a height");
                Console.WriteLine("5. Create a new route");
                Console.WriteLine("6. Switch active route");
                Console.WriteLine("7. Add waypoint to front of route");
                Console.WriteLine("8. Add waypoint to end of route");
                Console.WriteLine("9. Insert waypoint at a position");
                Console.WriteLine("10. Remove waypoint from route");
                Console.WriteLine("11. Display active route");
                Console.WriteLine("12. Reverse active route");
                Console.WriteLine("13. Display all route names");
                Console.WriteLine("14. Search waypoint in binary search tree");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        allWayPoints.DisplayAllWayPoints();
                        Pause();
                        break;
                    case "2":
                        SearchFullNameOrCode();
                        Pause();
                        break;
                    case "3":
                        SearchPartialName();
                        Pause();
                        break;
                    case "4":
                        SearchUnderHeight();
                        Pause();
                        break;
                    case "5":
                        CreateNewRoute();
                        Pause();
                        break;
                    case "6":
                        SwitchActiveRoute();
                        Pause();
                        break;
                    case "7":
                        AddWayPointToFront();
                        Pause();
                        break;
                    case "8":
                        AddWayPointToEnd();
                        Pause();
                        break;
                    case "9":
                        InsertWayPointIntoRoute();
                        Pause();
                        break;
                    case "10":
                        RemoveWayPointFromRoute();
                        Pause();
                        break;
                    case "11":
                        DisplayActiveRoute();
                        Pause();
                        break;
                    case "12":
                        ReverseActiveRoute();
                        Pause();
                        break;
                    case "13":
                        allRoutes.DisplayRoutes();
                        Pause();
                        break;
                    case "14":
                        SearchTreeByName();
                        Pause();
                        break;
                    case "0":
                        finished = true;
                        break;
                    default:
                        Console.WriteLine("Please choose one of the menu numbers.");
                        Pause();
                        break;
                }
            }
        }

        private static string GetActiveRouteName()
        {
            if (activeRoute == null)
            {
                return "none";
            }

            return activeRoute.RouteName;
        }

        private static void SearchFullNameOrCode()
        {
            Console.Write("Enter waypoint name or code: ");
            string searchText = Console.ReadLine();
            WayPoint result = allWayPoints.FindByNameOrCode(searchText);
            DisplaySearchResult(result);
        }


        private static void SearchTreeByName()
        {
            Console.Write("Enter waypoint full name for tree search: ");
            string searchText = Console.ReadLine();
            WayPoint result = allWayPointsTree.Find(searchText);
            DisplaySearchResult(result);
        }

        private static void SearchPartialName()
        {
            Console.Write("Enter the first letters of the waypoint name: ");
            string firstLetters = Console.ReadLine();
            allWayPoints.DisplayPartialNameMatches(firstLetters);
        }

        private static void SearchUnderHeight()
        {
            Console.Write("Display waypoints under this height in metres: ");
            int height = ReadInt();
            allWayPoints.DisplayWayPointsUnderHeight(height);
        }

        private static void CreateNewRoute()
        {
            Console.Write("Enter route name: ");
            string routeName = Console.ReadLine();

            Route newRoute = new Route(routeName);

            if (allRoutes.AddRoute(newRoute))
            {
                activeRoute = newRoute;
                Console.WriteLine("Route created and selected.");
            }
            else
            {
                Console.WriteLine("Route could not be added because the route array is full.");
            }
        }

        private static void SwitchActiveRoute()
        {
            allRoutes.DisplayRoutes();

            if (allRoutes.Count == 0)
            {
                return;
            }

            Console.Write("Enter route number: ");
            int routeNumber = ReadInt();

            Route selectedRoute = allRoutes.GetRouteAt(routeNumber - 1);

            if (selectedRoute == null)
            {
                Console.WriteLine("That route number was not found.");
            }
            else
            {
                activeRoute = selectedRoute;
                Console.WriteLine("Active route is now: " + activeRoute.RouteName);
            }
        }

        private static void AddWayPointToFront()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            WayPoint wayPoint = AskForWayPoint();

            if (wayPoint != null)
            {
                activeRoute.AddFront(wayPoint);
                Console.WriteLine("Waypoint added to the front of the route.");
            }
        }

        private static void AddWayPointToEnd()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            WayPoint wayPoint = AskForWayPoint();

            if (wayPoint != null)
            {
                activeRoute.AddEnd(wayPoint);
                Console.WriteLine("Waypoint added to the end of the route.");
            }
        }

        private static void InsertWayPointIntoRoute()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            WayPoint wayPoint = AskForWayPoint();

            if (wayPoint != null)
            {
                Console.Write("Enter position. 1 means the front: ");
                int position = ReadInt();
                activeRoute.InsertAtPosition(wayPoint, position);
                Console.WriteLine("Waypoint inserted into the route.");
            }
        }

        private static void RemoveWayPointFromRoute()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            Console.Write("Enter waypoint name or code to remove: ");
            string searchText = Console.ReadLine();

            if (activeRoute.RemoveWayPoint(searchText))
            {
                Console.WriteLine("Waypoint removed from route.");
            }
            else
            {
                Console.WriteLine("Waypoint was not found in the route.");
            }
        }

        private static void DisplayActiveRoute()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            activeRoute.DisplayRoute();
        }

        private static void ReverseActiveRoute()
        {
            if (!CheckActiveRoute())
            {
                return;
            }

            activeRoute.ReverseRoute();
            Console.WriteLine("Active route has been reversed.");
            activeRoute.DisplayRoute();
        }

        private static WayPoint AskForWayPoint()
        {
            Console.Write("Enter waypoint full name or code: ");
            string searchText = Console.ReadLine();
            WayPoint wayPoint = allWayPoints.FindByNameOrCode(searchText);

            if (wayPoint == null)
            {
                Console.WriteLine("Waypoint was not found in the array.");
            }

            return wayPoint;
        }

        private static bool CheckActiveRoute()
        {
            if (activeRoute == null)
            {
                Console.WriteLine("Create or select a route first.");
                return false;
            }

            return true;
        }

        private static int ReadInt()
        {
            int number;
            string input = Console.ReadLine();

            while (!int.TryParse(input, out number))
            {
                Console.Write("Enter a whole number: ");
                input = Console.ReadLine();
            }

            return number;
        }

        private static void DisplaySearchResult(WayPoint result)
        {
            if (result == null)
            {
                Console.WriteLine("Waypoint was not found.");
            }
            else
            {
                result.DisplayWayPoint();
            }
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
    }
}
