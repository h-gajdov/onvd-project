using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform tutorialPanel;
    [SerializeField] private Transform skopje;
    [SerializeField] private GameObject[] highlights;
    
    public int step = 0;

    private bool pressedA = false, pressedD = false;
    private bool pressedQ = false, pressedE = false;
    private bool pressedSpace;
    private bool changed = false;

    private Package skopjePackage;
    
    private void Start()
    {
        EnableStep(step);
        DisableAllHighlights();
        Player.canDropPackage = false;
    }

    private void Update()
    {
        if (step > 11) Player.canDropPackage = false;
        if(CanAdvance()) Next();
    }

    private bool CanAdvance()
    {
        switch (step)
        {
            case 0:
                return Input.GetKeyDown(KeyCode.W);
            case 1:
                if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
                if (Input.GetKeyDown(KeyCode.D)) pressedD = true;
                return pressedA && pressedD;
            case 2:
                if (Input.GetKeyDown(KeyCode.Q)) pressedQ = true;
                if (Input.GetKeyDown(KeyCode.E)) pressedE = true;
                return pressedQ && pressedE;
            case 8:
                return GameMath.PositionIsInView(skopje.position);
            case 9:
                return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
            case 10:
                return GameManager.cityIndex != 1;
            case 11:
                return GameManager.cityIndex != 2;
        }

        return false;
    }

    public void Next()
    {
        EnableStep(++step);

        if (step >= 10) Player.canDropPackage = true;
        
        if (step >= 5 && step <= 8)
        {
            EnableHighlight(step - 5);
            GameManager.instance.compass.SetActive(step == 8);
            Player.SetMovement(step == 8);
        }
        else
        {
            DisableAllHighlights();
            Player.SetMovement(true);
        }
        
        if(step == 4) Player.SetMovement(false);
    }
    
    private void DisableAllSteps()
    {
        foreach(Transform child in tutorialPanel) child.gameObject.SetActive(false);
    }

    private void EnableStep(int index)
    {
        DisableAllSteps();
        tutorialPanel.GetChild(index).gameObject.SetActive(true);
    }

    private void DisableAllHighlights()
    {
        foreach(GameObject highlight in highlights) highlight.SetActive(false);
    }

    private void EnableHighlight(int index)
    {
        DisableAllHighlights();
        highlights[index].SetActive(true);
    }
}
