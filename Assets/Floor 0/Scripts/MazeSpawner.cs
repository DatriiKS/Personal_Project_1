using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> WallsLeft = new List<GameObject>();
    [SerializeField] List<GameObject> WallsBottom = new List<GameObject>();
    [SerializeField] List<float> persentages = new List<float>();

    [SerializeField]
    private GameObject CellPrefab;

    [SerializeField]
    private GameObject WallLeftDefault;

    [SerializeField]
    private GameObject WallBottomDefault;

    [SerializeField]
    private Vector3 cellSize = new Vector3(1, 1, 0);


    [ContextMenu("SpawnMaze")]
    void SpawnMaze()
    {
        MazeGenerator generator = new MazeGenerator();
        MazeGeneratorCell[,] maze = generator.GenerateMaze();
        List<Cell> roomSides = new List<Cell>();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                Cell cell = Instantiate(CellPrefab, new Vector3(x * cellSize.x, y * cellSize.y, y * cellSize.z), Quaternion.identity).GetComponent<Cell>();

                SetupWallBottom(cell, maze, x, y);
                SetupWallLeft(cell, maze, x, y);

                cell.ceiling.SetActive(maze[x, y].Ceiling);
                cell.floor.SetActive(maze[x, y].Floor);
            }
        }

    }
    #region DEFAULTS
    //private void SetupDefaultWallLeft(Cell cell)
    //{
    //    cell.wallLeft = WallLeftDefault;
    //    cell.wallLeft.SetActive(false);
    //    Instantiate(cell.wallLeft, cell.transform);
    //}
    //private void SetupDefaultWallBottom(Cell cell)
    //{
    //    cell.wallBottom = WallBottomDefault;
    //    cell.wallBottom.SetActive(false);
    //    Instantiate(cell.wallBottom, cell.transform);
    //}
    #endregion
    private void SetupWallLeft(Cell cell, MazeGeneratorCell[,] maze, int x, int y)
    {
        if (maze[x,y].SideCell || maze[x, y].RoomSide)
        {
            cell.wallLeft = WallLeftDefault;
        }
        else
        {
            cell.wallLeft = GetRandomWallFromList(WallsLeft);
        }
        cell.wallLeft.SetActive(maze[x, y].WallLeft);
        Instantiate(cell.wallLeft, cell.transform);
    }

    private void SetupWallBottom(Cell cell, MazeGeneratorCell[,] maze, int x, int y)
    {
        if (maze[x, y].SideCell || maze[x, y].RoomSide)
        {
            cell.wallBottom = WallBottomDefault;
        }
        else
        {
            cell.wallBottom = GetRandomWallFromList(WallsBottom);
        }
        cell.wallBottom.SetActive(maze[x, y].WallBottom);
        Instantiate(cell.wallBottom, cell.transform);
    }

    private GameObject GetRandomWallFromList(List<GameObject> walls)
    {
        float random = Random.Range(0f, 1f);
        Debug.Log(random);
        float numToAdd = 0;

        float total = persentages.Sum();
        Debug.Log(total);
        for (int i = 0; i < walls.Count; i++)
        {
            Debug.Log(persentages[i]);
            if (persentages[i] / total + numToAdd >= random)
            {
                return walls[i];
            }
            else
            {
                numToAdd += persentages[i] / total;
            }
        }

        return walls[0];
    }
}
