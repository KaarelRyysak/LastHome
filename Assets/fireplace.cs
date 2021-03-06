﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireplace : MonoBehaviour
{
    private GameObject flames;
    private Trap trap;
    private bool activated;
    public GameObject firePrefab;
    public GameObject ashPrefab;
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
        if (human != null && activated && !human.onFire)
        {
            human.StartCoroutine(human.OnFire(firePrefab, ashPrefab));
        }
    }
}
