# Zombie Echo

Zombie Echo is a 2D top-down zombie survival game that I'm developing in Unity using C#. The goal is to survive as many waves of zombies as possible while collecting weapons, managing ammo, and avoiding getting overwhelmed.

## About the Game

The player starts with a pistol and has to survive waves of zombies that become harder over time. As the waves progress, more zombies spawn and gradually move faster.

One of the main features of the game is the vision and shooting system. The player has two cones: a flashlight cone that shows where they can see and a shooting cone that determines where they can shoot. Zombies outside the flashlight are hidden, making it harder to know exactly where they're coming from.

Shooting is automatic whenever a zombie enters the shooting cone. Moving also makes shooting less accurate, while standing still improves accuracy.

Players can find different weapons and collect health or ammo from defeated zombies to help them survive longer.

## Controls

- **WASD** - Move around
- **Mouse** - Aim
- **R** - Reload
- **Shooting** - Automatic when zombies enter the shooting cone

## Weapons

There are currently three weapons in the game:

- **Pistol:** The starting weapon with unlimited reserve ammo.
- **Assault Rifle:** Shoots faster and has a larger magazine.
- **Shotgun:** Has a shorter range but deals more damage.

The assault rifle and shotgun can be picked up during gameplay. When either weapon runs out of ammo, the player automatically switches back to the pistol.

## Current Features

- Zombies spawn in waves and become harder over time.
- Automatic shooting when zombies enter the aiming cone.
- Flashlight system that limits what the player can see.
- Moving decreases shooting accuracy.
- Three different weapons with their own damage, range, and reload speed.
- Health, ammo, and weapon drops from zombies.
- Pickups disappear after a short amount of time.
- Walls and obstacles that block bullets.
- Different weapon sprites and reload animations.
- Background music and gunshot sound effects.
- Health, ammo, and wave counters.

## Goal

The goal is to survive for as long as possible. As more zombies appear, the player has to pay attention to their surroundings, manage their ammo, and collect useful items to stay alive.

The game ends when the player's health reaches zero.

## Development

I'm currently working on improving the gameplay, balancing the weapons, and adding more features to make the game more enjoyable. Some of the main things I'm focusing on are zombie behavior, sound effects, and making sure the different game mechanics work well together.

**Engine:** Unity

**Language:** C#

**Genre:** 2D Top-Down Zombie Survival
