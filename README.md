# Cosmic Shatter 🚀

**Cosmic Shatter** is a fully playable, cross-platform arcade shooter developed in Unity (C#). Originally designed as a desktop experience, the application has been engineered to seamlessly support mobile web browsers with dynamic touch controls and responsive UI scaling. 

🎮 **[Play the Game Live Here!](https://chadcrowley.tech.github.io/Cosmic-Shatter/)**

## 🛠️ Technical Highlights
This project demonstrates a complete software development lifecycle, from writing core game logic to managing cross-platform web deployment. Key technical achievements include:

* **Hybrid Input System:** Custom C# event handlers that detect the active operating system (PC vs. Mobile) and seamlessly swap between standard keyboard tracking and continuous UI touch inputs.
* **Responsive UI Architecture:** Engineered Canvas scaling and anchored elements that maintain aspect ratio integrity across standard desktop monitors and smartphone screens.
* **Orientation Management:** Built-in viewport monitoring that enforces landscape orientation on mobile web browsers to protect the user experience and prevent UI clipping.
* **State Management:** Robust `GameManager` logic handling wave progression, life tracking, enemy spawning, and secure leaderboard data.
* **HTML/WebGL Optimization:** Custom HTML viewport metadata injection to prevent mobile browsers from intercepting game touches with native swipe/zoom gestures.

## 💻 Built With
* **Engine:** Unity 
* **Language:** C#
* **Deployment:** WebGL (Mobile/Desktop Web), Windows Native (.exe)

## 🕹️ Controls
* **Desktop:** `W` or `Up Arrow` to Thrust, `A/D` or `Left/Right Arrows` to Turn, `Left Click` to Shoot, `Right Click` for Hyperspace.
* **Mobile:** Intuitive on-screen dual-thumb controls (Auto-detected upon loading via mobile browser).

## 🚀 Local Installation (Windows Native)
For the highest fidelity experience, a native Windows build is available:
1. Clone the repository or download the latest Windows Build folder.
2. Extract the files to your local machine.
3. Run `CosmicShatter.exe`.
