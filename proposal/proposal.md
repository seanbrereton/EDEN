# School of Computing
# Ca326 Year 3 Project Proposal Form

**Project Title:** EDEN

**Students:**

| Name          | ID Number |
| ------------- | --------- |
| Seán Brereton | 17412844  |
| Seán Dyer     | 17482112  |

**Staff Member Consulted:** Alistair Sutherland


## Project Description

#### Description
The main aim of our project is to show the evolution of artificial beings in a simulated environment. Artificial beings will be able to move around the environment, and perform actions such as eating and reproducing.

Initially, a number of creatures will be generated completely randomly. Eventually, they will begin to sexually reproduce. This will start the process of evolution, as the creatures that are better at surviving are more likely to reproduce and spread their successful genes.

Food will spawn at random in different areas of the environment. The environment will have adjustable settings, such as temperature, which would affect how much energy is lost by the creatures, and the spawn rate of food.

The beings will each have a neural network that allows them to make decisions based on their statistics, and their environment. Their inputs will include things such as the distance and angle to the nearest visible food, and the nearest visible creature. It will also include attributes such as their energy level. We are considering adding a pheromone system, which would hopefully lead to them developing some level of communication.

From these inputs, they will decide their movement speed and direction, their turn angle, and possibly some information about their pheromone output. They will also decide whether or not perform certain actions, such as trying to eat, attack, or reproduce.

They will also have physical attributes. Maximum health is how much damage they can withstand. Maximum speed is the fastest they can move. Strength is how much damage they do when attacking. Age will keep track of how long they have lived.

The energy level is an important attribute of the creature. They can increase it by eating, but it is decreased by every other action. Different actions will cost different amounts of energy. The energy cost of actions will be affected by their physical attributes, so that a creature with better attributes will need to eat more food to make up for it. If a creature drops below the required minimum, the creature will die, and their body will become food for the others.


#### Division of Work
- We plan on doing pair programming either in person or with Visual Studio Code Live Share for the main aspects of the project such as the development of the neural network.
- Seán Dyer will focus on the graphics of the program, using Monogame to display the creatures and their environment, as well as the physics and movement of the creatures.
- Seán Brereton will manage the UI of the program, including menus and graphs to show various statistics, as well as the abilities of the creatures, such as eating, fighting, and reproducing.


#### Programming Language(s)
- C#


#### Programming Tool(s)
- Monogame
- C# compiler


#### Learning Challenges
- Both of us have very little experience with C#.
- We will need to figure out how to display the graphics.
- Some research into neural networks is required.


#### Hardware / Software Platform
- Windows


#### Special Hardware / Software Requirements
- None


