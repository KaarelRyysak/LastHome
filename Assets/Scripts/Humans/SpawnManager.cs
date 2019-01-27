using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public static SpawnManager instance;
	public Human humanPrefab;
	private float lastSpawn;
	public int numOfAlive;
    public int peopleSpawnedLastWave;
    public int peopleToSpawn = 0;
	private bool waveSpawned;
    public int cooldownBetweenSpawns;
    

	public List<Sprite> maleHeads;
	public List<Sprite> femaleHeads;
	public List<Sprite> bodies;

	private void Awake() {
		instance = this;

		lastSpawn = Time.time;

        waveSpawned = true;

        peopleSpawnedLastWave = 2;

		numOfAlive = 0;
	}

	void Update() {
		if (!waveSpawned) {
            if (Time.time > lastSpawn + cooldownBetweenSpawns)
            {
                lastSpawn = Time.time;
                peopleToSpawn--;
                if (peopleToSpawn >= 0)
                {
                    MakeHuman();
                }
                else
                {
                    waveSpawned = true;
                }
            }
		} else if (numOfAlive == 0) {
            //Start new wave
            peopleSpawnedLastWave += 2;
            peopleToSpawn = peopleSpawnedLastWave;
			waveSpawned = false;
		}


        //if (Input.GetKeyDown("1"))
        //{
        //    AudioPlayer.instance.clickGroup.Play();
        //}
        //if (Input.GetKeyDown("2"))
        //{
        //    AudioPlayer.instance.closeDoorGroup.Play();
        //}
        //if (Input.GetKeyDown("3"))
        //{
        //    AudioPlayer.instance.openDoorGroup.Play();
        //}
        //if (Input.GetKeyDown("4"))
        //{
        //    AudioPlayer.instance.femaleDeathGroup.Play();
        //}
        //if (Input.GetKeyDown("5"))
        //{
        //    AudioPlayer.instance.maleDeathGroup.Play();
        //}

    }
    

	private void MakeHuman() {
		Human human = Instantiate(humanPrefab, gameObject.transform.position, gameObject.transform.rotation);
		numOfAlive += 1;
		bool male = Mathf.Round(Random.Range(0, 1)) == 0;
		human.male = male;
		if (male) {
			human.GetComponent<SpriteRenderer>().sprite = maleHeads[Mathf.RoundToInt(Random.Range(-0.49f, maleHeads.Capacity - 0.51f))];
		} else {
			human.GetComponent<SpriteRenderer>().sprite = femaleHeads[Mathf.RoundToInt(Random.Range(-0.49f, femaleHeads.Capacity - 0.51f))];
		}
		human.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = bodies[Mathf.RoundToInt(Random.Range(-0.49f, bodies.Capacity - 0.51f))];
	}

}
