using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField]
    string csv_FileName;

    Dictionary<int, Dialog> dialogDic = new Dictionary<int, Dialog>();

    public static bool isFinish = false;                                    // 전부 저장이 되었는지를 알려주는 변수

    int startNum, endNum;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DialogParser parser = GetComponent<DialogParser>();
            Dialog[] dialogs = parser.Parse(csv_FileName);

            startNum = 1;
            endNum = dialogs.Length;

            for(int i = 0; i< dialogs.Length; ++i)
            {
                dialogDic.Add(i + 1, dialogs[i]);
            }
            isFinish = true;
        }
    }

    //public Dialog[] GetDialog(int _StartNum, int _EndNum)
    //{

    //    List<Dialog> dialogList = new List<Dialog>();

    //    for(int i=0; i<= _EndNum - _StartNum; ++i)
    //    {
    //        dialogList.Add(dialogDic[_StartNum + i]);
    //    }

    //    return dialogList.ToArray();
    //}

    public Dialog[] GetDialog()
    {

        List<Dialog> dialogList = new List<Dialog>();

        for (int i = 0; i <= endNum - startNum; ++i)
        {
            dialogList.Add(dialogDic[startNum + i]);
        }

        return dialogList.ToArray();
    }
}
