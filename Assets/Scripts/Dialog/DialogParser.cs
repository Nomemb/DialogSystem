using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogParser : MonoBehaviour
{
    public Dialog[] Parse(string _CSVFileName)
    {
        List<Dialog> dialogList = new List<Dialog>();                   // 대사 리스트
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);    // csv 파일 가져옴

        string[] data = csvData.text.Split('\n');
        
        for(int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(',');

            Dialog dialog = new Dialog();
            dialog.name = row[1];
            dialog.state = row[2];
        
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[3]);
                if (++i < data.Length)
                    row = data[i].Split(',');
                else
                    break;
            } while (row[0].ToString() == "");

            dialog.contexts = contextList.ToArray();
            dialogList.Add(dialog);
        }
        return dialogList.ToArray();
    }
}
