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
    
    print_success "Build process completed successfully!"
    echo ""
    print_info "Build artifacts location:"
    print_info "  Debug:   $PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.Core/bin/Debug/net8.0/"
    print_info "  Release: $PROJECT_ROOT/SelfHypnosisApp/HypnosisApp.Core/bin/Release/net8.0/"
    echo ""
    echo -e "${BLUE}================================================${NC}"
}

# Run main function
main "$@"
