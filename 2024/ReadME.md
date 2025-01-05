TODO:
Pathfinding Algos:
- Breadth-First Search (BFS)
- Depth-First Search (DFS)
- Dijkstra
- A* (A-Stern)

Parser:
- Span vs array vs list
- Mem Alocation



ChatGPT Beispiel:
```csharp
   private int BfsFindShortestPath(Vector2 start, Vector2 end)
   {
       var queue = new Queue<(Vector2 pos, int distance)>();
       queue.Enqueue((start, 0));

       // visited[y, x] statt Map[y, x] manipulieren
       var visited = new bool[MapHeight, MapWidth];
       visited[(int)start.Y, (int)start.X] = true;

       while (queue.Count > 0)
       {
           var (pos, dist) = queue.Dequeue();
           var (x, y) = ((int)pos.X, (int)pos.Y);

           if (pos == end)
               return dist;

           foreach (var dir in new[] { Up, Right, Down, Left })
           {
               var nx = x + (int)dir.X;
               var ny = y + (int)dir.Y;

               if (nx >= 0 && nx < MapWidth && ny >= 0 && ny < MapHeight)
               {
                   // -1 heißt wohl Blockade
                   if (!visited[ny, nx] && Map[ny, nx] != -1)
                   {
                       visited[ny, nx] = true;
                       queue.Enqueue((new Vector2(nx, ny), dist + 1));
                   }
               }
           }
       }

       return -1; // kein Weg gefunden
   }
   ```