using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    [Serializable]
    public class SimulationSave {

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
            Simulation simulation = new Simulation(app, settings);

            foreach (CreatureSave creatureSave in creatureSaves) {
                simulation.creatures.Add(creatureSave.ToCreature());
            }

            simulation.environment.tiles = environmentTiles;
            simulation.environment.GenerateTexture();

            return simulation;
        }

    }
}
