# Build Scripts

This directory contains build automation scripts for the SelfHypnosisApp project.

## Available Scripts

### `build_android.sh`

Main build script for compiling the SelfHypnosisApp.Core library.

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
3. Cleans previous build artifacts
4. Restores NuGet packages
5. Builds Debug configuration
6. Builds Release configuration
7. Reports build status and output locations

**Requirements:**

- Bash shell
- Internet connection (for downloading .NET SDK if needed)
- ~500MB disk space for .NET SDK

**Output:**

- Debug build: `SelfHypnosisApp/HypnosisApp.Core/bin/Debug/net8.0/`
- Release build: `SelfHypnosisApp/HypnosisApp.Core/bin/Release/net8.0/`

## Environment Variables

The build script respects the following environment variables:

- `DOTNET_ROOT` - Custom .NET SDK installation directory
- `PATH` - Will be updated to include .NET SDK if installed by script

## Troubleshooting

See [BUILD_INSTRUCTIONS.md](../../BUILD_INSTRUCTIONS.md) for detailed troubleshooting information.
