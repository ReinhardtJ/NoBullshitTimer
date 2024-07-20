# NoBullshitTimer

This timer ist mainly intended for sports. It can be used as an interval timer
for HIIT, LIIT or Tabata. It can also be used as a round timer for combat sports /
martial arts. You can even use it to create a pomodoro timer - sky is the limit.

## Visit
https://timer.reinhardt.ai

## Why did I develop this app?

After buying the iPhone 13 in 2022, I have quickly noticed a problem when 
trynig to do an interval workout: 
my go-to interval timer app (https://www.hiitmi.com/)
was not available. Instead, there was a set of either overly expensive apps or
apps whose usability drove me insane more than just one time.

Looking into the web, the situation didn't get better. Most websites simply
didn't work or had arbitrary constraints ("you can't set more than X rounds",
"one round can't last more than Y minutes"),
bad UX, a lot of advertisement, etc.

## No Bullshit

A solution was needed, and that solution was ~~to switch back to Android~~ 
to develop the No Bullshit Timer ;) The core motto for
this timer is: _No Bullshit_. An interval timer is an incredibly simple thing,
and I expect it to just work. And this is what this project is aiming to do.

And yeah, I also switched back to Android.

## Setup

The timer is a .NET Blazor WebAssembly Application. This has a variety of
implications, which can be read about in detail at https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-webassembly.

The app uses the following render- and hosting models:
- Render Model: Blazor WebAssembly (WASM), Client Side Rendering
- Hosting Model: Client Hosted

The reason why I chose this specific technology is, that the timer should
eventually also work as a standalone offline app. There is no reason for an
interval timer to require an internet connection.

To host the application for development, simply open the solution in Visual
Studio or Rider and use the `NoBullshitTimer.Server: http` run configuration
which will start a local development server. 

Styling relies on the Tailwind.css framework, which has an own CLI that needs
to be run to enable hot reloading: 

```bash
cd Client
npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --watch
```

to minify the stylesheets:
```bash
npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --minify
```

To run the timer in docker, refer to the https://github.com/ReinhardtJ/reinhardt.ai
repository.

## Planned Features
- [ ] Add support for circular sets
- [ ] Add a "full control" mode to explicitly define every single interval 
- [ ] Switch between voice commands and boxing-gym-timer-style "beep" noises
- [ ] Save and load workout plans (to local storage)
- [ ] Save and load workaut plans (between devices)
- [ ] Add presets to quickly create timers (tabata, boxing, pomodoro etc.)
- [ ] Add custom voice packs
