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
    Image image;
    public bool buttonPressed;

    void Start()
    {
        image = bg.gameObject.GetComponent<Image>();
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
        image.color = Color.black;
        // Color Background
    }
}
