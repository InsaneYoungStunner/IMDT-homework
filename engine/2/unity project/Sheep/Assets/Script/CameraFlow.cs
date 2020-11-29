using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public float speed = 2f;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,player.transform.position+offset,Time.deltaTime*speed);
    }
}
