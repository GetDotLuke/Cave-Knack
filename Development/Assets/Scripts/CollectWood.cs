using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectWood : MonoBehaviour
{

    Text text;
    //Quantity of item
    public static int quantity;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        quantity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + quantity;
    }

    public static void IncrementQuantity()
    {

        quantity++;
    }
}


