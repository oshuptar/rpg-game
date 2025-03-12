# Stage 2: Creating the dungeons

## Objective of the task.
Create following systems:
- creation of the dungeon,
- presentation of the game state.
  
## Requirements

### Creation of the dungeon
As part of the task, you need to create a dungeon building system that will allow you to create a dungeon according to the given strategies.

List of available building procedures:
 - empty dungeon - each element is an empty element,
 - filled dungeon - each element is a wall,
 - adding paths - adds random paths through the dungeon,
 - adding chambers - adds random empty fields in the dungeon,
 - adding a central room - adds a large central room to the dungeon,
 - adding items - distributes random items on non-wall fields,
 - adding weapons - distributes random weapons on non-wall fields,
 - adding modified weapons - distributes random weapons with modifiers on non-wall fields,
 - adding potions - distributes random potions on non-wall fields,
 - adding enemies - distributes random enemies on non-wall fields.

Procedures:
 - empty dungeon,
 - filled dungeon,

are starter strategies, this means that they must be called before all other strategies.
The other strategies can be combined with each other in any order, and the result of their application should always be correct.

Adding more strategies consisting of a sequence of procedures and new procedures should be easy.

This step should be implemented using the Builder pattern.

### Presentation of the game state
The task should create a centralized system to present the state of the game.
The game system should present:
 - dungeon,
 - information about objects on the player's field,
 - information about the surrounding opponents,
 - the status of the player - his equipped items, statistics and equipment,
 - information about the actions taken by the player.
   
The system is the only object in the program that can write to the console.

The system should be easily expandable to write out other objects, or to present data in another form.

This step should be implemented using the Singleton pattern.

### Displaying game instructions
A system that explains how to play should display instructions based on the elements present in the dungeon. This means that, for example, if there are no items in the maze, there should be no information about picking them up.

This step should be done using the Builder pattern and the system implemented in the previous section with the appropriate building procedures.
