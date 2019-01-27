using UnityEngine;

public abstract class BaseTrap : MonoBehaviour {
	bool activated = false;
	public bool Activated {
		get {
			return activated;
		} set {
			activated = value;
			if (value) {
				foreach (Human human in room.humans) {
					if (human.dead) {
						continue;
					}
					human.Repulse();
					trust.Value -= human.trustLossPerActiveTrap;
				}
			}
		}
	}
	Room room;
	Trust trust;
	public Vector2Int roomCoords;

	void Start() {
		trust = Trust.instance;
		room = House.instance.rooms[roomCoords.y, roomCoords.x];
	}
}
