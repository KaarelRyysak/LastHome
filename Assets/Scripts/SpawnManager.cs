using System.Collections;
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

    public List<Sprite> maleHeads;
    public List<Sprite> femaleHeads;
    public List<Sprite> bodies;

	private void Awake() {
		instance = this;

		lastSpawn = Time.time;
        
        waveIndex = 0;

        numOfAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveSpawned)
        {
            if(waveIndex < waves.Count && Time.time > lastSpawn + waves[waveIndex].Cooldown)
            {
                lastSpawn = Time.time;
                waves[waveIndex].Count -= 1;
                if(waves[waveIndex].Count >= 0)
                {
                    MakeHuman();
                }
                else
                {
                    waveSpawned = true;
                }
            }
        }
        else if (waveIndex < waves.Count && numOfAlive == 0)
        {
            //Start new wave
            waveIndex += 1;
            waveSpawned = false;
        }
    }
    
    private void MakeHuman()
    {
        Human human = Instantiate(humanPrefab, gameObject.transform.position, gameObject.transform.rotation);
        numOfAlive += 1;
        bool male = Mathf.Round(Random.Range(0, 1)) == 0;
        human.male = male;
        if (male)
        {
            human.GetComponent<SpriteRenderer>().sprite = maleHeads[Mathf.RoundToInt(Random.Range(-0.49f, maleHeads.Capacity - 0.51f))];
        }
        else
        {
            human.GetComponent<SpriteRenderer>().sprite = femaleHeads[Mathf.RoundToInt(Random.Range(-0.49f, femaleHeads.Capacity - 0.51f))];
        }
        human.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = bodies[Mathf.RoundToInt(Random.Range(-0.49f, bodies.Capacity - 0.51f))];
    }

}
