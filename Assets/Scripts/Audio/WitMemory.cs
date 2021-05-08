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
using System.Threading.Tasks;
using MyProject.Speech;

public class WitMemory : MonoBehaviour
{
    // Class Variables
    Task writing_request = null;
    public Button btn_mic;
    public MyButton myButton;
    // Audio variables
    public Text audioText;
    public AudioClip commandClip;
    int samplerate;
    HttpWebRequest request;

    // API access parameters
    string url;
    string token;
    bool btn_aux = false;
    UnityWebRequest wr;

    // Movement variables
    public float moveTime;
    public float yOffset;

    // GameObject to use as a default spawn point
    public GameObject spawnPoint;
    public Handle handle;

    private bool resetButton = false;
    // Use this for initialization
    void Start()
    {

        // If you are a Windows user and receiving a Tlserror
        // See: https://github.com/afauch/wit3d/issues/2
        // Uncomment the line below to bypass SSL
        // System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
        myButton = btn_mic.GetComponent<MyButton>();
        // set samplerate to 16000 for wit.ai
        samplerate = 16000;
        handle = gameObject.GetComponent<Handle>();
        audioListener.Start();
        
    }

    // Update is called once per frame

    void Update()
    {
        
        if (!btn_aux && myButton.buttonPressed)
        {
            print("Listening for command");
            commandClip = Microphone.Start(null, false, 10, samplerate);  //Start recording (rewriting older recordings)
            btn_aux = true;
        }
        else if (btn_aux && !myButton.buttonPressed)
        {
            var watch = new System.Diagnostics.Stopwatch();
            var watch1 = new System.Diagnostics.Stopwatch();
            watch.Start();
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
            watch.Stop();

            //Start a coroutine called "WaitForRequest" with that WWW variable passed in as an argument
            //audioFile
            watch1.Start();
            GetJSONText(audioFile);
            watch1.Stop();
            btn_aux = false;
        }
        if (Handle.has_new_audio)
        {
            print("HAHAHA AYURA");
            StartCoroutine(audioListener.Speech(Handle.outputText));
            print("JAJAJAJA WTF MANO");
            Handle.has_new_audio = false;
            MyButton.resetButton = true;
        }
    }

    void FinishWebRequest(IAsyncResult result)
    {
        print("ENTRO EN FINISH WEB REQ");
        var response = request.EndGetResponse(result);
        StreamReader response_stream = new StreamReader(response.GetResponseStream());
        string text = response_stream.ReadToEnd();
        print("BEFORE HANDLE ME");
        handle.HandleMe(text);
        print("Handle Text");
        print(text);
    }

    public void GetJSONText(byte[] BA_AudioFile)
    {
        // create an HttpWebRequest
        request = (HttpWebRequest)WebRequest.Create("https://api.wit.ai/speech?v=20200513");
        request.Method = "POST";
        request.Proxy = null;
        request.Headers["Authorization"] = "Bearer " + token;
        request.ContentType = "audio/wav";
        request.ContentLength = BA_AudioFile.Length;
        request.GetRequestStreamAsync().ContinueWith(stream => {
            stream.Result.WriteAsync(BA_AudioFile, 0, BA_AudioFile.Length).ContinueWith(antecedent => {
                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

}
