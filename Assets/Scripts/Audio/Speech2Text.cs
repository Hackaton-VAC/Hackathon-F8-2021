using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using IBM.Watson.TextToSpeech.V1;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.BasicAuth;
using IBM.Cloud.SDK;

namespace MyProject.Speech
{
	static public class audioListener
	{
		#region PLEASE SET THESE VARIABLES IN THE INSPECTOR

		private static string allisionVoice = "en-US_AllisonV3Voice";
		private static string synthesizeMimeType = "audio/wav";
		#endregion

		public static string username = "apikey";
		public static string password = "vjdinuh7lH_duRL2e5-fUID9UQ9wQwJSeTG8W9NA0HyA";
		public static string url = "api.us-south.text-to-speech.watson.cloud.ibm.com/instances/dcdd17b8-a759-4276-a5d9-1a3c565c3a51";
		public static string theURL = "https://apikey:vjdinuh7lH_duRL2e5-fUID9UQ9wQwJSeTG8W9NA0HyA@api.us-south.text-to-speech.watson.cloud.ibm.com/instances/dcdd17b8-a759-4276-a5d9-1a3c565c3a51/v1/synthesize?text=jamon&accept=audio%2Fwav";
		private static Authenticator authenticator;
		private static TextToSpeechService tts;

		public static void Start()
		{
			authenticator = new BasicAuthenticator(username, password);
			tts = new TextToSpeechService(authenticator);
		}

		public static void PlayClip(AudioClip clip)
		{
			if (Application.isPlaying && clip != null)
			{
				GameObject audioObject = new GameObject("AudioObject");
				AudioSource source = audioObject.AddComponent<AudioSource>();
				source.spatialBlend = 0.0f;
				source.loop = false;
				source.clip = clip;
				source.Play();

				GameObject.Destroy(audioObject, clip.length);
			}
		}

		public static IEnumerator Speech(string text)
		{
			Debug.Log("Me llaman para hablar");
            if (text != "")
            {
                byte[] synthesizeResponse = null;
                AudioClip clip = null;
                tts.Synthesize(
                    callback: (DetailedResponse<byte[]> response, IBMError error) =>
                    {
                        synthesizeResponse = response.Result;
                        Debug.Log("ExampleTextToSpeechV1");
                        clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                        PlayClip(clip);
                    },
                    text: text,
                    voice: allisionVoice,
                    accept: synthesizeMimeType
                );

                while (synthesizeResponse == null)
                    yield return null;

                yield return new WaitForSeconds(clip.length);
            }
            yield return null;
		}
	}
}


