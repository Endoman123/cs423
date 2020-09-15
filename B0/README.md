# B0 - Roll A Ball
This is my version of the _Roll A Ball_ Unity tutorial game.
This version includes multiplayer via two keyboard binds, a simple main menu UI,
a complex map, and falling.

## Controls
**Player 1**
- Roll: WASD
- Jump: Left Shift
**Player 2**
- Roll: Arrow Keys
- Jump: Right Control

## Extra Credit Attempt
Attempting networked multiplayer is too complicated given the time span, even for something as small as this,
as you would need to implement serverside movement verification and movement interpolation, otherwise
the experience would be slow, laggy, and very susceptible to cheating. Even then, Unity has deprecated
their old API, UNet, for networking. This also means that implementing P2P gameplay for a game like this
is now much harder since it would require looking for a good API to implement it.

