﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    private GameObject border;
    public bool activated;
    private bool bloody;
    public Sprite activatedSprite;
    public Sprite disabledSprite;
    public Sprite bloodyActivatedSprite;
    public Sprite bloodyDisabledSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Inactive on startup
        activated = false;
        spriteRenderer.sprite = disabledSprite;
    }

    private void OnMouseDown()
    {
        //Toggle active on click
        activated = !activated;

        //Change sprite
        if (activated && !bloody)
        {
            spriteRenderer.sprite = activatedSprite;
        }
        else if (!activated && !bloody)
        {
            spriteRenderer.sprite = disabledSprite;
        }
        else if (activated && bloody)
        {

            spriteRenderer.sprite = bloodyActivatedSprite;
        }
        else
        {
            spriteRenderer.sprite = bloodyDisabledSprite;
        }
    }


    private void Awake()
    {
        border = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Human human = collision.gameObject.GetComponent<Human>();
        //If collided with human
        if (human != null && activated && !human.falling)
        {

            SortLayer sortLayer = human.gameObject.GetComponent<SortLayer>();
            sortLayer.offset = -400;
            human.gameObject.transform.GetChild(0).GetComponent<SortParent>().offset = -401;

            Vector3 humanpos = human.transform.position;
            Vector3 newPos = Vector3.Lerp(humanpos, gameObject.transform.position, 0.1f);
            human.StopAllCoroutines();

            Vector3 thisPos = gameObject.transform.position;
            
            if (Vector3.Magnitude(thisPos - newPos) < 0.2f)
            {
                Drop(human);
                
            }
            else
            {
                human.transform.position = newPos;
            }
        }
    }

    private void Drop(Human human)
    {

        Debug.Log("test");

        human.transform.parent = gameObject.transform;
        human.falling = true;
        human.pitTrap = border;
        human.transform.parent = border.transform;

        human.Die();
    }
}
