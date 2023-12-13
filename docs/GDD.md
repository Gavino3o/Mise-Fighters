# Mise Fighters Game Development Document

**Mise Fighters is a Local Multiplayer Co-op Hack & Slash RPG.**<br>
The game lets you play as a chef, a butcher, a bartender or a pattisier on a mission to withstand the fury of vengeful food ingredients that have mysteriously come to life to seek revenge for their cooked brethen.<br>
While Hack & Slash games often exhibit a gory and intesne atmosphere, Mise Fighters is infused with humour and light-heartedness while maintaining a thrilling action-packed gameplay.

<br>

## Problem Motivation

Drawing inspiration from beloved hack and slash titles like Hades and Hyper Light Drifter, we set out to create a whimsical online multiplayer cooperative hack and slash experience. Our game introduces a team of a chef, a butcher, a bartender and a pâtissier on a mission to withstand the fury of vengeful food ingredients that have mysteriously come to life. While hack and slash games often exhibit a gory and intense atmosphere, we wanted to infuse our game with humour and light-heartedness while maintaining a thrilling action-packed gameplay.

In our unique culinary adventure, players are not only encouraged but also rewarded for synergizing their skills to unleash mesmerizing combos and amusing effects against the culinary adversaries. By seamlessly combining their culinary prowess, the 4 friends can overcome the challenges and obstacles thrown their way.

<br>

## Overview

### Game Flow

In the main menu, players can customise their controls/keybinds before choosing to either host a game or join a game. They can also access a character guide menu to read about the playable characters’ skills.

Upon connecting as a host or a client, both players can choose their characters in a character selection lobby, after which loading into the first stage. There are 3 stages in total, each stage increasing in difficulty and culminating in a boss challenge at the end. Enemies will spawn in waves and target the players, and they have to work together to defeat the horde. Once all waves of enemies are defeated the players can proceed to the next stage.

For combat, every character firstly has a unique auto-attack that fires automatically, their primary method of doing damage to enemies. Each character also has an arsenal of skills/spells that can be cast, each with unique effects and cooldowns. Enemies on the other hand deal damage either by firing projectiles of their own or directly colliding with the player. Further depth is added to combat through a status effect system, where players and enemy units alike can be afflicted with buffs or debuffs, providing additional options for dealing with enemies.

At the end of the final stage, there will be a boss fight with a tanky, hard-hitting enemy that has access to multiple moves and attack patterns. Defeating the boss would result in a victory for the players.

Upon victory or defeat, players will be brought back to the main menu, where they can play again with different characters.

### Units

Units in the game have 4 core stats: Health Points(HP), Attack Power(ATK), Attacks Per Second, and Movement Speed. Both Characters and Enemies are considered units.

They can also be afflicted with status effects. When afflicted by status effects, there will be simple sprite animations so that it is obvious to the player.

| Name | Effect |
|------|--------|
| Taunt | Enemies change targets to the object |
| Slow (CYAN) | Movement speed is by 50% |
| Freeze (BLUE) | Enemies are unable to move |
| Flattened (INVERTED SPRITE) | Movement speed is decreased by 50%, Attack Power is reduced by 50% |
| Douse/Burn (GREY/RED) | Douse: Attack on the unit is increased by 50% <br> Burn: Damage over time debuff is inflicted. If target is doused, an additional 7 Ignition Damage is inflicted |
| Buff (GREEN) | Attack Power is increased by +50% (affects all scaling) |

### Playable Characters

_Insert Image here_

4 characters with varying play styles and multiple cooperative options to deal with enemy waves. (See Appendix A for the detailed rundown on the characters’ stats and movesets)

Each character will have original hand-drawn sprites and animations for their various skills and movement.

### Enemy variety

The game has various enemy types that will spawn in waves. The enemies target the players and use AStar pathfinding algorithms to advance on the players. Different enemies have different attacks and behaviours to provide some strategic depth to how players want to deal with them. (See Appendix B for the detailed rundown on the enemies’ stats, move sets and behaviours)

Enemy waves have different enemy amounts and enemy variety, and are designed to provide a scaling difficulty curve as the game progresses. Enemies are spawned from various spawn points around the arena and will path find towards and attack the characters.

### Environments

The game will have different stages for the players to fend off the horde in. The environment can pose an added challenge to the players by virtue of both restricting the space available to kite enemies around the arena as well as obstacles that can hinder/slow movement.

#### Stages

1. **The Restaurant**
   - In this stage there should not be any stage hazards. This stage is designed to have large area for ease of movement.
   - Some tables and chairs are present to obstruct movement of both players and enemies. It also includes an accessible kitchen and toilet area for players to explore.
