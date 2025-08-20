using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
    public GameObject pauseUI;
    public Button resumeButton;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    public override void OpenUI()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public override void CloseUI()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnResumeButton()
    {
        CloseUI();
        CharacterManager.Instance.Player.controller.changeIsPaused();
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
