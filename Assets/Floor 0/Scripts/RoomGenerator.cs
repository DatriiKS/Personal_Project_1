
using System;

public class RoomGenerator 
{
    public Room GetRandomRoom()
    {
        Room room = new Room();
        Random random = new Random();

        room.Height = random.Next(2, 5);
        room.Width = random.Next(2, 5);

        return room;
    }
}
