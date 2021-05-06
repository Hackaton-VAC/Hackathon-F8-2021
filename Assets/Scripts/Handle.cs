using UnityEngine;
using System.Collections;
using SimpleJSON;

public partial class Handle : MonoBehaviour {

	void Handle(string textToParse) {

		print (textToParse);
		var response = JSON.Parse(textToParse);
		print ("SimpleJSON: " + response.ToString());

		string intent = response["intents"][0]["name"].Value.ToLower();

		// possible values for orientation entity: left, right, down, up
		// possible values for brain_part entity: brainstem, temporal, occipital,
		// parietal, frontal, cerebellum

		// what function should I call?
		switch (intent)
		{
		case "about_object":
			// When the user wants to know info about a specific brain part
			if (response["entities"].has("brain_part")) {
				string outputText = "";
				switch (response["entities"]["brain_part"]["value"] == "brainstem")
				{
					case "brainstem":
						outputText = @"The brainstem is the major route that connects the forebrain, spinal cord, 
									and peripheral nerves. It also controls breathing and heart rate.";

					case "temporal":
						outputText = @"Separated from the frontal lobe by the lateral fissure, the temporal lobe also contains regions dedicated 
										to processing sensory information, particularly important for hearing, recognising language, 
										and forming memories.";

					case "occipital":
						outputText = @"The occipital lobe is responsible for receiving visual information and sending it to other brain areas that process it.";

					case "parietal":
						outputText = @"The parietal lobe is the part of the body that is responsible for receiving sensations of touch, cold heat, 
										pressure, pain, and coordinating balance and is located behind the frontal lobe.";

					case "frontal":
						outputText = @"The frontal lobe is responsible for linguistic and oral production. Defines our behavioral orientation and is related to motivation.
										Here is where other higher executive functions including planning, reasoning and problem solvin occur.";

					case "cerebellum":
						outputText = @"The cerebellum is the region of the brain whose main function is to integrate the sensory and motor pathways. 
										It controls balance, coordinates movement, and maintains muscle tone.";

					default:
						outputText = "Sorry, I didn't understand the brain part that you want information about."
					
					VoiceCommand(outputText);
				}
				
			} 
			else {
				outputText = "Sorry, I didn't understand the brain part that you want information about."
				VoiceCommand(outputText);
			}
			break;
		case "group_object":
			// When the user wants to group an independent part with the group 
			if (response["entities"].has("brain_part")) {
				string brainPart = response["entities"]["brain_part"]["value"];
				GroupCommand(brainPart);
			}
			else {
				string outputText = "Sorry, I didn't understand the brain part that you want to group."
				VoiceCommand(outputText);
			}
			break;
		case "divide_object":
			// When the user wants to separate a part of the group
			if (response["entities"].has("brain_part")) {
				string brainPart = response["entities"]["brain_part"]["value"];
				DivideCommand(brainPart);
			}
			else {
				string outputText = "Sorry, I didn't understand the brain part that you want to separate."
				VoiceCommand(outputText);
			}
			break;
		case "select_object":
			if (response["entities"].has("brain_part")) {
				string brainPart = response["entities"]["brain_part"]["value"];
				SelectCommand(brainPart);
			}
			else {
				string outputText = "Sorry, I didn't understand the brain part that you want to select."
				VoiceCommand(outputText);
			}
			break;
		case "turn_object":
			// defining the default value
			string orientation = "right";

			if (response["entities"].has("orientation")) {
				orientation = response["entities"]["orientation"]["value"];
				
			}
			TurnCommand(orientation);
			break;
		case "focus_group":
			FocusCommand();
			break;
		default:
			string outputText = "Sorry, didn't understand your intent."
			VoiceCommand(outputText);
			break;
		}


	}
}