# Hackathon-F8-2021

## About the project
**TeachAR** is an educational AR application that allows kids to learn by experimenting anywhere and anytime, interacting with a voice assistant. This project was developed for the [**F8 Refresh: Hackathon 2021**](https://f82021.facebookhackathons.com/) and it is not intended for commercial use. Our motivation was to help kids to learn new topics through the use of technology. We see this necessary on these days, when the number of kids being home-schooled has increased significantly since the COVID-19 pandemic. 

## Supported Devices
Devices used to run this application should necessarily have access to a back camera, microphone and sound output available.

Before installing the app on your device you should check if it is supported for Vuforia. You can check if you own a supported device for the Ground Plane feature using this [link](https://library.vuforia.com/platform-support/vuforia-engine-recommended-devices.html).

We tested the application on iOS and Android devices supported by Vuforia, but the APK generated is only for Android devices.

## Available modules
For the hackathon we developed two learning experiences: brain parts and solar system. 

### Brain parts commands:
Brain parts available are: cerebellum, brain stem and parietal, frontal, temporal and occipital lobes. Brain parts are also associated with colors, in case the user don't know yet the real names. You can use all the commands referring with the real name or with the color associated.

- Select: this command allows the user to select a brain part and putting it over a platform. You can activate this saying something like "select + <brain_part>" or "select + <color_part>".
- Information: you can ask for information about a specific brain part saying something like "tell me about <brain_part>" or "tell me about <color_part>". The information is received by voice.
- Rotate: this command allows you to rotate the brain parts located over the platform. You can choose the direction of the rotation saying something like "rotate to <direction>". Directions available are: up, down, right, left. The rotation will always be 90 ?? in the direction indicated by the user.
- Join: join command is used to merge elements over the platform to form a unified model. For this, you can try saying something like "join + <brain_part>" or "select + <color_part>".
- Remove: this command can be used to remove elements from the platform and send them to the background. You can try saying something like "remove + <brain_part>" or "select + <color_part>".

### Solar system commands:
Models available: the sun, mercury, venus, earth, mars, jupiter, saturn, uranus and neptune.

Both select and information commands will give the user information about the model selected and move it over the platform. The information is received by voice.

## Technology stack
- Unity: cross-platform game engine used to integrate all the components from the project. All the animations were developed here.
- Vuforia: augmented reality engine used to detect the plane where the user wants to place the objects. We integrated it directly with Unity and used the Ground Plane feature.
- Wit.ai: NLU interface developed by Facebook. We integrated it with Unity through HTTP requests. We also used the speech to text capabilities to process the voice interaction sent for the user and extract the entities and intents with the NLU model.
- IBM Watson Speech to Text: as we did not have an endpoint to perform text-to-speech with wit.ai, we decided to use the IBM Watson text-to-speech API, so that the virtual assistant of our application could respond to all the user's commands also by voice. We used the Unity SDK generated for IBM to integrate it directly with Unity.

## User privacy
Our application never stores any type of user data or their interaction with it. The only data stored are present on the side of Wit.ai upon receiving the instructions from the clients. This should not initially contain personal information, because it is not prompted to the user, but as system administrators it is possible to apply periodic maintenance tasks to remove sensitive records.

## Demo video
Demo video of the project can be found [here](https://www.youtube.com/embed/9TnfNIRXPTk)

## The team
This project was developed for: [David Hern??ndez](https://www.linkedin.com/in/david-hernandez-3a5592a1/), [Mar??a Victoria Jorge](https://www.linkedin.com/in/maria-victoria-jorge), [Carlos Spaggiari](https://www.linkedin.com/in/carlos-spaggiari-52b6988b/) and [Arturo Toro](https://www.linkedin.com/in/arturot1212/).

## Credits
- Planet models by [Akshat](https://sketchfab.com/shooter24994) and the Sun model by [SebastianSosnowski](https://sketchfab.com/SebastianSosnowski) are licensed under [Creative Commons Attribution](http://creativecommons.org/licenses/by/4.0/).
- Platform model by [Bill Nguyen](https://www.cgtrader.com/free-3d-models/architectural/decoration/scifi-pedestal-turntable) has Royalty Free License.
- Arcade Font by [Yuji Adachi](https://fontmeme.com/fonts/arcade-yuji-adachi-font/) has Free License.
- Clean Settings UI by [Landan Lloyd](https://assetstore.unity.com/packages/tools/gui/clean-settings-ui-65588).
- Animated Loading Icons by [David Stenfors](https://assetstore.unity.com/packages/2d/gui/icons/animated-loading-icons-47844).
- Brain Models by [David Hern??ndez](https://www.linkedin.com/in/david-hernandez-3a5592a1/).
