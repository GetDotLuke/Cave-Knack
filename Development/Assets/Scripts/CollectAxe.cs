﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollectAxe : MonoBehaviour {

    Text text;
    //Quantity of item
    public static int quantity;
    public static int numberUses;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        quantity = 0;
        numberUses = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CollectStone.quantity >= 1 && CollectWood.quantity >= 1 && Input.GetKeyDown("j"))
        {
            quantity++;
            CollectStone.quantity--;
            CollectWood.quantity--;
            numberUses = 0;

        }

        text.text = "" + quantity;
    }

    public static void IncrementUses()
    {
        numberUses++;
    }

    public static void ResetUses()
    {
        numberUses = 0;
    }
}
