using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField]
    DialogEvent dialog;

    public Dialog[] GetDialog()
    {
        dialog.dialogs = DatabaseManager.instance.GetDialog();

        return dialog.dialogs;
    }
}
