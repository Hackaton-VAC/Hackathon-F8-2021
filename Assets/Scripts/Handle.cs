using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using IBM.Watson.TextToSpeech.V1;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.BasicAuth;
using IBM.Cloud.SDK;
using MyProject.Speech;
using UnityEngine.UI;
using System;

public class Handle : MonoBehaviour {
	public static bool has_new_audio = false;
	public string outputText = "";
	public GameObject popup;
	public GameObject backStage;
	public Text myText;
	public static bool help_action = false;
	public bool successAction = false;
	public string title;
	public string brainPart;
	public string intent = "";
	string orientation = "";

	ExhibitionController controller;
	Dictionary<string, string> uglyDict = new Dictionary<string, string> { { "brainstem","Stem" },
		{ "temporal","Temporal Lobe" },
		{ "occipital","Occipital Lobe" },
		{ "parietal","Parietal Lobe" },
		{ "frontal","Frontal Lobe" },
		{ "cerebellum","Cerebellum" }
		};

	public void Update()
    {
		if(help_action && outputText == "Hold the button and try one of these phrases!")
        {
			popup.SetActive(true);
			help_action = false;
		}
		if (outputText == "<speak version='1.0'><prosody pitch='150Hz'>Closing help</prosody></speak>")
		{
			popup.SetActive(false);
			help_action = false;
		}
        if (has_new_audio)
        {
            if (successAction)
            {
                switch (intent)
                {
					case "select_object":
						controller.SetAsMainSHow(uglyDict[brainPart]);
						break;
					case "group_object":
						controller.CollidePart(uglyDict[brainPart]);
						break;
					case "divide_object":
						controller.RemovePartFromGroup(uglyDict[brainPart]);
						break;
					case "turn_object":
						var jaja = (HackathonUtils.Rotations)Enum.Parse(typeof(HackathonUtils.Rotations), orientation.ToUpper());
						controller.SignalShowRotation(jaja);
						break;
					default:
						print("THIS SHOULD NOT HAPPEN");
						break;
				}
			}
			myText.text = title;
			print($@"OUTPUTTEXT: {outputText}");
			StartCoroutine(audioListener.Speech(outputText));
			Handle.has_new_audio = false;
			MyButton.resetButton = true;
		}
	}

    public void Start()
    {
		controller = backStage.GetComponent<ExhibitionController>();

	}

    public void HandleMe(string textToParse) {
		audioListener.Start();
		var response = JSON.Parse(textToParse);
        intent = response["intents"][0]["name"].Value.ToLower();

		// possible values for orientation entity: left, right, down, up
		// possible values for brain_part entity: brainstem, temporal, occipital,
		// parietal, frontal, cerebellum

		// what function should I call?
		title = "";
		successAction = false;
        switch (intent)
		{
		case "about_object":
			// When the user wants to know info about a specific brain part
			if (response["entities"]["brain_part:brain_part"] != null && ("group" != response["entities"]["brain_part:brain_part"][0]["value"])) {
				title = response["entities"]["brain_part:brain_part"][0]["value"];
				switch (response["entities"]["brain_part:brain_part"][0]["value"])
				{
					case "brainstem":
						outputText = @"The brainstem is the major route that connects the forebrain, spinal cord, 
									and peripheral nerves. It also controls breathing and heart rate.";
						break;
					case "temporal":
						outputText = @"Separated from the frontal lobe by the lateral fissure, the temporal lobe also contains regions dedicated 
										to processing sensory information, particularly important for hearing, recognising language, 
										and forming memories.";
						break;
					case "occipital":
						outputText = @"The occipital lobe is responsible for receiving visual information and sending it to other brain areas that process it.";
						break;
					case "parietal":
						outputText = @"The parietal lobe is the part of the body that is responsible for receiving sensations of touch, cold heat, 
										pressure, pain, and coordinating balance and is located behind the frontal lobe.";
						break;
					case "frontal":
						outputText = @"The frontal lobe is responsible for linguistic and oral production. Defines our behavioral orientation and is related to motivation.
										Here is where other higher executive functions including planning, reasoning and problem solvin occur.";
						break;
					case "cerebellum":
						outputText = @"The cerebellum is the region of the brain whose main function is to integrate the sensory and motor pathways. 
										It controls balance, coordinates movement, and maintains muscle tone.";
							break;
					default:
							outputText = "Sorry, I didn't understand the brain part that you want information about.";
							title = "";
							break;
				}
				//StartCoroutine(audioListener.Speech(outputText));
			}
			else {
				outputText = "Sorry, I didn't understand the brain part that you want information about.";
					//StartCoroutine(audioListener.Speech(outputText));
			}
			break;
		case "group_object":
			// When the user wants to group an independent part with the group 
			if (response["entities"]["brain_part:brain_part"] != null && ("group" != response["entities"]["brain_part:brain_part"][0]["value"]) ) {
				brainPart = response["entities"]["brain_part:brain_part"][0]["value"];
				title = $@"Grouping {brainPart}";
				successAction = true;
				outputText = title;
					//controller.SetAsMainSHow(uglyDict[brainPart]);
					//GroupCommand(brainPart);
				}
			else {
				outputText = "Sorry, I didn't understand the brain part that you want to group.";

			}
			break;
		case "divide_object":
			// When the user wants to separate a part of the group
			if (response["entities"]["brain_part:brain_part"] != null && ("group" != response["entities"]["brain_part:brain_part"][0]["value"])) {
				brainPart = response["entities"]["brain_part:brain_part"][0]["value"];
				title = $@"Dividing {brainPart}";
				successAction = true;
				outputText = title;
				//DivideCommand(brainPart);
				}
			else {
				outputText = "Sorry, I didn't understand the brain part that you want to separate.";
			}
			break;
		case "select_object":
			if (response["entities"]["brain_part:brain_part"] != null) {
					brainPart = response["entities"]["brain_part:brain_part"][0]["value"];
					title = $@"Selecting {brainPart} ";
					outputText = title;
					successAction = true;
				}
				else {
					outputText = "Sorry, I didn't understand the brain part that you want to select.";
			}
			break;
		case "turn_object":
			// defining the default value
			orientation = "right";
			if (response["entities"]["orientation:orientation"] != null) {
				orientation = response["entities"]["orientation:orientation"][0]["value"];
				title = "Turning " + response["entities"]["brain_part:brain_part"][0]["value"];
				successAction = true;
				outputText = "Rotating Part";

			}
				//TurnCommand(orientation);
				break;
		case "open_help":
			outputText = "Hold the button and try one of these phrases!";
			help_action = true;
			title = "";
			//FocusCommand();
			break;
		case "close_help":
			outputText = "<speak version='1.0'><prosody pitch='150Hz'>Closing help</prosody></speak>";
			help_action = true;
			title = "";
			//FocusCommand();
			break;
		default:
			title = "";
			outputText = "Sorry, didn't understand your intent.";
			//StartCoroutine(audioListener.Speech(outputText));
			break;
		}

        Debug.Log("SALGO DE AQUI");
        has_new_audio = true;
	}
}