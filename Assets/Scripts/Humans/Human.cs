using System.Collections;
using UnityEngine;

public class Human : MonoBehaviour {
	enum State {
		Idle,
		Walking
	}

	public bool male;
	bool dead = false;
	public bool falling = false;
	const float fallingSpeed = 10f;


	Room currentRoom;
	House house;
	Trust trust;
	State state = State.Idle;

	public float speed, patience;
	public float trustGainPerDoor, trustLossPerSecondWaiting;
	public int idleStrolls;

	void Start() {
		house = House.instance;
		trust = Trust.instance;
		currentRoom = house.StartRoom;

		StartCoroutine(AI());
	}

	IEnumerator AI() {
		while (true) {
			yield return Idle();
			yield return PathToRoom();
		}
	}

	IEnumerator Idle() {
		for (int i = 0; i < idleStrolls; i++) {
			Vector3 pos = currentRoom.transform.position;
			pos = new Vector3(Random.Range(pos.x - 4, pos.x + 4), Random.Range(pos.y - 4, pos.y + 4));
			yield return LerpMove(transform.position, pos);
			yield return new WaitForSeconds(Random.Range(0f, 3f));
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
				trust.Value -= trustLossPerSecondWaiting * Time.deltaTime;
				yield return null;
			}
			if (!door.open) { //Out of patience
				yield break;
			} else {
				trust.Value += trustGainPerDoor;
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
		if (!dead) {
			dead = true;

			SpawnManager.instance.numOfAlive -= 1;
			StopAllCoroutines();

			gameObject.transform.Rotate(new Vector3(0, 0, 90));
		}
	}

	public IEnumerator Fall(GameObject pitTrap) {
		falling = true;
		while (gameObject.transform.position.y > pitTrap.transform.position.y) {
			gameObject.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
			gameObject.transform.Rotate(Vector3.forward * 0.1f * Time.deltaTime);
			yield return null;
		}
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		while (gameObject.transform.localPosition.y >= -0.988f) {
			gameObject.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
			gameObject.transform.Rotate(Vector3.forward * 0.1f * Time.deltaTime);
			yield return null;
		}
		Destroy(gameObject);
	}
}
