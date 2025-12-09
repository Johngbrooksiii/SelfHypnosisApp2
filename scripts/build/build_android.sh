#!/usr/bin/env bash
# Build script for SelfHypnosisApp Android
# Compatible with Termux + Debian-in-Termux environment

set -e  # Exit on error
set -u  # Exit on undefined variable

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Script directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"

echo -e "${BLUE}================================================${NC}"
echo -e "${BLUE}  SelfHypnosisApp Build Script${NC}"
echo -e "${BLUE}================================================${NC}"
echo ""

# Function to print colored messages
print_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if dotnet is installed
check_dotnet() {
    print_info "Checking for .NET SDK..."
    
    if command -v dotnet &> /dev/null; then
        DOTNET_VERSION=$(dotnet --version)
        print_success ".NET SDK found: version $DOTNET_VERSION"
        return 0
    else
        print_warning ".NET SDK not found."
        return 1
    fi
}

# Check if MAUI workload is installed
check_maui_workload() {
    print_info "Checking for .NET MAUI workload..."
    
    if dotnet workload list | grep -q "maui"; then
        print_success ".NET MAUI workload found"
        return 0
    else
        print_warning ".NET MAUI workload not found"
        return 1
    fi
}

# Install MAUI workload
install_maui_workload() {
    print_info "Installing .NET MAUI workload..."
    
    if dotnet workload install maui; then
        print_success ".NET MAUI workload installed successfully"
    else
        print_error ".NET MAUI workload installation failed"
        exit 1
    fi
}

# Install .NET SDK using the dotnet-install.sh script
install_dotnet() {
    print_info "Installing .NET SDK 8.0..."
    
    if [ -f "$PROJECT_ROOT/dotnet-install.sh" ]; then
        bash "$PROJECT_ROOT/dotnet-install.sh" --channel 8.0 --install-dir "$HOME/.dotnet"
        
        # Add to PATH for current session
        export PATH="$HOME/.dotnet:$PATH"
        export DOTNET_ROOT="$HOME/.dotnet"
        
        print_success ".NET SDK installed successfully"
    else
        print_error "dotnet-install.sh not found at $PROJECT_ROOT"
        exit 1
    fi
}

# Clean previous build artifacts
clean_build() {
    print_info "Cleaning previous build artifacts..."
    
    cd "$PROJECT_ROOT/SelfHypnosisApp"
    
    if command -v dotnet &> /dev/null; then
        dotnet clean SelfHypnosisApp.sln --configuration Debug > /dev/null 2>&1 || true
        dotnet clean SelfHypnosisApp.sln --configuration Release > /dev/null 2>&1 || true
        print_success "Clean completed"
    else
        print_warning "Skipping clean (dotnet not available)"
    fi
}

# Restore NuGet packages
restore_packages() {
    print_info "Restoring NuGet packages..."
    
    cd "$PROJECT_ROOT/SelfHypnosisApp"
    
    if dotnet restore SelfHypnosisApp.sln; then
        print_success "Package restore completed"
    else
        print_error "Package restore failed"
        exit 1
    fi
}

# Build the project
build_project() {
    local configuration=$1
    print_info "Building project in $configuration configuration..."
    
    cd "$PROJECT_ROOT/SelfHypnosisApp"
    
    if dotnet build SelfHypnosisApp.sln --configuration "$configuration" --no-restore; then
        print_success "$configuration build completed successfully"
    else
        print_error "$configuration build failed"
        exit 1
    fi
}

# Build Android APK
build_apk() {
    local configuration=$1
    print_info "Building Android APK in $configuration configuration..."
    
    cd "$PROJECT_ROOT/SelfHypnosisApp"
    
    if dotnet publish HypnosisApp.UI/HypnosisApp.UI.csproj \
        -f net8.0-android \
        -c "$configuration" \
        -p:AndroidPackageFormat=apk \
        -p:AndroidSdkDirectory="$ANDROID_SDK_ROOT" 2>/dev/null || \
       dotnet publish HypnosisApp.UI/HypnosisApp.UI.csproj \
        -f net8.0-android \
        -c "$configuration" \
        -p:AndroidPackageFormat=apk; then
        print_success "$configuration APK build completed successfully"
        
        # Find and report APK location
        local apk_path=$(find "$PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.UI/bin/$configuration/net8.0-android" -name "*.apk" | head -n 1)
        if [ -n "$apk_path" ]; then
            print_info "APK location: $apk_path"
        fi
    else
        print_warning "$configuration APK build failed (this is normal if Android SDK is not configured)"
        print_info "Library build was successful. APK generation requires Android SDK."
    fi
}

# Main build process
main() {
    print_info "Starting build process..."
    print_info "Project root: $PROJECT_ROOT"
    echo ""
    
    # Check and install .NET SDK if needed
    if ! check_dotnet; then
        install_dotnet
        check_dotnet || {
            print_error "Failed to install .NET SDK"
            exit 1
        }
    fi
    
    echo ""
    
    # Check and install MAUI workload if needed
    if ! check_maui_workload; then
        print_warning "MAUI workload not installed. Installing now..."
        install_maui_workload
        check_maui_workload || {
            print_error "Failed to install MAUI workload"
            exit 1
        }
    fi
    
    echo ""
    
    # Clean previous builds
    clean_build
    echo ""
    
    # Restore packages
    restore_packages
    echo ""
    
    # Build Debug configuration
    build_project "Debug"
    echo ""
    
    # Build Release configuration
    build_project "Release"
    echo ""
    
    # Build Android APK (Release only)
    print_info "Attempting to build Android APK..."
    build_apk "Release"
    echo ""
    
    print_success "Build process completed successfully!"
    echo ""
    print_info "Build artifacts locations:"
    print_info "  Core Library (Debug):   $PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.Core/bin/Debug/net8.0/"
    print_info "  Core Library (Release): $PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.Core/bin/Release/net8.0/"
    print_info "  Android APK (Release):  $PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.UI/bin/Release/net8.0-android/"
    echo ""
    echo -e "${BLUE}================================================${NC}"
}

# Run main function
main "$@"
