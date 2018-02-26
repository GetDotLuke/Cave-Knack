using UnityEngine;
using System.Collections;

public class MoveSheep : MonoBehaviour
{

    public float speed = 3.0F;
    public float rotationSpeed = 2.0F;
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
        
    }

}