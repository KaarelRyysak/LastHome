using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
	public static House instance;
	public Room roomPrefab;
	public Door verticalDoorPrefab, horizontalDoorPrefab;
	public GameObject verticalWallPrefab, horizontalWallPrefab;

	public int height, width;
	public int roomSize;
	public int startY, startX;
	Room[,] rooms;
	public Dictionary<Pair<Room>, Door> roomsToDoor = new Dictionary<Pair<Room>, Door>();
	public Dictionary<Door, Pair<Room>> doorToRooms = new Dictionary<Door, Pair<Room>>();

	public Room StartRoom => rooms[startY, startX];
	public Room RandomRoom => rooms[Random.Range(0, height), Random.Range(0, width)];
	public Vector2Int[] doors; //(x, y) coordinates of rooms. 2 consecutive define a door
	public Vector2Int[] walls; //(x, y) coordinates of rooms. 2 consecutive define a door

	void Awake() {
		instance = this;

		//Instantiate rooms
		rooms = new Room[height, width];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				rooms[y, x] = Instantiate(roomPrefab, new Vector3(x * roomSize, y * roomSize), Quaternion.identity);
			}
		}

		//Instantiate doors and room neighbors
		for (int i = 0; i < doors.Length / 2; i++) {
			Vector2Int roomLoc1 = doors[i * 2];
			Vector2Int roomLoc2 = doors[i * 2 + 1];
			Room room1 = rooms[roomLoc1.y, roomLoc1.x];
			Room room2 = rooms[roomLoc2.y, roomLoc2.x];

			room1.neighbors.Add(room2);
			room2.neighbors.Add(room1);

			Door door = Instantiate(roomLoc1.y == roomLoc2.y ? verticalDoorPrefab : horizontalDoorPrefab, (room1.transform.position + room2.transform.position) / 2, Quaternion.identity);
			doorToRooms[door] = new Pair<Room>(room1, room2);
			roomsToDoor[new Pair<Room>(room1, room2)] = door;
		}

		//Instantiate walls
		for (int i = 0; i < walls.Length / 2; i++) {
			Vector2Int roomLoc1 = walls[i * 2];
			Vector2Int roomLoc2 = walls[i * 2 + 1];
			Room room1 = rooms[roomLoc1.y, roomLoc1.x];
			Room room2 = rooms[roomLoc2.y, roomLoc2.x];

			Instantiate(roomLoc1.y == roomLoc2.y ? verticalWallPrefab : horizontalWallPrefab, (room1.transform.position + room2.transform.position) / 2, Quaternion.identity);
		}
	}

	public List<Room> GetPath(Room from, Room to, bool respectDoors) {
		List<Room> path = new List<Room>();

		if (from == to) {
			return path;
		}

		HashSet<Room> processed = new HashSet<Room>();
		List<Room> thisIteration = new List<Room>() { from };
		List<Room> nextIteration = new List<Room>();

		while (thisIteration.Count != 0) { //While there are more rooms to check
			for (int i = 0; i < thisIteration.Count; i++) { //Check all rooms that are at this amount of rooms (n) away
				Room current = thisIteration[i];

				foreach (Room adjacent in current.neighbors) { //Check all rooms that are n+1 rooms away
					if (respectDoors && !roomsToDoor[new Pair<Room>(current, adjacent)].open) {
						continue; //No pathing through closed doors when respecting doors
					}

					if (adjacent == to) {
						path.Add(adjacent);
						while (current != from) { //Follow the path back to start
							path.Add(current);
							current = current.previous;
						}
						path.Reverse();
						return path;
					}

					if (!processed.Contains(adjacent)) { //Add them to the to-check list if they have yet to be processed
						adjacent.previous = current;
						nextIteration.Add(adjacent);
						processed.Add(adjacent);
					}
				}

				processed.Add(current);
			}
			thisIteration = nextIteration; //Switch to the n+1-th iteration
			nextIteration = new List<Room>();
		}

		return null;
	}
}
