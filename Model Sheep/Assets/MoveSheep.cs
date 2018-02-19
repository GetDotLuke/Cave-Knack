using UnityEngine;
using System.Collections;

public class MoveSheep : MonoBehaviour
{

    float speed = 7.0f;

    void Update()
    {
        var move = new Vector3(0, 0, Input.GetAxis("Vertical"));

        transform.Rotate( 0.0f, -Input.GetAxis("Horizontal")* speed,  0.0f);

        transform.position += move * speed * Time.deltaTime;
    }
}