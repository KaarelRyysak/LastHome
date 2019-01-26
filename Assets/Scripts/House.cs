using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
	public static House instance;
	public Room roomPrefab;

	public int height, width;
	public int roomSize;
	public int startY, startX;
	Room[,] rooms;
    //Dictionary<List<Room>, Door>

	void Awake() {
		instance = this;
		rooms = new Room[height, width];
		for (int y = 0; y < startY; y++) {
			for (int x = 0; x < startX; x++) {
				rooms[y, x] = Instantiate(roomPrefab, new Vector3(x * roomSize, y * roomSize), Quaternion.identity);
			}
		}
	}

	public Room StartRoom => rooms[startY, startX];

	public List<Vector3> GetPath(Room current, Room other) {
		return null;
	}

	public void SetDoorOpen(/*Door door, */bool open) {

	}
}
