using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : UIBase
{
    public GameObject BackGround;
    
    private void Start()
    {
        BackGround.SetActive(false);
    }

    public override void OpenUI()
    {
        BackGround.SetActive(true);
        UIManager.Instance.uiStack.Push(this);
        CharacterManager.Instance.Player.controller.ToggleCursor();
        Time.timeScale = 0f;
    }

    public override void CloseUI()
    {
        BackGround.SetActive(false);
        UIManager.Instance.uiStack.Pop();
        Time.timeScale = 1f;
    }

    public void OnClickExitButton()
    {
        CloseUI();
        SceneManager.LoadScene("StartScene");
    }
}
