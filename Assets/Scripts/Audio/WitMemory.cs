using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine.UI;

public class WitMemory : MonoBehaviour
{
    // Class Variables

    // Audio variables
    public Text audioText;
    public AudioClip commandClip;
    int samplerate;

    // API access parameters
    string url;
    string token;
    UnityWebRequest wr;

    // Movement variables
    public float moveTime;
    public float yOffset;

    // GameObject to use as a default spawn point
    public GameObject spawnPoint;

    // Use this for initialization
    void Start()
    {

        // If you are a Windows user and receiving a Tlserror
        // See: https://github.com/afauch/wit3d/issues/2
        // Uncomment the line below to bypass SSL
        // System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };

        // set samplerate to 16000 for wit.ai
        samplerate = 16000;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Listening for command");
            commandClip = Microphone.Start(null, false, 10, samplerate);  //Start recording (rewriting older recordings)
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {

            // Debug
            print("Thinking ...");
            // Save the audio file
            Microphone.End(null);
            // Call trim here (Record time between start and end)
            byte[] audioFile = WavMemory.Save("sample", commandClip);

            // At this point, we can delete the existing audio clip
            commandClip = null;

            //Grab the most up-to-date JSON file
            token = "ISUHEKFR3XLJ5S7BOZVJSBPUNN3VMKZF";

            //Start a coroutine called "WaitForRequest" with that WWW variable passed in as an argument
            //audioFile
            audioText.text = GetJSONText(audioFile);
            print(audioText.text);
        }


    }

    string GetJSONText(byte[] BA_AudioFile)
    {
        // create an HttpWebRequest
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.wit.ai/speech?v=20200513");

        request.Method = "POST";
        request.Headers["Authorization"] = "Bearer " + token;
        request.ContentType = "audio/wav";
        request.ContentLength = BA_AudioFile.Length;
        request.GetRequestStream().Write(BA_AudioFile, 0, BA_AudioFile.Length);

        // Process the wit.ai response
        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                print("Http went through ok");
                StreamReader response_stream = new StreamReader(response.GetResponseStream());
                return response_stream.ReadToEnd();
            }
            else
            {
                return "Error: " + response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

}
