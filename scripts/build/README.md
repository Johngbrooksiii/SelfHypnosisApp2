# Build Scripts

This directory contains build automation scripts for the SelfHypnosisApp project.

## Available Scripts

### `build_android.sh`

Main build script for compiling the complete SelfHypnosisApp including the Core library and Android MAUI application.

**Usage:**

```bash
# From project root
bash scripts/build/build_android.sh

# Or make it executable and run directly
chmod +x scripts/build/build_android.sh
./scripts/build/build_android.sh
```

**What it does:**

1. Checks for .NET SDK 8.0 installation
2. Automatically installs .NET SDK if missing
3. Checks for .NET MAUI workload installation
4. Automatically installs MAUI workload if missing
5. Cleans previous build artifacts
6. Restores NuGet packages
7. Builds Debug configuration (Core + UI)
8. Builds Release configuration (Core + UI)
9. Generates Android APK (Release configuration)
10. Reports build status and output locations

**Requirements:**

- Bash shell
- Internet connection (for downloading .NET SDK and MAUI workload if needed)
- ~2GB disk space for .NET SDK and MAUI workload
- Android SDK (optional, for APK generation - will skip APK if not available)

**Output:**

- Core Library (Debug): `SelfHypnosisApp/HypnosisApp.Core/bin/Debug/net8.0/`
- Core Library (Release): `SelfHypnosisApp/HypnosisApp.Core/bin/Release/net8.0/`
- Android APK (Release): `SelfHypnosisApp/HypnosisApp.UI/bin/Release/net8.0-android/com.selfhypnosis.app-Signed.apk`

## Project Structure

The solution now contains two projects:

- **HypnosisApp.Core**: Cross-platform core library with business logic
- **HypnosisApp.UI**: .NET MAUI Android application with platform-specific implementations

## Environment Variables

The build script respects the following environment variables:

- `DOTNET_ROOT` - Custom .NET SDK installation directory
- `PATH` - Will be updated to include .NET SDK if installed by script
- `ANDROID_SDK_ROOT` - Android SDK location (optional, for APK generation)

## Installing Android SDK (Optional)

To generate APK files, you need the Android SDK installed:

### On Linux/Termux:

```bash
# Install Android SDK via sdkmanager
pkg install android-sdk
export ANDROID_SDK_ROOT=$HOME/android-sdk
```

### On macOS:

```bash
# Install via Homebrew
brew install --cask android-sdk
export ANDROID_SDK_ROOT=/usr/local/share/android-sdk
```

### On Windows (WSL):

Follow the official Android SDK installation guide for your platform.

## Troubleshooting

### MAUI Workload Installation Fails

If MAUI workload installation fails, try:

```bash
# Clear workload cache
dotnet workload clean

# Try installing again
dotnet workload install maui
```

### APK Generation Fails

This is normal if you don't have Android SDK installed. The Core library and UI project will still build successfully. To generate APKs:

1. Install Android SDK (see above)
2. Set ANDROID_SDK_ROOT environment variable
3. Run the build script again

### Out of Memory (Termux/Low-RAM Devices)

```bash
# Build only the core library
cd SelfHypnosisApp
dotnet build HypnosisApp.Core/HypnosisApp.Core.csproj --configuration Release
```

## Manual APK Build

If you want to build the APK manually:

```bash
cd SelfHypnosisApp
dotnet publish HypnosisApp.UI/HypnosisApp.UI.csproj \
  -f net8.0-android \
  -c Release \
  -p:AndroidPackageFormat=apk
```

The APK will be located in:
`HypnosisApp.UI/bin/Release/net8.0-android/com.selfhypnosis.app-Signed.apk`

## Deploying to Device

To install the APK on an Android device:

```bash
# Via ADB
adb install path/to/com.selfhypnosis.app-Signed.apk

# Or copy to device and install manually
```

See [BUILD_INSTRUCTIONS.md](../../BUILD_INSTRUCTIONS.md) for detailed troubleshooting information.
