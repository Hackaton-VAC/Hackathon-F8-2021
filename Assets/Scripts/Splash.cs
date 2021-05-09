using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public float timeRemaining = 2;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if (timeRemaining <= 0)
        {

            StartCoroutine(FadeImage());
        }
    }

    IEnumerator FadeImage()
    {

        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime*2)
        {
            // set color with i as alpha
            image.color = new Color(1, 1, 1, i);
            yield return null;
        }
        gameObject.SetActive(false);
    

    }
}
