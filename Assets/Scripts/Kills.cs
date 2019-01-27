using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kills : MonoBehaviour
{
    public static Kills instance;
    Text killsText;

    int value = 0;
    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            killsText.text = "Kills: " + value;
        }
    }

    void Awake()
    {
        instance = this;
        killsText = GetComponentsInChildren<Text>()[0];
        Value = Value;
    }
}



