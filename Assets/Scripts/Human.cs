using System.Collections;
using UnityEngine;

public class Human : MonoBehaviour {
	enum State {
		Idle,
		Walking
	}

	Room currentRoom;
	House house;
	Trust trust;
	State state = State.Idle;

	public float speed, patience;
	public GameObject corpse;

	void Start() {
		house = House.instance;
		trust = Trust.instance;
		currentRoom = house.StartRoom;

		StartCoroutine(AI());
	}

	IEnumerator AI() {
		while (state == State.Idle) {
			yield return new WaitForSeconds(3);
			state = State.Walking;
			yield return StartCoroutine(PathToRoom());
			state = State.Idle;
		}
	}

	IEnumerator PathToRoom() {
		Room target = house.RandomRoom;

		//Extend path to include standing before and after doors
		Vector3 location = transform.position;
		foreach (Room room in house.GetPath(currentRoom, target, false)) {
			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.4f));

			//Only proceed if door is open
			Door door = house.roomsToDoor[new Pair<Room>(currentRoom, room)];
			float waitEnd = Time.time + patience;

			while (!door.open && Time.time < waitEnd) {
				trust.Value -= 10 * Time.deltaTime;
				yield return null;
			}
			if (!door.open) { //Out of patience
				yield break;
			} else {
				trust.Value += 1;
			}

			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.6f));
			currentRoom = room;
		}
	}

	IEnumerator LerpMove(Vector2 from, Vector2 to) {
		Vector2 direction = to - from;
		float duration = direction.magnitude / speed;
		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime) {
			yield return null;
			transform.position = Vector2.Lerp(from, to, (Time.time - startTime) / duration);
		}
	}

	public void Die() {
		Instantiate(corpse, gameObject.transform.position, corpse.transform.rotation);

		SpawnManager.instance.numOfAlive -= 1;

		Destroy(gameObject);
	}
}
