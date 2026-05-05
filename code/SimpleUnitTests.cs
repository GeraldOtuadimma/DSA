using System;

namespace WaypointManager
{
    // Small console tests for the main data structures.
    class SimpleUnitTests
    {
        private static int testsRun;
        private static int testsPassed;

        public static void RunAll()
        {
            testsRun = 0;
            testsPassed = 0;

            Console.WriteLine("--- Simple unit tests ---");
            TestWayPointArrayStoresWayPoint();
            TestTreeRejectsDuplicateName();
            TestRouteAddEndUsesTailAndKeepsOrder();
            TestRouteRemoveFrontAndEnd();
            TestTreeFindByName();
            Console.WriteLine("Tests passed: " + testsPassed + "/" + testsRun);
            Console.WriteLine("--- End of unit tests ---");
            Console.WriteLine();
        }

        private static void TestWayPointArrayStoresWayPoint()
        {
            WayPointArray array = new WayPointArray(3);
            bool firstAdd = array.AddWayPoint(MakeWayPoint("Ambleside", "AMB"));

            Assert(firstAdd && array.Count == 1, "array stores a waypoint in the next free position");
        }

        private static void TestTreeRejectsDuplicateName()
        {
            WayPointTree tree = new WayPointTree();
            bool firstAdd = tree.InsertItem(MakeWayPoint("Ambleside", "AMB"));
            bool duplicateAdd = tree.InsertItem(MakeWayPoint("Ambleside", "AMB2"));

            Assert(firstAdd && !duplicateAdd && tree.Count == 1, "binary search tree rejects a duplicate waypoint name");
        }

        private static void TestRouteAddEndUsesTailAndKeepsOrder()
        {
            Route route = new Route("Test Route");
            route.AddEnd(MakeWayPoint("A", "A"));
            route.AddEnd(MakeWayPoint("B", "B"));
            route.AddEnd(MakeWayPoint("C", "C"));

            Assert(route.NumberOfItems() == 3 && route.IsPresent("C"), "route AddEnd keeps all waypoints in the linked list");
        }

        private static void TestRouteRemoveFrontAndEnd()
        {
            Route route = new Route("Remove Test");
            route.AddEnd(MakeWayPoint("A", "A"));
            route.AddEnd(MakeWayPoint("B", "B"));
            route.AddEnd(MakeWayPoint("C", "C"));

            bool removedFront = route.RemoveWayPoint("A");
            bool removedEnd = route.RemoveWayPoint("C");

            Assert(removedFront && removedEnd && route.NumberOfItems() == 1 && route.IsPresent("B"), "route removes front and end links correctly");
        }

        private static void TestTreeFindByName()
        {
            WayPointTree tree = new WayPointTree();
            tree.InsertItem(MakeWayPoint("Keswick", "KES"));
            tree.InsertItem(MakeWayPoint("Ambleside", "AMB"));
            tree.InsertItem(MakeWayPoint("Windermere", "WIN"));

            WayPoint found = tree.Find("Windermere");
            WayPoint missing = tree.Find("Not Real");

            Assert(found != null && found.Code == "WIN" && missing == null, "binary search tree finds by name");
        }

        private static WayPoint MakeWayPoint(string name, string code)
        {
            return new WayPoint(name, code, "0", "0", 0, "test waypoint");
        }

        private static void Assert(bool condition, string testName)
        {
            testsRun++;

            if (condition)
            {
                testsPassed++;
                Console.WriteLine("PASS: " + testName);
            }
            else
            {
                Console.WriteLine("FAIL: " + testName);
            }
        }
    }
}
