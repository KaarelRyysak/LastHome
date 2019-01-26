using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public bool activated;
    public Sprite activatedSprite;
    public Sprite disabledSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        //Inactive on startup
        activated = false;
        spriteRenderer.sprite = disabledSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Toggle active on click
        activated = !activated;

        //Change sprite
        if (activated)
        {
            spriteRenderer.sprite = activatedSprite;
        }
        else
        {
            spriteRenderer.sprite = disabledSprite;
        }
    }

}
