using System;
using System.Collections.Generic;

namespace EDEN {
    [Serializable]
    public class SimulationSave {

        // A class of serializable data representing a simulation, that can be saved to a file

        public List<CreatureSave> creatureSaves = new List<CreatureSave>();
        bool[,] environmentTiles;
        Settings settings;

        public SimulationSave(Simulation simulation) {
            foreach (Creature creature in simulation.creatures)
                creatureSaves.Add(new CreatureSave(creature));

            environmentTiles = simulation.environment.tiles;
            settings = simulation.settings;
        }

        public Simulation ToSimulation(Application app) {
            // Constructs new simulation using saved settings
            Simulation simulation = new Simulation(app, settings);

            // Adds saved creatures to simulation's creature list
            // Does not add to simulation's component list, as they are not ready to be Started
            foreach (CreatureSave creatureSave in creatureSaves)
                simulation.creatures.Add(creatureSave.ToCreature(simulation));

            // Generate simulation's environment texture using saved tiles
            simulation.environment.tiles = environmentTiles;
            simulation.environment.GenerateTexture();

            return simulation;
        }

    }
}
