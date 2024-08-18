using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float speed = 1f;
    public float scaleMultiplier = 1.1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(HoverUp());
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(HoverDown());
    }

    private IEnumerator HoverUp()
    {
        while (transform.localScale.x <= 1.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleMultiplier, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator HoverDown()
    {
        while (transform.localScale.x >= 1.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, speed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
