using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToScene : MonoBehaviour
{
	//public Button yourButton;
	public GameObject popup;
	void Start()
	{
		//Button btn = yourButton.GetComponent<Button>();
		//btn.onClick.AddListener(TaskOnClick);
	}

	public void StartBrainGame()
	{
		SceneManager.LoadScene("BackupBrain");
	}

    public void StartSolarSystemGame()
    {
        SceneManager.LoadScene("SolarScene");
    }

    public void BackToMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void HelpToggle()
	{
        if (popup.activeSelf)
        {
			popup.SetActive(false);
		}
        else
        {
			popup.SetActive(true);
		}
	}


}
