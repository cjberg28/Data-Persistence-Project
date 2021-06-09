using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameInput : MonoBehaviour
{
    public void onChange()
    {
        MenuUIHandler.playerName = GetComponent<TMP_InputField>().text;
    }
}
