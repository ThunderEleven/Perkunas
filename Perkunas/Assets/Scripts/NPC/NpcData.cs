using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public int id;
    public string npcName;
    public string text;
}

// public class NpcData : MonoBehaviour
// {
//     public string npcName;
//     private List<Dictionary<string, object>> dialogue;
//     
//     private void Start()
//     {
//         dialogue = CSVReader.Read("DialogueData");
//
//         for (int i = 0; i < dialogue.Count; i++)
//         {
//             Debug.Log("대화 번호: " + dialogue[i]["대화 번호"]);
//             Debug.Log("NPC 이름: " + dialogue[i]["npc 이름"]);
//             Debug.Log("대화 내용: " + dialogue[i]["대화 내용"]);
//         }
//     }
// }

// npc 데이터에 CSV 파일에서 가져온 대화 데이터가 잘 들어가는지 확인하는 코드
public class NpcData : MonoBehaviour
{
    public string npcName;

    [SerializeField] private List<DialogueEntry> dialogues; // Inspector에서 보임

    private void Start()
    {
        var rawData = CSVReader.Read("DialogueData");
        dialogues = new List<DialogueEntry>();

        foreach (var row in rawData)
        {
            DialogueEntry entry = new DialogueEntry();
            entry.id = (int)row["대화 번호"];
            entry.npcName = row["npc 이름"].ToString();
            entry.text = row["대화 내용"].ToString();
            dialogues.Add(entry);
        }

        // 확인용
        foreach (var d in dialogues)
        {
            Debug.Log($"[{d.id}] {d.npcName}: {d.text}");
        }
    }
}