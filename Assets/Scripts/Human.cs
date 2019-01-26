using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
	enum State {
		Idle,
		Walking
	}

	Room currentRoom;
	House house;
	State state = State.Idle;

	public float speed;

	public GameObject corpse;

	void Start() {
		house = House.instance;
		currentRoom = house.StartRoom;

		StartCoroutine(AI());
	}

	IEnumerator AI() {
		while (state == State.Idle) {
			yield return new WaitForSeconds(10);
			state = State.Walking;
			yield return StartCoroutine(PathToRoom());
			state = State.Idle;
		}
	}

	IEnumerator PathToRoom() {
		Room target = house.GetRandomRoom();
		List<Room> path = house.GetClosedPath(currentRoom, target);

		foreach (Room room in path) {
			yield return LerpMove(transform.position, room.transform.position);
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
		Instantiate(corpse, gameObject.transform.position, gameObject.transform.rotation);

		Destroy(gameObject);
	}
}
