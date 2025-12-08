# SelfHypnosisApp

Modular, reproducible, and automation-friendly self-hypnosis app designed for Android using .NET MAUI.  
This app leverages audio entrainment, reflex-layered neural pipelines, and SQLite logging to support stress relief and autonomic regulation.

---

## 🧠 Purpose
Enable frictionless, offline-first self-hypnosis sessions with reproducible state, minimal memory footprint, and full control over assets and logic.

---

## 📁 Project Structure
markdown

SelfHypnosisApp

Modular, reproducible, and automation-friendly self-hypnosis app designed for Android using .NET MAUI.  
This app leverages audio entrainment, reflex-layered neural pipelines, and SQLite logging to support stress relief and autonomic regulation.

---

🧠 Purpose
Enable frictionless, offline-first self-hypnosis sessions with reproducible state, minimal memory footprint, and full control over assets and logic.

---

📁 Project Structure
`
SelfHypnosisApp/
├── assets/          # Static images, icons, fonts
├── audio/           # Hypnosis tracks, entrainment loops
├── build/           # Build scripts and toolchain configs
├── docs/            # Markdown documentation and architecture notes
├── Models/          # Data models (e.g., SessionLog.cs)
├── Services/        # Background services (e.g., AudioPlayerService.cs)
├── src/             # Main app logic and UI
├── templates/       # Externalized session templates
├── tests/           # Unit and integration tests
└── scripts/         # Shell scripts for setup and automation
`

---

⚙️ Setup Instructions

Termux + Debian-in-Termux
`bash

Install dependencies
pkg install git proot-distro
proot-distro install debian
proot-distro login debian
apt update && apt install -y dotnet-sdk-8.0 sqlite3
`

Clone and Initialize
`bash
cd ~/storage/shared/git
git clone https://your-local-repo/SelfHypnosisApp.git
cd SelfHypnosisApp
bash scripts/init.sh
`

---

🔁 Features
- Modular audio entrainment engine  
- SQLite session logging with reproducible state  
- Reflex-layered neural pipeline with autonomic watcher  
- Offline-first architecture with local asset fallback  
- Copy-paste setup blocks for instant deployment  

---

🧪 Testing
`bash

Run tests
cd tests
bash run_tests.sh
`

---

🧩 Integration Notes
- Designed for mobile-only workflows  
- Git-based local backup to SD card  
- Compatible with VS Code via code-server  
- Clipboard-safe script blocks for Android  

---

📜 License
MIT — reclaim your agency, remix freely.

---

✨ Author
John — hacker-philosopher reclaiming control through automation.

---

🧭 Next Steps
- [ ] Finalize MAUI UI bindings  
- [ ] Integrate SQLite logging into reflex pipeline  
- [ ] Validate toolchain health in Debian-in-Termux  
- [ ] Auto-init pseudo-RAM grid and watcher  
`
