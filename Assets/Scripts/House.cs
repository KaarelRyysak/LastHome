using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
	public static House instance;
	public Room roomPrefab;

	public int height, width;
	public int roomSize;
	public int startY, startX;
	Room[,] rooms;
	public Room StartRoom => rooms[startY, startX];
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

	public Room GetRandomRoom() {
		return rooms[Random.Range(0, height), Random.Range(0, width)];
	}

	public List<Room> GetClosedPath(Room from, Room to) {
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
