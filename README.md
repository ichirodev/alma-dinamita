## Single-player Zombies FPS Blueprint made in Unity
---
### About
This repository contains the blueprint developed in Unity for a specific type of game, aiming to be a Zombies FPS that is played on SP, gameplay is inspired on the classic round-based mode from the CoD Zombies saga but excluding between rounds time-out to give it a more aggressive gameplay.

*This repository includes mostly code for the logic for the mentioned project, no animations, no sounds, no models, only those needed for the project to compile.*

### What does this repo contains?
This repository contains code for:
#### Player
The player have the following abilities programmed:
* Basic movement for crouching, slow-walking, walking and sprinting.
* Fast sliding (No collition modification added, just logic)
* Jumping
* Basic collition and gravity
* Pick any type of weapons from anywhere (given a prefab asset)
* Inflict damage on enemies with any weapon

#### Weapons
* Automatic weapons logic (Used for primary weapons inside the project)
* Semiautomatic weapons logic (Used for secondary weapons inside the project)

Both weapons logic can:
* Shoot
* Reload

Weapons have a draw time programmed that can be used to add drawing animations

Automatic weapons have a recoil logic added like the one used for Counter-Strike 

#### Enemies
The enemies can chase a player and attack him causing damage to the player health, enemies have a logic for different action states:
* Patrolling
* Standing Still
* Chase
* Search/Look for the player

#### Spawns
Spawns can be used to spawn enemies with a certain frequency, limiting quantities and types of enemies, all this parameters can be changed for every spawn type created and it has no variant limits inside the Unity engine.

### How can I use this repo?
You can use this repository freely as long as you give me credit for the source code used.

### Requirements
The only thing you need to run and modify this project to your liking is Unity 2020.3.26f1 and a computer with the required specs to run it [More information here](https://unity3d.com/unity/whats-new/2020.3.26)