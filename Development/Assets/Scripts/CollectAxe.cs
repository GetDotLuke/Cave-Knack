using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollectAxe : MonoBehaviour {

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
        if (CollectStone.quantity >= 1 && CollectWood.quantity >= 1) //&& ClickAxe.clicked == true)
        {
            quantity++;
            CollectStone.quantity--;
            CollectWood.quantity--;
            //ClickAxe.clicked = false;

        }

        text.text = "" + quantity;
    }
}
