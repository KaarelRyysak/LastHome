using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	public int y, x;
    public List<Room> neighbors = new List<Room>();

    public Room(int y, int x) {
		this.y = y;
		this.x = x;
	}


    public void Connect(Room room)
    {
        neighbors.Add(room);
        
        //If the other room doesn't have this room, add it there too
        if (!room.neighbors.Contains(this))
        {
            room.Connect(this);
        }
    }
}
