using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayValidator : MonoBehaviour, IPointerDownHandler
{
    public string[] messageToIgnore;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        string msg = MainMenuManager.GetErrorMessage();
        if(msg == "" || IsOneOfIgnore(msg)) return;
        
        GameObject error = Instantiate(MainMenuManager.instance.errorPrefab);
        error.transform.parent = MainMenuManager.instance.feedback;
        error.transform.position = Input.mousePosition;
        error.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = msg;
        Destroy(error, 2f);
    }

    private bool IsOneOfIgnore(string msg)
    {
        foreach (string m in messageToIgnore)
        {
            if (m == msg) return true;
        }
        return false;
    }
}
