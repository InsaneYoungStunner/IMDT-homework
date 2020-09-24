using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContrller : MonoBehaviour
{
    public int n = 0;
    public Text score;
    void Start()
    {
        
    }


    void Update()
    {
        score.text = "得分：" + n;
    }
}
