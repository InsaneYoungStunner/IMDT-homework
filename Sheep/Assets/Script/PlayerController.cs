using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform groudCheck;
    public float moveSpeed = 0.1f;
    public float rotateSpeed =2f;
    public float groudDis = 0.4f;
    public LayerMask groudMask;
    public bool isGround;
    public UIContrller uIContrller;
    void Start()
    {
        
    }


    void Update()
    {
        isGround = Physics.CheckSphere(groudCheck.position, groudDis, groudMask);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (y!=0)
        {
            GetComponent<Animator>().SetInteger("animation",2);
        }
        else
        {
            GetComponent<Animator>().SetInteger("animation", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            GetComponent<Animator>().SetInteger("animation", 3);
            GetComponent<Rigidbody>().AddForce(new Vector3(0,150,0));
        }

        transform.Translate(transform.forward*y* moveSpeed, Space.World);
        transform.Rotate(0,x* rotateSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "pick")
        {
            Destroy(collision.collider.gameObject);
            uIContrller.n++;
        }
    }
}
