using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    public int Height = 11;
    public int Width = 11;

    RoomGenerator roomGenerator = new RoomGenerator();
    public MazeGeneratorCell[,] GenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[Height, Width];


        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        HideUnnecessaryCells(maze);

        MarkSideCells(maze);

        RemoveWallsWithBactracking(maze);

        RandomizeWalls(maze);

        MarkRoomCells(maze);

        return maze;
    }

    private void RemoveWallsWithBactracking(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Height - 2 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Width - 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell current, MazeGeneratorCell chosen)
    {
        if (current.X == chosen.X)
        {
            if (current.Y > chosen.Y) current.WallBottom = false;
            else chosen.WallBottom = false; 
        }
        else
        {
            if (current.X > chosen.X) current.WallLeft = false;
            else chosen.WallLeft = false;
        }
    }

    private void HideUnnecessaryCells(MazeGeneratorCell[,] maze)
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, Width - 1].Ceiling = false;
            maze[x, Width - 1].WallLeft = false;
            maze[x, Width - 1].Floor = false;
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[Height - 1, y].Ceiling = false;
            maze[Height - 1, y].WallBottom = false;
            maze[Height - 1, y].Floor = false;
        }
    }

    private void MarkSideCells(MazeGeneratorCell[,] maze)
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, 0].SideCell = true;
        }
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, Width - 1].SideCell = true;
            maze[x, Width - 2].SideCell = true;
        }
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[0, y].SideCell = true;
        }
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[Height - 1, y].SideCell = true;
            maze[Height - 2, y].SideCell = true;
        }
    }

    private void RandomizeWalls(MazeGeneratorCell[,] maze)
    {
        for (int x = 1; x < maze.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < maze.GetLength(1) - 1; y++)
            {
                if (RandomBool())
                {
                    maze[x, y].WallBottom = false;
                }
                if (RandomBool())
                {
                    maze[x, y].WallLeft = false;
                }
            }
        }
    }

    private void MarkRoomCells(MazeGeneratorCell[,] maze)
    {
        int maxRooms = GetMaxRooms(Height, Width);
        int minRooms = GetMinRooms(Height, Width);

        Room room;

        int roomsLimit = Random.Range(minRooms, maxRooms);
        for (int i = 0; i < roomsLimit; i++)
        {
            room = roomGenerator.GetRandomRoom();
            int roomHeight = room.Height;
            int roomWidth = room.Width;


            int randomWight = Random.Range(0, Width-1);
            int randomHeight = Random.Range(0, Height-1);
            //Debug.Log("Tried" + randomHeight * 3 + randomWight * 3);
            if (maze[randomHeight,randomWight].SideCell == false && IsRoomsAround(maze,maze[randomHeight,randomWight],roomHeight, roomWidth))
            {
                List<MazeGeneratorCell> currentSideCells = new List<MazeGeneratorCell>();

                //Debug.Log("Worked" + randomHeight *3 + randomWight *3);
                maze[randomHeight, randomWight].Floor = false;

                bool iterated = false;
                Vector2 first = new Vector2(0, 0);
                Vector2 last = new Vector2(0, 0);
                for (int x = randomHeight - roomHeight; x <= randomHeight + roomHeight; x++)
                {
                    for (int y = randomWight - roomWidth; y <= randomWight + roomWidth; y++)
                    {
                        if (x < Height && y < Width && x >= 0 && y >= 0 && maze[x,y].SideCell == false)
                        {
                            if (iterated == false)
                            {
                                iterated = true;
                                first.x = maze[x, y].X;
                                first.y = maze[x, y].Y;
                            }
                            maze[x, y].WallBottom = false;
                            maze[x, y].WallLeft = false;
                            last.x = maze[x, y].X;
                            last.y = maze[x, y].Y;
                        }
                    }
                }
                first.x += 1;
                first.y += 1;

                for (int x = (int)first.x; x <= (int)last.x; x++)
                {
                    currentSideCells.Add(maze[x, (int)first.y]);
                    maze[x, (int)first.y].RoomSide = true;
                    maze[x, (int)first.y].WallBottom = true;
                    maze[x, (int)first.y].WallLeft = false;
                }

                for (int y = (int)first.y; y <= (int)last.y; y++)
                {
                    currentSideCells.Add(maze[(int)first.x, y]);
                    maze[(int)first.x, y].RoomSide = true;
                    if (y > (int)first.y)
                    {
                        maze[(int)first.x, y].WallBottom = false;
                        maze[(int)first.x, y].WallLeft = true;
                    }
                    else
                    {
                        maze[(int)first.x, y].WallLeft = true;
                    }
                }

                for (int x = (int)last.x; x >= (int)first.x ; x--)
                {
                    currentSideCells.Add(maze[x, (int)last.y]);
                    maze[x, (int)last.y].RoomSide = true;
                    maze[x, (int)last.y].WallBottom = true;
                    maze[x, (int)last.y].WallLeft = false;
                }

                for (int y = (int)last.y; y >= (int)first.y; y--)
                {
                    currentSideCells.Add(maze[(int)last.x, y]);
                    maze[(int)last.x, y].RoomSide = true;
                    if (y == (int)last.y)
                    {
                        maze[(int)last.x, y].WallBottom = false;
                    }
                    else
                    {
                        maze[(int)last.x, y].WallBottom = false;
                        maze[(int)last.x, y].WallLeft = true;
                    }
                }

                currentSideCells.Remove(maze[(int)last.x, (int)last.y]);

                MazeGeneratorCell enter = currentSideCells[Random.Range(0,currentSideCells.Count)];
                enter.WallLeft = false;
                enter.WallBottom = false;
                enter.Floor = false;;
            }
        }
    }

    private bool IsRoomsAround(MazeGeneratorCell[,] maze, MazeGeneratorCell center, int height, int width)
    {
        for (int x = center.X - height; x <= center.X + height; x++)
        {
            for (int y = center.Y - width; y <= center.Y + width; y++)
            {
                if (x < Height && y < Width && x >= 0 && y >= 0)
                {
                    if (maze[x, y].RoomSide == true)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private int GetMaxRooms(int height, int width)
    {
        return (height + width) / 8;
    }
    private int GetMinRooms(int height, int width)
    {
        return (height + width) / 12;
    }

    private bool RandomBool()
    {
        System.Random random = new System.Random();
        if (random.Next(100) <= 25)
        {
            return true;
        }
        return false;
    }

}