2. **Market Area**
   - A colourful market area with various obstacles to obstruct movement or allow for kiting. This stage includes new enemy variants that will keep the players on their toes.
3. **Basement**
   - Final face off against the boss monster, in an arena stage. This stage includes a large open space for players to move around and dodge the bosses attacks.


### User Interface (UI)

The in game overlay will provide key information to the player while also being as unobtrusive as possible.

### Music/SFX

The game will consist of captivating audio throughout. Players are able to enjoy background music in menus and in game accompanied by fun sound effects for characters, enemies and environments. The audio of this game aims to enhance the player experience.

<br>

## User Stories

1. As a player, I want to access the main menu to start or join a multiplayer game, adjust settings, and navigate the game options easily.
2. As a player, I want to host a multiplayer game so that I can invite friends or other players to join and play together.
3. As a player, I want to enter the character select scene after joining or hosting a multiplayer game, where I can choose a unique character to play.
4. As a player, I want to see information about each character, including their abilities, strengths, and weaknesses, to make an informed decision during character selection.
5. As a player, I want to face waves of enemies, challenging my skills and teamwork with other players.
6. As a player, I want to survive against increasingly difficult waves of enemies by utilising my character's skills and discovering powerful skill combinations.
7. As a player, I want the option to restart the game if all players die, allowing us to make a fresh attempt to overcome the challenges.
8. As a player, I want to be able to exit the game midway, ensuring that I can easily return to the main menu or exit the game entirely.

<br>

## Design

### Technologies Used

- Unity: Engine used to develop the game.
- FishNet: Networking solution for Unity that we used to implement multiplayer functionality.
- C#: Main scripting language used for development.
- Aseprite: Used for sprites and animations for Units, Environments and VFX.
- Clip Studio Paint: Used for splash art and concept art for the game.
- GitHub: Version control and collaboration.

### Overall Design

![OverallDesign](images\Overall_Design.png)

### Characters

![CharacterScripts](images\CharacterScriptDiagram.png)

- Unit - contains information (Stats, StatusEffects) about units in the game and implements methods for units to interact via the combat system. (Taking damage, being afflicted by Statuses)
- Player - contains information about the player and a reference to their controlled character.
- Character - contains references to all components, as well as basic information about the character.
- InputCharacter - wrapper class around player inputs calculating and containing information used by other character components.
- MoveCharacter - movement script for character.
- AttackCharacter - character automatically attacks at intervals determined by their AttackSpeed.
- CastCharacter - abstract class outlining universal spellcasting behaviour. (spell cool-downs, spell information etc.)
- CastCharacter - concrete classes containing the specific implementation for each character’s unique skill set.
- AnimatorCharacter - controls what animations are playing for each character.

### Managers/UI

![ManagerUI](images\ManagerScriptDiagram.png)

- GameManager - singleton in charge of scene changes and keeping references to all connected players. Handles the lives total as well.
- UIManager - singleton with functionality to change UI screens that can be used from other classes.
- MainMenu - Main Menu screen with host, join and quit buttons.
- CharacterSelect - Character select lobby.
- GameInfo - UI overlay when in game with information like health and skill cooldowns.
- ReadyScreen - Screen overlay when the current stage is clear and awaiting transition to the next level.
- AwaitingRespawn - Screen overlay when downed and awaiting help from an ally.

### Combat Scripts implementing Combat System above

- SpellData - contains information like spell name, description, cool-down, damage to be used by CastCharacter classes.
- StatusEffectData - contains information about the buffs and de buffs associated with a status effect to be used by Unit classes when being afflicted.
- Damager - inflicts damage on collision with a Character or Enemy.
- Lifetime - server side coroutine to despawn objects after a set time.

### Enemy System

![EnemySystemDiagram](images/EnemyScriptDiagram.png)

- EnemyMovement - A controller to decide which enemy movement algorithms to use
- EnemyAI - Contains information about enemy max health, enemy death event and controls enemy movement and player targetter
- PlayerTargetter - Stores information on the current player targeted for movement and attack.
- Projectiles - Scripts regarding behaviour of projectiles spawned by enemies to be attached to projectile prefabs.

![EnemyWaveDiagram](images/EnemyWaveDiagram.png)

- EnemySpawner - Spawns enemies type according to EnemySpawnerData injected.
- EnemySpawnerData - Contains information such as enemy prefabs, max enemy to spawn and spawn locations
- EnemyManager - Singleton managing and storing references and counts of active enemies currently in the wave
- WaveManager - Singleton managing wave information, enemy spawners and enemy boss information that are injected using WaveData.
- WaveData - Scriptable Object containing information regarding waves such as wave delays, spawners involved.

