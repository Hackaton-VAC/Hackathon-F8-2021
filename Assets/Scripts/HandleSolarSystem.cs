using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Speech;
using UnityEngine.UI;
using System;
public class HandleSolarSystem : MonoBehaviour
{
    public static bool has_new_audio = false;
    public static string outputText = "";
    public GameObject popup;
    public Text myText;
    public static bool help_action = false;
    public bool successAction = false;
    public string title;
    public string planet;
    public string intent = "";

    public Platform platform;
    Dictionary<string, int> planetMap = new Dictionary<string, int> {
        { "Mercury", 1},
        { "Venus", 2},
        { "Earth", 3},
        { "Mars", 4},
        { "Sun", 5},
        { "Jupiter", 6},
        { "Saturn", 7},
        { "Uranus", 8},
        { "Neptune", 9},
    };

    private string openHelpText = "Hold the button and try one of these phrases!";
    private string closeHelpText = "Closing help";

    public void Update()
    {
        if (help_action && outputText == openHelpText)
        {
            popup.SetActive(true);
            help_action = false;
        }
        if (outputText == closeHelpText)
        {
            popup.SetActive(false);
            help_action = false;
        }
        if (has_new_audio)
        {
            myText.text = title;
            StartCoroutine(audioListener.Speech(HandleSolarSystem.outputText));
            HandleSolarSystem.has_new_audio = false;
            MyButton.resetButton = true;
        }
    }

    public void Start()
    {


    }

    public void HandleMe(string textToParse)
    {
        audioListener.Start();
        var response = JSON.Parse(textToParse);
        print("SimpleJSON: " + response.ToString());

        intent = response["intents"][0]["name"].Value.ToLower();

        // possible values for brain_part entity: brainstem, temporal, occipital,
        // parietal, frontal, cerebellum

        // what function should I call?
        title = "";
        successAction = false;
        switch (intent)
        {
            case "about_object":
                // When the user wants to know info about a specific brain part
                if (response["entities"]["planet:planet"] != null && ("group" != response["entities"]["planet:planet"][0]["value"]))
                {
                    planet = response["entities"]["planet:planet"][0]["value"];
                    platform.focus(planetMap[planet]);
                    switch (planet)
                    {
                        case "sun":
                            title = "The Sun";
                            outputText = @"At the center of the solar system is a star called the Sun. It is the largest object in the solar system. Living things on Earth depend on its energy, in the form of light and heat.";
                            break;
                        case "mercury":
                            title = "Mercury";
                            outputText = @"Mercury is the closest planet to the Sun. It travels around the Sun at a faster speed than any other planet.";
                            break;
                        case "venus":
                            title = "Venus";
                            outputText = @"Venus is the brightest planet in the sky when viewed from Earth. It is Earth’s nearest neighbor, coming closer to Earth than any other planet. Venus is the second planet from the Sun. It is the hottest planet in the solar system.";
                            break;
                        case "earth":
                            title = "Earth";
                            outputText = @"The Earth is the third planet from the Sun and appears bright and bluish when seen from outer space. Earth is the only planet in the solar system that can support life.";
                            break;
                        case "mars":
                            title = "Mars";
                            outputText = @"Mars is the fourth planet from the Sun. It is also Earth’s outer neighbor. It has two small, rocky moons, Phobos and Deimos.";
                            break;
                        case "jupiter":
                            title = "Jupiter";
                            outputText = @"Jupiter is the largest planet in the solar system. It is bigger than all the other planets put together and is the fifth planet from the Sun.";
                            break;
                        case "saturn":
                            title = "Saturn";
                            outputText = @"Saturn is the second largest planet in the solar system, after Jupiter. It is known for its beautiful rings. Saturn is the sixth planet from the Sun.";
                            break;
                        case "uranus":
                            title = "Uranus";
                            outputText = @"Uranus was the first planet to be discovered after the invention of the telescope. It is the seventh planet from the Sun.";
                            break;
                        case "neptune":
                            title = "Neptune";
                            outputText = @"Neptune is a huge, distant planet that is deep blue in color. It is a stormy world. The planet has the fastest winds ever discovered in the solar system. Neptune is the farthest planet from the Sun.";
                            break;


                        default:
                            print("Auxilio pa dentro");
                            outputText = "Sorry, I didn't understand about what planet you want information about.";
                            title = "";
                            break;
                    }
                    print("Auxilio 1");
                    //StartCoroutine(audioListener.Speech(outputText));
                }
                else
                {
                    outputText = "Sorry, I didn't understand about what planet you want information about.";
                    print("Auxilio 2");

                    //StartCoroutine(audioListener.Speech(outputText));
                }
                break;


            case "open_help":
                outputText = "Hold the button and try one of these phrases!";
                help_action = true;
                title = "";
                //FocusCommand();
                break;
            case "close_help":
                outputText = closeHelpText;
                help_action = true;
                title = "";
                //FocusCommand();
                break;
            default:
                title = "";
                outputText = "Sorry, didn't understand your intent.";
                print("Auxilio 5");
                //StartCoroutine(audioListener.Speech(outputText));
                break;
        }

        Debug.Log("SALGO DE AQUI");
        has_new_audio = true;
    }
}