# CountdownWidgetApp

A simple .NET MAUI application that lets you manage multiple countdown events and display a selected countdown on your Android home screen as a widget. Built to run on devices like the Samsung S23, this app keeps you updated on upcoming dates—showing days left (or days, hours, and minutes) and even switching to minute‑precision in the final 48 hours.

## Features

* **Multiple Events**: Keep track of as many events as you like in a scrolling list.
* **Custom Display Formats**: Choose to show each countdown in “days only” or “days, hours & minutes.”
* **Home‑Screen Widget**:

  * Pick your “Primary Event” to pin in a single widget.
  * Choose widget size: Small (1×1), Medium (2×2), or Large (4×2).
  * Optional auto‑rotation: cycle through your next events at a chosen interval.
  * Minute‑precision kicks in automatically in the last 48 hours.
* **Persistence & Reliability**:

  * Events and settings are stored using `Preferences` (survives app restarts and device reboots).
  * Receives `BOOT_COMPLETED` and `MY_PACKAGE_REPLACED` broadcasts to re‑schedule widget updates after reboot or app update.

## Screenshots

<!-- Add screenshots of EventsPage, EditEventPage, SettingsPage, and widget in action -->

## Getting Started

### Prerequisites

* [.NET 7 SDK (or .NET 6) with MAUI workload](https://docs.microsoft.com/dotnet/maui/get-started/installation)
* Android SDK (API Level 31+)
* A Samsung S23 (or emulator) running Android

### Clone the Repo

```bash
git clone https://github.com/yourusername/CountdownWidgetApp.git
cd CountdownWidgetApp
```

### Build & Run

1. **Restore workloads**:

   ```bash
   ```

dotnet workload restore

````

2. **Build and deploy to your device**:
   ```bash
dotnet build -t:Run -f net7.0-android
````

3. **Add the widget**: Long‑press your home screen → Widgets → “Countdown Widget” → drop it on your home.

4. **Configure events**: Open the app, tap **Events**, add/edit dates, then go to **Settings** to pick your Primary Event and widget options.

## Project Structure

```
CountdownWidgetApp/
├─ AppShell.xaml          # App navigation (Tabs: Events, Settings)
├─ Pages/
│  ├─ EventsPage.xaml     # List of events
│  ├─ EditEventPage.xaml  # Add/Edit event UI
│  ├─ SettingsPage.xaml   # Widget config UI
├─ Models/
│  └─ EventItem.cs        # Data model (Name, Date, DisplayFormat, RemainingTime)
├─ Services/
│  └─ EventService.cs     # Load/save events
│  └─ SettingsService.cs  # Persist widget settings
├─ Platforms/Android/
│  ├─ Resources/xml/
│  │   └─ countdown_widget_info.xml
│  ├─ Resources/layout/
│  │   └─ widget_countdown.xml
│  └─ CountdownWidgetProvider.cs
└─ README.md              # You are here
```

## Contributing

1. Fork the repo.
2. Create your feature branch (`git checkout -b feature/MyFeature`).
3. Commit your changes (`git commit -m "Add MyFeature"`).
4. Push to your branch (`git push origin feature/MyFeature`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License. Feel free to use, modify, and share!
