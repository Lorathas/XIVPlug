# XIVPlug
Simple ASP.NET web server to act as an interface layer between Triggernometry and Initface

## Setup
To get the project setup you're going to need a few things:  
1. Initface Desktop
2. Advanced Combat Tracker with the Final Fantasy XIV and Triggernometry plugins installed.
3. Visual Studio. This is a simple project to just run a web proxy between Triggernometry's JSON actions as a proof of concept mainly so there's no easy way to get this project running as a standalone application. Maybe version two will support that.
4. Whatever hardware you wish to use. Just make sure it is supported by 

After that just run the server, navigate to the index page, scan and select your device, and then setup Triggernometry to fire JSON blobs to either `{serverUrl}/api/plugs/queueaction` or `{serverUrl}/api/plugs/queueactions` if you want to queue up multiple commands.

The requests should look like one of the following:  
```
{ "Type": "Vibrate", "Value": 0.5, "DurationMillis": 500 }
```

or for multiple actions:  
```
[{ "Type": "Vibrate", "Value": 0.5, "DurationMillis": 500 }, { "Type": "Vibrate", "Value": 0.8, "DurationMillis": 500 }]
```

To break it down quickly `Value` is the intesnity between 0 and 1, and `DurationMillis` is the duration in milliseconds to execute the command.