### Audio System

![AudioScriptDiagram](images/AudioScriptDiagram.png)

- AudioManager - Contains a list of all sound clips and audio sources to control the playing/synchronisation of game sounds.
- AudioMixers - Allows playing of multiple clips simultaneously.

<br>

## Testing

### Unit Testing

Unit testing was conducted with a focus on whether individual scripts and methods within those scripts worked as intended.

### Player-Character related systems

Note: All tests in this section were conducted in multiplayer context. (one game instance in the Unity Editor as the host and another built instance as the client). The tests are  considered successes only if it is performed and satisfactory result is achieved across both client and host in synchronisation. Double checking of synchronised values was done via the Unity Editor’s hierarchy.

<br>

## Build

Link: [Mise-Fighters Download](https://drive.google.com/file/d/1TGjD3pHHSMJFhYWlhF8fZCtCaoKDS3S_/view?usp=drive_linkhttps://drive.google.com/drive/u/0/folders/1Cx69BKabV8r-LCioAJPXIR8c9OckY4Fi)

(Extract the contents before proceeding.)

![BuildExampleImg1](images\BuildExampleImg1.png)

We are using this Network Discovery package for LAN play (as recommended by FishNet official documentation) : Abdelfattah-Radwan/Fish-Networking-Discovery

In order to facilitate testing both over LAN and on a single machine (two instances), we have left both FishNet’s NetworkHUD and the package NetworkDiscovery HUD active.

**LAN Testing instructions:**<br>
You will require two machines connected to the same WiFi network. One will be the host and the other the client.
The host machine runs the application, and selects host. They will be loaded into the lobby. Click Start under the Advertising tab if not automatically advertising.
The client machine runs the application. Click Start under the Searching tab if not automatically searching.
Once the server has been found, the IP will be displayed under the servers tab in the same section.
Click the button with your IP to connect to the same lobby as the host machine.

![HostingExample1](images/HostingExample1.png)

**Single Machine testing instructions:**<br>
Open two instances of the game on one machine. One will be the host and the other the client.
The host instance selects host. They will be loaded into the lobby.
The client instance clicks the “Start Client” button at the top left of the screen to connect to the same lobby as the host machine.

<br>

## Problems Faced

Managing the multiple aspects of the game 
With such a complex design and multiple interconnected parts, there was a constant need for us to update each other on the changes we made and do peer code reviews to understand the design of all the game systems, at least at a basic level.

Us having to affect multiple scripts/systems when making changes also led to the occasional merge conflicts.

Managing the difficulty curve (solved with playtesting)
As developers we are very familiar with the game and its mechanics, so we would naturally underestimate the difficulty of the game. Even with this in mind, we somehow still managed to make the game too difficult on the first playtest. Through player feedback we have brought the difficulty back to an acceptable level, but definitely could be refined more.

### Bugs Squashed:

#### Synchronisation bugs

Faced many challenges involving syncing of values across clients. Especially with so many moving parts, we had to make sure values were synchronised properly.

To fix this bug, we used SyncVar and ServerRPC calls to alter values that are meant to be synced (notably, health) this not only reduced desync but made health bars more responsive and stop jittering, which was a problem before.

#### Client side errors and dealing with RPC calls

Often when implementing features they would work on the host instance/in singleplayer but break in multiplayer on the client instance.

To fix these bugs, we had to correctly implement RPC calls and chain the correct function calls together. For instance, UI displays were bugged and inconsistent for the client instance. Our solution was to chain a TargetRPC function so that only the targeted connection shows the new UI, while other clients remain unaffected (this was useful for Respawn screen and Pause menu screen)

#### Spellcasting related bugs

Spell casting was challenging to implement as animations and damagers had to be synchronised across both clients. We often encountered issues where the client could not cast spells in the desired input direction.

We found out the bug was caused by us calculating/accessing values from our InputCharacter class within RPC functions. To fix it we passed the data as arguments instead.

#### Enemy spawning

Initially, our enemy system uses multiple spawners per wave with set number of enemies per wave and pre-determined enemy variants. The wave system also utilised a buffer mechanism to determine the number of enemy deaths before signalling a wave transition. This system has caused some performance issues and logic error when the wave sized has been increased and boss waves were implemented.

After some brainstorming, we decided we had to overhaul the whole enemy system entirely. The new system would utilise one spawner per wave with set number of enemies, and the enemy variants spawned are determined by set probability. Wave transition conditions were also revised to be more in depth with multiple checks instead of just checking for enemy death counts. These changes have made the wave transition more stable and made enemy waves more diverse and unpredictable, resulting in overall better player experience.
