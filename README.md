# GPS Route Creation Tool

This project is a C# console application for loading UK waypoint records from a CSV file and using them to create and edit routes. A waypoint stores details such as its name, code, latitude, longitude, elevation and description. Routes are built as ordered lists of selected waypoints, so the program can be used to add, remove, search and display route points in a clear way.

## Project structure

The main source code is in the `code` folder. The waypoint CSV file is in the `data` folder.

Important files:

- `Program.cs` controls the menu, reads the waypoint file and connects the main classes together.
- `WayPoint.cs` stores one waypoint record.
- `WayPointArray.cs` stores all loaded waypoint objects in a fixed-size array.
- `Link.cs` stores one linked-list node for a route.
- `Route.cs` stores and edits one route as a linked list.
- `RouteArray.cs` stores the routes created by the user.
- `TreeNode.cs` stores one node in the binary search tree.
- `WayPointTree.cs` stores waypoint names in a binary search tree for searching and duplicate checking.
- `SimpleUnitTests.cs` contains simple checks for the main data structures.

## Data structures used

The program uses an array, a linked list and a binary search tree because each one suits a different part of the problem.

`WayPointArray` is used for the full waypoint list. This works well because the waypoints are loaded from a file and then stored in the next free array position. The class keeps track of how many waypoints have been added, so the program can display and search the used part of the array.

`Route` uses a linked list made from `Link` objects. This fits route editing because a route is an ordered list of waypoints. The route can add a waypoint at the start, add one at the end, insert at a chosen position, remove a waypoint and reverse the route. A `last` pointer is also used so adding to the end of a route does not need to walk through the whole list every time.

`WayPointTree` is a binary search tree ordered by waypoint name. It supports name searching, duplicate name checking and in-order display. The tree uses recursive insertion and search, with smaller names going to the left and larger names going to the right.

## Main features

When the program runs, it loads the waypoint file and then shows a menu. The menu allows the user to:

- display all loaded waypoints;
- search for a waypoint by name or code;
- search using the first letters of a waypoint name;
- display waypoints below a chosen elevation;
- create and switch between routes;
- add waypoints to the front or end of a route;
- insert or remove waypoints in a route;
- reverse and display a route;
- search for a waypoint name using the binary search tree;
- run simple tests for the main structures.

## Running the project

Open `code/WayPointRouteTool.sln` in Visual Studio. Make sure the file `data/UK_waypoints.csv` stays in the submission folder, then build and run the project.

The project was written as a console program, so all interaction happens through the numbered menu. The data file should be kept with the project because the program needs it when loading the waypoint records.

## Complexity note

The report analyses the `ReadFileWayPoints` method. This method reads the CSV file, splits each line, creates a `WayPoint`, inserts the waypoint into the binary search tree and then stores accepted waypoints in the array. In the usual case, insertion into the tree gives an overall complexity of `O(n log n)`. In the worst case, if the tree becomes unbalanced, insertion can degrade to `O(n²)`.

## Testing

`SimpleUnitTests.cs` includes basic console tests for the main structures. These tests check that waypoints can be added, routes can be edited and the binary search tree can find stored waypoints. They are simple tests, but they give a quick way to confirm that the main features still work after changes.
