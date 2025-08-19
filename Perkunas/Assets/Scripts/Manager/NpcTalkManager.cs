using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcTalkManager : MonoSingleton<NpcTalkManager>
{
    [SerializeField] public GameObject TalkingUI;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    private List<DialogueEntry> curDialogue = new List<DialogueEntry>();
    private int dialogueNum = 0;

    [SerializeField] private float delay = 0.125f;

    private void Update()
    {
        if (TalkingUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                NextDialogueTalkingUI();
            }
        }
    }

    // npc와 상호작용하면 호출될 메서드 -> 대화UI에게 정보들을 넘겨준다
    public void InitTalkingUI(List<DialogueEntry> dialogue)
    {
        TalkingUI.SetActive(true);
        
        foreach (var data in dialogue)
        {
            curDialogue.Add(data);
        }
        
        npcNameText.text = curDialogue[dialogueNum].npcName;
        // npcDialogueText.text = curDialogue[dialogueNum].text;
        StartCoroutine(DelayText());
    }

    private IEnumerator DelayText()
    {
        int count = 0;
        npcDialogueText.text = "";
        
        while (count != curDialogue[dialogueNum].dialogText.Length)
        {
            if (count < curDialogue[dialogueNum].dialogText.Length)
            {
                npcDialogueText.text += curDialogue[dialogueNum].dialogText[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    // npc와 상호작용이 종료되면 호출 -> 대화UI의 정보들을 초기화
    public void RemoveTalkingUI()
    {
        npcNameText.text = "";
        npcDialogueText.text = "";
        dialogueNum = 0;
        curDialogue.Clear();
        TalkingUI.SetActive(false);
    }

    // npc의 대화를 다음 페이지로 넘기는 메서드
    private void NextDialogueTalkingUI()
    {
        dialogueNum++;

        if (dialogueNum < curDialogue.Count)
        {
            StartCoroutine(DelayText());
        }
        else
        {
            RemoveTalkingUI();
        }
    }
}
