using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITalking : UIBase
{
    [SerializeField] public GameObject TalkingUI;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    private List<DialogueEntry> curDialogue = new List<DialogueEntry>();
    private int dialogueNum = 0;
    private Coroutine talkCoroutine;
    private bool isTyping = false;

    [SerializeField] private float delay = 0.125f;

    private void Update()
    {
        if (TalkingUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isTyping)
                {
                    SkipTyping();
                }
                else
                {
                    NextDialogueTalkingUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RemoveTalkingUI();
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
        talkCoroutine = StartCoroutine(DelayText());
    }

    private IEnumerator DelayText()
    {
        isTyping = true;
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

        isTyping = false;
    }

    private void SkipTyping()
    {
        StopCoroutine(talkCoroutine);
        npcDialogueText.text = curDialogue[dialogueNum].dialogText;
        isTyping = false;
    }

    // npc와 상호작용이 종료되면 호출 -> 대화UI의 정보들을 초기화
    public void RemoveTalkingUI()
    {
        npcNameText.text = "";
        npcDialogueText.text = "";
        dialogueNum = 0;
        
        if (talkCoroutine != null)
            StopCoroutine(talkCoroutine);
        
        curDialogue.Clear();
        TalkingUI.SetActive(false);
    }

    // npc의 대화를 다음 페이지로 넘기는 메서드
    private void NextDialogueTalkingUI()
    {
        dialogueNum++;

        if (dialogueNum < curDialogue.Count)
        {
            npcNameText.text = curDialogue[dialogueNum].npcName;

            if (talkCoroutine != null)
                StopCoroutine(talkCoroutine);

            talkCoroutine = StartCoroutine(DelayText());
        }
        else
        {
            RemoveTalkingUI();
        }
    }
}
