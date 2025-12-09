# Self-Hypnosis App - Finish Build Instructions

This document provides explicit instructions for an AI coder to finish the build of the **SelfHypnosisApp** project, integrate SQLite persistence, compile the APK, and push all source files and the APK into a GitHub repository named **SelfHypnosisApp2**.

---

## 1. Environment Setup
- Install .NET 8.0 SDK or higher
- Install MAUI workload:
  dotnet workload install maui
- Install Android SDK via your platform’s SDK manager

---

## 2. Project Structure Creation
(… full scaffolding script …)

---

## 3. Core Models and Services
(… SessionTemplate, IFrequencyEngine, INarrationEngine, ISessionPlayer …)

---

## 4. Implementations and Dependency Injection
(… FrequencyEngine, SessionPlayer, MauiProgram.cs DI setup …)

---

## 5. SQLite Integration
- Add Microsoft.Data.Sqlite package
- Implement ISessionRepository and SqliteSessionRepository
- Register repository in DI

---

## 6. Build and Compilation
- Use build_android.sh script
- Or run:
  dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=apk

---

## 7. GitHub Repository Setup
git init  
git add .  
git commit -m "Initial commit with source and APK"  
git branch -M main  
git remote add origin https://github.com/<your-username>/SelfHypnosisApp2.git  
git push -u origin main  

---

## 8. Post-Build Verification
adb install bin/APK_Output/HypnosisApp.UI.apk

---

## ✅ Completion
Following these steps, the AI coder will scaffold the project, integrate SQLite, compile the APK, and push everything to GitHub under **SelfHypnosisApp2**.
