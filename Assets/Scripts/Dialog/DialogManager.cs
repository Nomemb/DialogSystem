using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBar;
    [SerializeField] GameObject dialogNameBar1;
    [SerializeField] GameObject dialogNameBar2;

    [SerializeField] Image img_Cookie1;
    [SerializeField] Image img_Cookie2;

    [SerializeField] Text txt_Dialog;
    [SerializeField] Text txt_Cookie1;
    [SerializeField] Text txt_Cookie2;

    [SerializeField] Sprite[] cookie1Images;
    [SerializeField] Sprite[] cookie2Images;

    Dialog[] dialogs;

    bool isDialog = false;              // 대화중일경우 false
    bool isNext = false;                // 키 입력 대기

    public string currentName;         // 지금 말하는 캐릭터 이름
    bool isFirst = true;       // 첫번째인지 두번째인지

    [Header("텍스트 출력 딜레이")]
    [SerializeField]
    float textDelay;


    int lineCount = 0;                  // 대화 카운트
    int contextCount = 0;               // 대사 카운트

    private void Start()
    {
        ShowDialog(this.transform.GetComponent<InteractionEvent>().GetDialog());
        SettingDialog();
    }


    private void Update()
    {
        if(isDialog)
        {
            if(isNext)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isNext = false;
                    txt_Dialog.text = "";

                    if(++contextCount < dialogs[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if(++lineCount < dialogs.Length)
                        {
                            isFirst = !isFirst;
                            StartCoroutine(TypeWriter());
                        }
                        else                                // 대사 끝났을 경우
                        {
                            EndDialog();
                        }
                    }
                }
            }
        }
    }
    public void ShowDialog(Dialog[] p_dialogs)
    {
        isDialog = true;
        txt_Dialog.text = "";
        txt_Cookie1.text = "";
        txt_Cookie2.text = "";
        img_Cookie1.enabled = false;
        img_Cookie2.enabled = false;
        currentName = "";

        dialogs = p_dialogs;

        StartCoroutine(TypeWriter());
    }
    void EndDialog()
    {
        isDialog = false;
        contextCount = 0;
        lineCount = 0;
        dialogs = null;

        isNext = false;
        isFirst = true;
        SettingUI(false);
    }
    IEnumerator TypeWriter()
    {
        string t_ReplaceText = dialogs[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        string _currentName = dialogs[lineCount].name;
        SettingUI(true, isFirst);

        if(isFirst)
        {
            if(txt_Cookie1.text != _currentName)
            {
                txt_Cookie1.text = _currentName;
                ChangeImage(isFirst);
                Debug.Log("Change Cookie1 Image");
            }
        }
        else
        {
            if (txt_Cookie2.text != _currentName)
            {
                txt_Cookie2.text = _currentName;
                ChangeImage(isFirst);
                Debug.Log("Change Cookie2 Image");

            }
        }

        for (int i=0; i< t_ReplaceText.Length; ++i)
        {
            if(t_ReplaceText[i] == '-')
                txt_Dialog.text += "\n";
            else
                txt_Dialog.text += t_ReplaceText[i];
            yield return new WaitForSeconds(textDelay);
        }
        isNext = true;
    }

    void SettingDialog()
    {
        txt_Cookie1.text = dialogs[0].name;
        txt_Cookie2.text = dialogs[1].name;
        string cookie1Path = "Image/" + txt_Cookie1.text;
        string cookie2Path = "Image/" + txt_Cookie2.text;

        cookie1Images = Resources.LoadAll<Sprite>(cookie1Path);
        cookie2Images = Resources.LoadAll<Sprite>(cookie2Path);

        img_Cookie1.sprite = cookie1Images[0];
        img_Cookie2.sprite = cookie2Images[0];

        img_Cookie1.enabled = true;
        img_Cookie2.enabled = true;
    }
    void ChangeImage(bool p_isFirst)
    {
        if(p_isFirst)           // 첫번째 쿠키가 변경됐을 경우
        {
            string cookie1Path = "Image/" + txt_Cookie1.text;
            cookie1Images = Resources.LoadAll<Sprite>(cookie1Path);
            img_Cookie1.sprite = cookie1Images[0];
        }
        else
        {
            string cookie2Path = "Image/" + txt_Cookie2.text;
            cookie2Images = Resources.LoadAll<Sprite>(cookie2Path);
            img_Cookie2.sprite = cookie2Images[0];
        }
    }
    void SettingUI(bool p_flag, bool p_isFirst = true)
    {
        dialogBar.SetActive(p_flag);
        if (!p_flag)
        {
            dialogBar.SetActive(p_flag);
            dialogNameBar1.SetActive(p_flag);
            dialogNameBar2.SetActive(p_flag);
            img_Cookie1.enabled = false;
            img_Cookie2.enabled = false;
            return;
        }
        if (p_isFirst)                                          // 첫번째
        {
            dialogNameBar1.SetActive(p_flag);
            dialogNameBar2.SetActive(!p_flag);
        }
        else
        {
            dialogNameBar1.SetActive(!p_flag);
            dialogNameBar2.SetActive(p_flag);
        }
    }
}
