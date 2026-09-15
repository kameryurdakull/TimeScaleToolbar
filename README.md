# Time Scale Toolbar

Time Scale Toolbar adds a compact Scene view overlay for adjusting `Time.timeScale` while the Unity Editor is in Play Mode. It stays synchronized when another system changes the time scale and restores the default value when Play Mode exits.

## Features

- Scene view toolbar integration
- Live `0x` to `3x` time-scale control
- Current value display with culture-invariant formatting
- One-click reset to `1x`
- Synchronization with external `Time.timeScale` changes
- Automatic reset when leaving Play Mode
- Editor-only assembly with no runtime or third-party dependency

## Requirements

- Unity 6.0 or newer

## Installation

Open Unity Package Manager, select `+ > Install package from git URL...`, and enter:

```text
https://github.com/kameryurdakull/TimeScaleToolbar.git#v1.0.0
```

The equivalent `Packages/manifest.json` entry is:

```json
{
  "dependencies": {
    "com.kamer.time-scale-toolbar": "https://github.com/kameryurdakull/TimeScaleToolbar.git#v1.0.0"
  }
}
```

## Usage

1. Open a Scene view.
2. Enter Play Mode.
3. Adjust the `Time` slider or click `1x` to reset.

The overlay is displayed by default. If it is hidden, open the Scene view Overlays menu and enable `Time Scale`.

## Development

Run `npm pack --dry-run` from the repository root to inspect the distributable contents. Editor tests live under `Tests/Editor` and can be enabled by adding `com.kamer.time-scale-toolbar` to the host project's `testables` list.

See [Publishing](Documentation~/Publishing.md) for the release workflow.
