using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject spinner;
    public GameObject icon;
    public GameObject pulse;
    public GameObject bg;
    public Color activeColor;
    public Color blockedColor;

    Image image;

    public bool buttonPressed;

    void Start()
    {
        image = bg.gameObject.GetComponent<Image>();
        image.color = activeColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        pulse.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        buttonPressed = false;
        spinner.SetActive(true);
        icon.SetActive(false);
        image.color = blockedColor;
        // Color Background
    }

    public void DefaultState()
    {
        Debug.Log("Entro!");
        buttonPressed = false;
        spinner.SetActive(false);
        icon.SetActive(true);
        pulse.SetActive(true);
        image.color = activeColor;
    }
}
