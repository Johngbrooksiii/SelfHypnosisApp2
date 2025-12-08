# Build Instructions for SelfHypnosisApp

## Overview

This document provides step-by-step instructions for building and compiling the SelfHypnosisApp project.

## Prerequisites

### Required Software

- **Git** (for cloning the repository)
- **.NET SDK 8.0** (will be auto-installed by the build script if not present)
- **Bash shell** (available on Linux, macOS, WSL, or Termux)

### Supported Environments

- Linux (Debian, Ubuntu, etc.)
- Termux + Debian-in-Termux (Android)
- macOS
- Windows (via WSL or Git Bash)

## Quick Start

### Option 1: Using the Build Script (Recommended)

The easiest way to build the project is using the provided build script:

```bash
# Navigate to the project root
cd /path/to/SelfHypnosisApp

# Make the build script executable (first time only)
chmod +x scripts/build/build_android.sh

# Run the build script
bash scripts/build/build_android.sh
```

The script will automatically:
1. Check for .NET SDK installation
2. Install .NET SDK 8.0 if not found
3. Clean previous build artifacts
4. Restore NuGet packages
5. Build both Debug and Release configurations

### Option 2: Manual Build with dotnet CLI

If you prefer to build manually:

```bash
# Navigate to the solution directory
cd SelfHypnosisApp

# Restore NuGet packages
dotnet restore SelfHypnosisApp.sln

# Build Debug configuration
dotnet build SelfHypnosisApp.sln --configuration Debug

# Build Release configuration
dotnet build SelfHypnosisApp.sln --configuration Release
```

## Build Output

After a successful build, you'll find the compiled assemblies in:

- **Debug build**: `SelfHypnosisApp/HypnosisApp.Core/bin/Debug/net8.0/`
- **Release build**: `SelfHypnosisApp/HypnosisApp.Core/bin/Release/net8.0/`

Key files in the output directory:
- `HypnosisApp.Core.dll` - The main library assembly
- `HypnosisApp.Core.pdb` - Debug symbols
- `HypnosisApp.Core.deps.json` - Dependency information

## Termux Setup (Android)

For building on Android devices using Termux:

```bash
# Install Termux from F-Droid (recommended) or Google Play

# Install required packages
pkg install git proot-distro

# Install Debian
proot-distro install debian
proot-distro login debian

# Inside Debian, update and install .NET SDK
apt update
apt install -y wget

# Clone your repository
cd ~/storage/shared/git
git clone <your-repo-url>
cd SelfHypnosisApp

# Run the build script
bash scripts/build/build_android.sh
```

## Troubleshooting

### .NET SDK Not Found

If the build script can't find or install .NET SDK:

```bash
# Manually install .NET SDK 8.0
bash dotnet-install.sh --channel 8.0 --install-dir $HOME/.dotnet

# Add to PATH
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"

# Verify installation
dotnet --version
```

### Permission Denied

If you get "Permission denied" when running the build script:

```bash
chmod +x scripts/build/build_android.sh
```

### Build Fails with Missing Dependencies

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages again
cd SelfHypnosisApp
dotnet restore SelfHypnosisApp.sln --force
```

### Out of Memory (Termux/Low-RAM Devices)

```bash
# Build only one configuration at a time
cd SelfHypnosisApp
dotnet build SelfHypnosisApp.sln --configuration Release
```

## Continuous Integration

The build script is designed to work in CI/CD environments:

```yaml
# Example GitHub Actions workflow
- name: Build SelfHypnosisApp
  run: |
    chmod +x scripts/build/build_android.sh
    bash scripts/build/build_android.sh
```

## Project Structure

```
SelfHypnosisApp/
├── HypnosisApp.Core/              # Core library project
│   ├── Models/                    # Data models
│   │   └── SessionTemplate.cs
│   ├── Services/                  # Service interfaces and implementations
│   │   ├── FrequencyEngine.cs
│   │   ├── IFrequencyEngine.cs
│   │   ├── INarrationEngine.cs
│   │   ├── ISessionPlayer.cs
│   │   └── SessionPlayer.cs
│   └── HypnosisApp.Core.csproj   # Project file
└── SelfHypnosisApp.sln           # Solution file
```

## Clean Build

To perform a clean build (removes all previous build artifacts):

```bash
cd SelfHypnosisApp
dotnet clean SelfHypnosisApp.sln
dotnet build SelfHypnosisApp.sln --configuration Release
```

## Advanced Build Options

### Build Specific Configuration

```bash
# Debug build only
dotnet build --configuration Debug

# Release build only
dotnet build --configuration Release
```

### Verbose Output

```bash
# Get detailed build information
dotnet build --verbosity detailed
```

### Parallel Builds

```bash
# Use multiple CPU cores for faster builds
dotnet build --configuration Release /maxcpucount
```

## Next Steps

After building successfully:

1. Review the compiled assemblies in the output directory
2. Run unit tests (when available): `dotnet test`
3. Create a MAUI Android app project that references HypnosisApp.Core
4. Package for distribution

## Getting Help

- Check the main [build.md](build.md) for project overview
- Review error messages carefully - they often indicate the exact issue
- Ensure all prerequisites are installed and up to date

## License

MIT - See project root for full license information.
