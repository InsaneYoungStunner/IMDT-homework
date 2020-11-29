using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeadCheck : MonoBehaviour
{

    public GameObject deadPage;
    public Text score;
    public UIContrller uIContrller;
    void Start()
    {
        uIContrller = FindObjectOfType<UIContrller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<PlayerController>().enabled = false;
            collision.collider.GetComponent<Animator>().SetInteger("animation",8);
            deadPage.SetActive(true);
            score.text = "得分：" + uIContrller.n;
        }
    }
}
