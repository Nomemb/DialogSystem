using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [Tooltip("현재 말하는 캐릭터 이름")]
    public string name;

    [Tooltip("현재 캐릭터 감정")]
    public string state;

    [Tooltip("대사 내용")]
    public string[] contexts;
}
