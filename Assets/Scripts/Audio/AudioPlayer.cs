using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    
    public AudioGroup clickGroup;
    public AudioGroup closeDoorGroup;
    public AudioGroup openDoorGroup;
    public AudioGroup femaleDeathGroup;
    public AudioGroup maleDeathGroup;
    public AudioGroup negGroup;
    public AudioGroup posGroup;
    public AudioGroup neutGroup;

    public static AudioPlayer instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
