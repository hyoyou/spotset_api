# SpotSet API [![Build Status](https://travis-ci.com/hyoyou/spotset_api.svg?branch=master)](https://travis-ci.com/hyoyou/spotset_api)

## Prerequisites
* .NET Core 3.0 (Download SDK from [Here](https://dotnet.microsoft.com/download))
* For C#: Not required, but convenient to use an IDE like [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) - Free, [Visual Studio for Windows](https://visualstudio.microsoft.com/vs/) - Free, [JetBrains Rider](https://www.jetbrains.com/rider/) - Paid

## Setup Environment Variables
* To get an API key from setlist.fm:
    * Sign up for an account [here](https://www.setlist.fm/signup)
    ***Skip** this step if you already have an account with setlist.fm
    
    * Once logged in, create an application [here](https://www.setlist.fm/settings/apps) and fill out the form like below
    <p align="center">
      <img width="650" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.27.14+PM.png">
    </p>
    Your API Key is displayed on the next page:
    <p align="center">
      <img width="650" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.28.05+PM.png">
    </p>
    
* To get an API key and secret from Spotify:
    * Sign up for an account [here](https://www.spotify.com/us/signup/)
    ***Skip** this step if you already have an account with Spotify
    * Sign in to the Developer Console [here](https://developer.spotify.com/dashboard/)
    * It should take you to your Dashboard. Click on the green button that says `Create a Client ID`
    <p align="center">
      <img width="650" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.32.10+PM.png">
    </p>
    You should see a modal pop up, it will consist of 3 steps.
    * Step 1: Enter app name and short description. Check off `Website`
    <p align="center">
      <img width="300" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.33.54+PM.png">
    </p>
    
    * Step 2: Click `No` on the next step
    <p align="center">
      <img width="300" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.34.38+PM.png">
    </p>
    
    * Step 3: Check appropriate boxes and continue
    <p align="center">
      <img width="300" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.34.58+PM.png">
    </p>
    
    You Client ID should be displayed on the next page. Click on `Show Client Secret` to view your secret key.
    **Make note of these keys.**
    <p align="center">
      <img width="300" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.35.36+PM.png">
    </p>
    
    Click the green `Edit Settings` button on the upper right hand corner.
    Under `Redirect URIs`, add `http://localhost:3000`. Make sure to click the green `Add` button, and the green `Save` button on the bottom.
    <p align="center">
      <img width="300" src="https://spotset.s3.amazonaws.com/Screen+Shot+2019-07-16+at+5.36.25+PM.png">
    </p>
   
* Clone this repo and `cd` into the project directory
* To save these variables to your environment:
    * Navigate to the project directory (the one housing the .csproj file) in your terminal (ie. If you're inside the root level `SpotSet.Api` solution directory containing the `SpotSet.Api` and `SpotSet.Api.Tests` directory, `cd` once more into the `SpotSet.Api` project directory), then run these commands to save each variable:
    
   ```
   $ dotnet user-secrets set "SetlistApiKey" "<Your setlist.fm API Key>"
   $ dotnet user-secrets set "SpotifyApiKey" "<Your Spotify Client ID>"
   $ dotnet user-secrets set "SpotifyApiSecret" "<Your Spotify Client Secret>"
   ```
   
   * You could check that the keys have been saved properly by running `dotnet user-secrets list`
   
## Setup
* Switch directory with `cd ..` back into the solution directory

* To start the app:

```
$ dotnet run --project SpotSet.Api
```

Press `ctrl + c` to stop.

## Running the Tests
* To run unit tests:

```
$ dotnet test
```
