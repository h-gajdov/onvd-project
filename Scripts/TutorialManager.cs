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
    private bool pressedSpace;
    private bool changed = false;

    private Package skopjePackage;
    
    private void Start()
    {
        EnableStep(step);
        Player.canDropPackage = false;
    }

    private void Update()
    {
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
            case 7:
                return GameMath.PositionIsInView(skopje.position);
            case 8:
                return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
            case 9:
                return GameManager.cityIndex != 1;
            case 10:
                return GameManager.cityIndex != 2;
        }

        return false;
    }

    public void Next()
    {
        EnableStep(++step);

        if (step >= 9) Player.canDropPackage = true;

        if (step >= 4 && step <= 6)
        {
            EnableHighlight(step - 4);
            Player.SetMovement(false);
        }
        else
        {
            DisableAllHighlights();
            Player.SetMovement(true);
        }
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
