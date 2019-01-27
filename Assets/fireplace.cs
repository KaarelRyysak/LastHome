using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireplace : MonoBehaviour
{
    private GameObject flames;
    private Trap trap;
    private bool activated;
    public GameObject firePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        flames = gameObject.transform.GetChild(0).gameObject;
        trap = gameObject.GetComponent<Trap>();
        activated = trap.Activated;
    }

    // Update is called once per frame
    void Update()
    {
        if (trap.Activated && !activated || !trap.Activated && activated)
        {
            if (trap.Activated)
            {
                flames.transform.localScale = new Vector3(1f, 1f, 1f);
                activated = true;
            }
            else
            {
                flames.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                activated = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Human human = collision.gameObject.GetComponent<Human>();
        //If collided with human
        if (human != null && activated)
        {
            GameObject fire2 = GameObject.Instantiate(firePrefab, human.transform.position, human.transform.rotation, human.transform);
            fire2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            fire2.transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}
