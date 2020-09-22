using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    bool zhui = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zhui)
        {
            Vector3 dir = Player.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 1);
            transform.Translate(Vector3.forward * Time.deltaTime * 1);//向前走
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            zhui = false;
        }
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zhui = true;
        }
    }
}
