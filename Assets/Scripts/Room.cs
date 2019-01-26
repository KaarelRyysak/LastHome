using System.Collections.Generic;

public class Room {
	public int y, x;
    private List<Room> connected;

	public Room(int y, int x) {
		this.y = y;
		this.x = x;
	}

    public List<Room> GetConnected()
    {
        return connected;
    }

    public void Connect(Room room)
    {
        connected.Add(room);
        List<Room> otherConnected = room.GetConnected();
        
        //If the other room doesn't have this room, add it there too
        if (!otherConnected.Contains(this))
        {
            room.Connect(this);
        }
    }
}
