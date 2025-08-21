using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : UIBase
{
    public GameObject BackGround;
    
    private void Start()
    {
        BackGround.SetActive(false);
    }
}
