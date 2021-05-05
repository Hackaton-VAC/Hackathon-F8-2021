using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class audioListener : MonoBehaviour
{

    string theURL = "https://apikey:vjdinuh7lH_duRL2e5-fUID9UQ9wQwJSeTG8W9NA0HyA@api.us-south.text-to-speech.watson.cloud.ibm.com/instances/dcdd17b8-a759-4276-a5d9-1a3c565c3a51/v1/synthesize?text=jamon&accept=audio%2Fwav";


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(DownloadAndPlay(theURL));
        }
    }


    IEnumerator DownloadAndPlay(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(theURL, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (true)
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                AudioSource audio = GetComponent<AudioSource>();
                audio.clip = myClip;
                audio.Play();
                print("reprodujo");
            }
        }
    }
}