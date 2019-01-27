using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Vector3 initialScale;
    private float isMouseOver = 0;
    void Awake()
    {
        initialScale = transform.localScale;
    }
    void Update()
    {
        isMouseOver -= Time.deltaTime;
        if (isMouseOver <= 0)
            transform.localScale = initialScale;
    }
    void OnMouseOver()
    {
        transform.localScale = 1.05f * initialScale;
        isMouseOver = 0.05f;
    }
}
