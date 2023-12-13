# Mise Fighters Game Development Document

## Problem Motivation

Drawing inspiration from beloved hack and slash titles like Hades and Hyper Light Drifter, we set out to create a whimsical online multiplayer cooperative hack and slash experience. Our game introduces a team of a chef, a butcher, a bartender and a pâtissier on a mission to withstand the fury of vengeful food ingredients that have mysteriously come to life. While hack and slash games often exhibit a gory and intense atmosphere, we wanted to infuse our game with humour and light-heartedness while maintaining a thrilling action-packed gameplay.

In our unique culinary adventure, players are not only encouraged but also rewarded for synergizing their skills to unleash mesmerizing combos and amusing effects against the culinary adversaries. By seamlessly combining their culinary prowess, the 4 friends can overcome the challenges and obstacles thrown their way.

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

