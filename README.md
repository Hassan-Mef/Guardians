# 🎮 Guardians: Tower Defense Game (DSA Project in Godot with C#)

## 📘 Overview

**Guardians** is a Tower Defense game developed using the **Godot Engine** with **C#** and **GDScript**, created as part of a university project to implement **core Data Structures and Algorithms (DSA)** in a practical, game-based environment. The game focuses on defending a base from waves of enemies while utilizing custom-built data structures to manage game behavior.

---

## 🏗️ Game Architecture

### 📂 Scenes Overview

- **`Game.tscn`**: Main scene. Initializes the game, loads sub-scenes (Map, Spawner, Guild), and manages game state.
- **`Map.tscn`**: Contains the game environment and the base to protect.
- **`Spawner.tscn`**: Manages enemy creation and spawn logic.
- **`Guild.tscn`**: Displays guild-related UI (optional).
- **`EndingScene.tscn`**: Final game-over screen showing the final score and retry option.

### 🎮 Gameplay Flow

1. Enemies spawn from specific tiles.
2. They path toward the base and deal damage upon contact.
3. Player accumulates score when enemies are defeated.
4. Game ends when base health reaches zero.
5. Final score is displayed on `EndingScene`.

---

## 🧮 Implemented Data Structures

### ✅ Custom Queue

- **Class**: `Queue<T>` (linked-list based)
- **Usage**: Used in the Spawner to preload and dequeue enemies in FIFO order.
- **Purpose**: Demonstrates queue-based enemy wave management.
- **Location**: `Spawner.cs`

```csharp
enemyQueue.Enqueue(enemyInstance);
var enemy = enemyQueue.Dequeue();
```

---

### ✅ Custom Hash Table

- **Class**: `HashTable`
- **Usage**: Maps enemy types (e.g., "FastEnemy") to score values.
- **Purpose**: Used by `ScoreManager` to reward points based on enemy type.
- **Collision Resolution**: Open Addressing with Linear Probing.

```csharp
enemyRewards.Insert("SlowEnemy", 10);
int reward = enemyRewards.Search("FastEnemy");
```

---

### ✅ Score Manager

- **Scene**: `ScoreManager.tscn`
- **Singleton Node**: Accessed across scenes to track score.
- **Integration**: Updates score on enemy death and displays it on game over.
- **Uses**: Custom hash table internally.

```csharp
scoreManager.AddScore("TankEnemy");
int finalScore = scoreManager.GetScore();
```

---

## 🧠 Code Structure Highlights

### `Game.cs`
- Loads all core scenes (Map, Spawner, Guild).
- Connects to base’s `BaseDestroyed` signal.
- On game over, transitions to `EndingScene` and passes score.

### `Base.cs`
- Manages base health.
- Emits signal when health drops to zero.
- Integrates with `ProgressBar` UI.

### `Spawner.cs`
- Manages enemy spawn timing using a `Timer`.
- Uses **custom queue** to spawn enemies in order.

### `EnemyBase.cs`
- Handles logic for when an enemy dies.
- Communicates with `ScoreManager` to award score.

### `ScoreManager.cs`
- Central score tracking logic.
- Stores enemy type rewards using **custom hash table**.

### `EndingScene.cs`
- Displays final score using a `Label` (`FinalScoreLabel`).
- Contains a retry button to return to main menu.

---

## 💻 Technologies Used

- **Game Engine**: Godot 4.x (C# scripting)
- **Language**: C# (.NET 6)
- **UI**: Godot Control Nodes (Label, Button, ProgressBar)
- **Audio**: Background music integrated using `AudioStreamPlayer`

---

## 🏁 How the Game Ends

- When the base is destroyed, `BaseDestroyed` signal triggers `OnBaseDestroyed` in `Game.cs`.
- The score is retrieved from `ScoreManager` and passed to `EndingScene`.
- `EndingScene` displays the score using a label.

```csharp
endingScene.UpdateFinalScore(finalScore);
```

---

## 🎯 Educational Objectives Achieved

- ✅ Applied **Queue** (FIFO logic) in enemy spawning.
- ✅ Applied **Hash Table** to score management and retrieval.
- ✅ Designed custom data structures from scratch.
- ✅ Integrated data structures in real-time game logic.
- ✅ Built and transitioned between modular scenes in Godot.
- ✅ Signal-based architecture and event handling in C#.

---

## 🚀 Future Scope

- Add tower placement & enemy pathfinding.
- Implement additional DSA concepts (Stack, Heap, Graph).
- Save/load high scores.
- Multiplayer support or difficulty scaling.

---

## 📸 Screenshots



---

## ✨ Credits

- Developed by: Hassan-Mef , Aliyan Waseem , Shaheer , Feham
- University Project - Data Structures & Algorithms (DSA)
- Engine: Godot Engine + C#

---

## 📁 Project Directory Structure

```
Guardians/
├── Scripts/
│   ├── Game.cs
│   ├── Base.cs
│   ├── Spawner.cs
│   ├── EnemyBase.cs
│   ├── ScoreManager.cs
│   ├── CustomDataStructures/
│       ├── Queue.cs
│       ├── HashTable.cs
├── Scenes/
│   ├── Game.tscn
│   ├── Map.tscn
│   ├── Spawner.tscn
│   ├── EndingScene.tscn
│   ├── ScoreManager.tscn
│   └── Guild.tscn
├── Assets/
│   ├── Sounds/
│   └── Textures/
└── README.md
```

---

## 📌 Note

This project is intentionally focused on **demonstrating DSA in practice**, so some design choices prioritize **academic integration** over full game polish.
