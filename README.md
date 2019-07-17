# SpotSet API [![TravisCI](https://travis-ci.com/hyoyou/spotset_api.svg?branch=master)]

## Prerequisites
* C# (IDE like [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) - Free, [Visual Studio for Windows](https://visualstudio.microsoft.com/vs/) - Free, [JetBrains Rider](https://www.jetbrains.com/rider/) - Paid)
* .NET Core 2.2 (Download SDK from [Here](https://dotnet.microsoft.com/download))

## Setup Environment Variables

* To get an API key from setlist.fm:
    * *Skip this step if you already have an account with setlist.fm:
    * Sign up for an account [here](https://www.setlist.fm/signup)
    * Once logged in, create an application [here](https://www.setlist.fm/settings/apps) and fill out the form like below
    ![Creating an App on Setlist.fm](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.27.14+PM.png)
    Your API Key is displayed on the next page:
    ![Your API Key](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.28.05+PM.png)
    
* To get an API key and secret from Spotify:
    * *Skip this step if you already have an account with Spotify
    * Sign up for an account [here](https://www.spotify.com/us/signup/)
    * Sign in to the Developer Console [here](https://developer.spotify.com/dashboard/)
    * It should take you to your Dashboard. Click on the green button that says `Create a Client ID`
    
    ![Dashboard](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.32.10+PM.png)
    
    You should see a modal pop up, it will consist of 3 steps.
    * Step 1: Enter app name and short description. Check off `Website`
    
    ![Step 1](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.33.54+PM.png)
    
    * Step 2: Click `No` on the next step
    
    ![Step 2](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.34.38+PM.png)
    
    * Step 3: Check appropriate boxes and continue
    
    ![Step 3](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.34.58+PM.png)
    
    You Client ID should be displayed on the next page. Click on `Show Client Secret` to view your secret key.
    **Make note of these keys.**
    
    ![Key and Secret](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.35.36+PM.png)
    
    Click the green `Edit Settings` button on the upper right hand corner.
    Under `Redirect URIs`, add `http://localhost:3000`. Make sure to click the green `Add` button, and the green `Save` button on the bottom.
    
    ![Redirect Uri](https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.36.25+PM.png)

* To save these variables to your environment:
    * Navigate to the project directory in your terminal, then run these commands to save each variable:
    
   ```
   $ dotnet user-secrets set “SetlistApiKey" “<Your setlist.fm API Key>“
   $ dotnet user-secrets set “SpotifyApiKey" “<Your Spotify Client ID>“
   $ dotnet user-secrets set “SpotifyApiSecret” “<Your Spotify Client Secret>“
   ```
   

## Setup
* Clone this repo and `cd SpotSet.Api` once more into the project directory
* Use IDE to build and run, or to install dependencies, compile, and run on the command line, use following command:

```
$ dotnet run
```

## Running the Tests
* To run unit tests, you can run them from your chosen IDE or the following command:

```
$ dotnet test
```