# B0 - Roll A Ball
This is my version of the _Roll A Ball_ Unity tutorial game.
This version includes multiplayer via two keyboard binds, a simple main menu UI,
a complex map, and falling.

You roll a ball around a map, jumping up to get to hard-to-reach places. 
The objective of the game is to score more points than your opponent
in two minutes.

## Controls
**Player 1**
- Roll: WASD
- Jump: Left Shift
**Player 2**
- Roll: Arrow Keys
- Jump: Right Control
**Other**
- Pause: Escape

## Features
- Scoring: Scoring is done by picking up yellow boxes around the map.
  Players can also be penalized for jumping on top of players.
  Scores cannot drop below 0.
- Falling: The map has no physical boundaries, and you can easily fall into gaps in the map.
  Falling forces you to respawn back at the starting point.
- Multiplayer: Play against your friend, your S/O, or whoever else on the same keyboard.

## Extra Credit Attempt
Attempting networked multiplayer is too complicated given the time span, even for something as small as this,
as you would need to implement serverside movement verification and movement interpolation, otherwise
the experience would be slow, laggy, and very susceptible to cheating. Even then, Unity has deprecated
their old API, UNet, for networking. This also means that implementing P2P gameplay for a game like this
is now much harder since it would require looking for a good API to implement it.