using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToScene : MonoBehaviour
{
	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		//btn.onClick.AddListener(TaskOnClick);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
