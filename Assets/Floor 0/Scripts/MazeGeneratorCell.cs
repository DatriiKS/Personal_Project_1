using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool WallLeft { get; set; } = true;
    public bool WallBottom { get; set; } = true;
    public bool Floor { get; set; } = true;

    public bool Ceiling { get; set; } = true;

    public bool Visited { get; set; } = false;

    public bool SideCell { get; set; } = false;

    public bool RoomSide { get; set; } = false;

}
