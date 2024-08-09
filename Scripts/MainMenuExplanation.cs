using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuExplanation : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
    }
}
