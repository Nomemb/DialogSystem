using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBar;
    [SerializeField] GameObject dialogNameBar1;
    [SerializeField] GameObject dialogNameBar2;
    [SerializeField] GameObject skipButton;

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

    bool isFirst = true;       // 첫번째인지 두번째인지

    private string cookie1State;
    private string cookie2State;

    [Header("텍스트 출력 딜레이")]
    [SerializeField]
    float textDelay;


    int lineCount = 0;                  // 대화 카운트
    int contextCount = 0;               // 대사 카운트

    private void Start()
    {
        ShowDialog(this.transform.GetComponent<InteractionEvent>().GetDialog());
        ReverseCookie2Image();
        SettingDialog(1);
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

        dialogs = p_dialogs;

        StartCoroutine(TypeWriter());
    }
    public void EndDialog()
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
        string _currentState = dialogs[lineCount].state;
        string _changeLocate = dialogs[lineCount].changeLocate;

        if (_changeLocate == "1")
        {
            Debug.Log(_changeLocate);
            SettingDialog(lineCount);
            isFirst = true;
            SettingUI(true, isFirst, true);
        }
        else
            SettingUI(true, isFirst);

        if(isFirst)
        {
            if(txt_Cookie1.text != _currentName || cookie1State != _currentState)
            {
                txt_Cookie1.text = _currentName;
                cookie1State = _currentState;
                ChangeImage(isFirst);
            }
        }
        else
        {
            if (txt_Cookie2.text != _currentName || cookie2State != _currentState)
            {
                txt_Cookie2.text = _currentName;
                cookie2State = _currentState;
                ChangeImage(isFirst);
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
    void ReverseCookie2Image()
    {
        Vector3 scale = img_Cookie2.transform.localScale;           // 2p 이미지 뒤집기
        scale.x = -Mathf.Abs(scale.x);
        img_Cookie2.transform.localScale = scale;
    }
    void SettingDialog(int p_lineNo)
    {
        txt_Cookie1.text = dialogs[p_lineNo - 1].name;
        txt_Cookie2.text = dialogs[p_lineNo].name;
        cookie1State = dialogs[p_lineNo - 1].state;
        cookie2State = dialogs[p_lineNo].state;

        string cookie1Path = "Image/" + txt_Cookie1.text;
        string cookie2Path = "Image/" + txt_Cookie2.text;

        cookie1Images = Resources.LoadAll<Sprite>(cookie1Path);
        cookie2Images = Resources.LoadAll<Sprite>(cookie2Path);

        ChangeCookieImage(img_Cookie1);
        ChangeCookieImage(img_Cookie2);

        img_Cookie1.enabled = true;
        img_Cookie2.enabled = true;
    }
    void ChangeImage(bool p_isFirst)
    {
        if(p_isFirst)           // 첫번째 쿠키가 변경됐을 경우
        {
            string cookie1Path = "Image/" + txt_Cookie1.text;
            cookie1Images = Resources.LoadAll<Sprite>(cookie1Path);
            ChangeCookieImage(img_Cookie1);
        }
        else
        {
            string cookie2Path = "Image/" + txt_Cookie2.text;
            cookie2Images = Resources.LoadAll<Sprite>(cookie2Path);
            ChangeCookieImage(img_Cookie2);
        }
    }

    void ChangeCookieImage(Image p_Cookie)
    {
        if(p_Cookie == img_Cookie1)
        {
            Debug.Log(1);
            foreach(Sprite i in cookie1Images)
            {
                if (i.name == cookie1State)
                {
                    img_Cookie1.sprite = i;
                    return;
                }
            }
        }
        else
        {
            Debug.Log(2);
            foreach (Sprite i in cookie2Images)
            {
                if (i.name == cookie2State)
                {
                    img_Cookie2.sprite = i;
                    return;
                }
            }
        }
    }
    void SettingUI(bool p_flag, bool p_isFirst = true, bool p_hideCookie2 = false)
    {
        dialogBar.SetActive(p_flag);
        skipButton.SetActive(p_flag);

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
            if (p_hideCookie2)
                img_Cookie2.enabled = false;
            else
                img_Cookie2.enabled = true;
            dialogNameBar2.SetActive(!p_flag);
        }
        else
        {
            img_Cookie2.enabled = true;
            dialogNameBar1.SetActive(!p_flag);
            dialogNameBar2.SetActive(p_flag);
        }
    }
}
