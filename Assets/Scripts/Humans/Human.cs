﻿using System.Collections;
using UnityEngine;
public class Human : MonoBehaviour {
	enum State {
		Idle,
		Walking,
		Fleeing
	}

	public bool male;
	public bool dead = false;
	public bool falling = false;
	public bool onFire = false;
	const float fallingSpeed = 10f;

	public GameObject brokenHeart;
	Room currentRoom;
	House house;
	Trust trust;
	State state = State.Idle;
	SpriteRenderer headRenderer, bodyRenderer;

	public float speed, patience;
	public float trustGainPerDoor, trustLossPerSecondWaiting, trustLossPerCorpse, trustLossPerActiveTrap;
	public int idleStrolls;

	void Awake() {
		headRenderer = GetComponent<SpriteRenderer>();
		bodyRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	void Start() {
		house = House.instance;
		trust = Trust.instance;
		currentRoom = house.StartRoom;
		currentRoom.humans.Add(this);
		OnEnterRoom();

		StartCoroutine(AI());
	}

	public void UpdateRoom(Room room) {
		currentRoom.humans.Remove(this);
		currentRoom = room;
		currentRoom.humans.Add(this);
		OnEnterRoom();
	}

	void OnEnterRoom() {
		if (dead) {
			return;
		}

		//Check for dead humans
		foreach (Human human in currentRoom.humans) {
			if (human.dead) {
				Instantiate(brokenHeart, human.transform.position, Quaternion.identity);
				trust.Value -= trustLossPerCorpse;
				Repulse();
			}
		}

		//Check for active traps
		foreach (BaseTrap trap in currentRoom.traps) {
			if (trap.Activated) {
				Instantiate(brokenHeart, trap.transform.position, Quaternion.identity);
				trust.Value -= trustLossPerActiveTrap;
				Repulse();
			}
		}
	}

	public void Attract(Room room) {
		if (dead) {
			return;
		}

		if (state != State.Fleeing) {
			state = State.Walking;
			StopAllCoroutines();
			StartCoroutine(MoveTowards(room));
		}
	}

	public void Repulse() {
		if (dead) {
			return;
		}

		if (state != State.Fleeing) {
			state = State.Fleeing;
			StopAllCoroutines();
			StartCoroutine(Flee());
		}
	}

	IEnumerator AI() {
		while (true) {
			state = State.Idle;
			yield return Idle();
			state = State.Walking;
			yield return PathToRoom(house.RandomRoom);
		}
	}

	IEnumerator MoveTowards(Room room) {
		yield return PathToRoom(room);
		StartCoroutine(AI());
	}

	IEnumerator Flee() {
        AudioPlayer.instance.negGroup.Play();

		Room target = house.RandomRoom;
		while (target == currentRoom) {
			target = house.RandomRoom;
		}
		speed *= 2;
		yield return PathToRoom(target, house.GetPath(currentRoom, target, true) != null);
		speed /= 2;
		StartCoroutine(AI());
	}

	IEnumerator Idle() {
		for (int i = 0; i < idleStrolls; i++) {
			Vector3 pos = currentRoom.transform.position;
			pos = new Vector3(Random.Range(pos.x - 4, pos.x + 4), Random.Range(pos.y - 4, pos.y + 4));
			yield return LerpMove(transform.position, pos);
			yield return new WaitForSeconds(Random.Range(0f, 3f));

    
            AudioPlayer.instance.neutGroup.Play();
		}
	}

	IEnumerator PathToRoom(Room target, bool respectDoors = false) {
		//Extend path to include standing before and after doors
		Vector3 location = transform.position;
		foreach (Room room in house.GetPath(currentRoom, target, respectDoors)) {
			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.4f));

			//Only proceed if door is open
			Door door = house.roomsToDoor[new Pair<Room>(currentRoom, room)];
			float waitEnd = Time.time + patience;

			while (!door.open && Time.time < waitEnd) {
				trust.Value -= trustLossPerSecondWaiting * Time.deltaTime;
				yield return null;
			}
			if (!door.open) { //Out of patience
				Instantiate(brokenHeart, transform.position, Quaternion.identity);
				yield break;
			} else {
				trust.Value += trustGainPerDoor;

                AudioPlayer.instance.posGroup.Play();
			}

			yield return LerpMove(transform.position, Vector3.Lerp(currentRoom.transform.position, room.transform.position, 0.6f));
			UpdateRoom(room);
		}
	}

	IEnumerator LerpMove(Vector2 from, Vector2 to) {
		Vector2 direction = to - from;
		headRenderer.flipX = direction.x < 0;
		bodyRenderer.flipX = headRenderer.flipX;
		float duration = direction.magnitude / speed;
		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime) {
			yield return null;
			transform.position = Vector2.Lerp(from, to, (Time.time - startTime) / duration);
		}
	}

	public void Die() {
		if (dead) {
			return;
		}

		if (male) {
            AudioPlayer.instance.maleDeathGroup.Play();
		} else {
			AudioPlayer.instance.femaleDeathGroup.Play();
		}

		dead = true;
		foreach (Human human in currentRoom.humans) {
			if (human != this && human != dead) {
				human.Repulse();
				Instantiate(brokenHeart, human.transform.position, Quaternion.identity);
				trust.Value -= trustLossPerCorpse;
			}
		}

		Kills.instance.Value++;
		SpawnManager.instance.numOfAlive--;
		StopAllCoroutines();

		gameObject.transform.Rotate(new Vector3(0, 0, 90));

	}

	public IEnumerator Fall(GameObject pitTrap) {
		gameObject.transform.parent = pitTrap.transform;
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
		currentRoom.humans.Remove(this);
		Destroy(gameObject);
	}

	public IEnumerator OnFire(GameObject firePrefab, GameObject ashPrefab) {
		onFire = true;

		if (male) {
			AudioPlayer.instance.maleDeathGroup.Play();
		} else {
			AudioPlayer.instance.femaleDeathGroup.Play();
		}

		dead = true;
		foreach (Human human in currentRoom.humans) {
			if (human != this && human != dead) {
				Instantiate(brokenHeart, human.transform.position, Quaternion.identity);
				human.Repulse();
				trust.Value -= trustLossPerCorpse;
			}
		}

		SpawnManager.instance.numOfAlive--;

		GameObject fire = Instantiate(firePrefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
		fire.transform.Rotate(0, 0, -90);
		yield return new WaitForSeconds(Random.Range(3f, 4f));
		GameObject ash = Instantiate(ashPrefab, gameObject.transform.position, gameObject.transform.rotation);
		ash.transform.Rotate(0, 0, -90);
		Destroy(gameObject);
	}
}