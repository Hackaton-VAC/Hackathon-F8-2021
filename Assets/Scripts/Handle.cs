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


public partial class Handle : MonoBehaviour {


	public void HandleMe(string textToParse) {
		audioListener.Start();
		print (textToParse);
		var response = JSON.Parse(textToParse);
		print ("SimpleJSON: " + response.ToString());
		string outputText = "";

		string intent = response["intents"][0]["name"].Value.ToLower();

		// possible values for orientation entity: left, right, down, up
		// possible values for brain_part entity: brainstem, temporal, occipital,
		// parietal, frontal, cerebellum

		// what function should I call?
		switch (intent)
		{
		case "about_object":
			// When the user wants to know info about a specific brain part
			if (response["entities"]["brain_part"] != null) {
				switch (response["entities"]["brain_part"]["value"])
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
							break;
					StartCoroutine(audioListener.Speech(outputText));
				}
				
			} 
			else {
					outputText = "Sorry, I didn't understand the brain part that you want information about.";
				StartCoroutine(audioListener.Speech(outputText));
			}
			break;
		case "group_object":
			// When the user wants to group an independent part with the group 
			if (response["entities"]["brain_part"] != null) {
				string brainPart = response["entities"]["brain_part"]["value"];
				//GroupCommand(brainPart);
			}
			else {
					outputText = "Sorry, I didn't understand the brain part that you want to group.";
				StartCoroutine(audioListener.Speech(outputText));
			}
			break;
		case "divide_object":
			// When the user wants to separate a part of the group
			if (response["entities"]["brain_part"] != null) {
				string brainPart = response["entities"]["brain_part"]["value"];
				//DivideCommand(brainPart);
			}
			else {
					outputText = "Sorry, I didn't understand the brain part that you want to separate.";
				StartCoroutine(audioListener.Speech(outputText));
			}
			break;
		case "select_object":
			if (response["entities"]["brain_part"] != null) {
				string brainPart = response["entities"]["brain_part"]["value"];
				//SelectCommand(brainPart);
			}
			else {
					outputText = "Sorry, I didn't understand the brain part that you want to select.";
				StartCoroutine(audioListener.Speech(outputText));
			}
			break;
		case "turn_object":
			// defining the default value
			string orientation = "right";

			if (response["entities"]["orientation"] != null) {
				orientation = response["entities"]["orientation"]["value"];
				
			}
			//TurnCommand(orientation);
			break;
		case "focus_group":
			//FocusCommand();
			break;
		default:
				outputText = "Sorry, didn't understand your intent.";
			StartCoroutine(audioListener.Speech(outputText));
			break;
		}


	}
}