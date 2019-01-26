using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public static SpawnManager instance;
	public Human humanPrefab;
	public List<Wave> waves;
	private float lastSpawn;
	public int numOfAlive;
	private int waveIndex;
	private bool waveSpawned;

	private void Awake() {
		instance = this;

		lastSpawn = Time.time;

		waveIndex = 0;

		numOfAlive = 0;
	}

	// Update is called once per frame
	void Update() {
		if (!waveSpawned) {
			if (waveIndex < waves.Count && Time.time > lastSpawn + waves[waveIndex].Cooldown) {
				lastSpawn = Time.time;
				waves[waveIndex].Count -= 1;
				if (waves[waveIndex].Count >= 0) {
					Instantiate(humanPrefab, gameObject.transform.position, gameObject.transform.rotation);
					numOfAlive += 1;
				} else {
					waveSpawned = true;
				}
			}
		} else if (waveIndex < waves.Count && numOfAlive == 0) {
			//Start new wave
			waveIndex ++;
			waveSpawned = false;
		}
	}

}
