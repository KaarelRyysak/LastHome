﻿using System.Collections;
using UnityEngine;

public class Human : MonoBehaviour {
	enum State {
		Idle,
		Walking
	}

	public bool male;
	bool dead = false;
	bool hiddenBody = false;
	public bool falling = false;
	float fallingSpeed = 10f;
	public GameObject pitTrap;


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
			Debug.Log("Stoppd idling");
			yield return PathToRoom();
			Debug.Log("Stoppd pathing");
		}
	}

	IEnumerator Idle() {
		for (int i = 0; i < idleStrolls; i++) {
			Vector3 pos = currentRoom.transform.position;
			pos = new Vector3(Random.Range(pos.x - 4, pos.x + 4), Random.Range(pos.y - 4, pos.y + 4));
			yield return LerpMove(transform.position, pos);
			yield return new WaitForSeconds(Random.Range(0, 3));
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

			//Kui pit trap-i pole kasutusel
			if (pitTrap == null) {
				gameObject.transform.Rotate(new Vector3(0, 0, 90));
				Destroy(this);
			}

		}
	}

	public void Update() {
		if (falling) {
			gameObject.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
			gameObject.transform.Rotate(Vector3.forward * 0.1f * Time.deltaTime);

			if (!hiddenBody && gameObject.transform.position.y <= pitTrap.transform.position.y) {
				gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}
			if (gameObject.transform.localPosition.y < -0.988f) {
				Destroy(gameObject);
			}
		}
	}
}
