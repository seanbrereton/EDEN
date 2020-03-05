using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    [Serializable]
    public class CreatureSave {

        string name;
        NeuralNet network;
        float x;
        float y;
        float rotation;
        int childrenCount;
        int generation;
        float age;
        float reproductionTimer;
        float energy;
        uint colorValue;
        float scale;

        public CreatureSave(Creature creature) {
            name = creature.name;
            network = creature.network;
            x = creature.position.X;
            y = creature.position.Y;
            childrenCount = creature.childrenCount;
            generation = creature.generation;
            age = creature.age;
            reproductionTimer = creature.reproductionTimer;
            energy = creature.energy;
            colorValue = creature.color.PackedValue;
            rotation = creature.rotation;
            scale = creature.scale;
        }

        public Creature ToCreature(Simulation simulation) {
            Creature creature = new Creature() {
                name = name,
                network = network,
                position = new Vector2(x, y),
                childrenCount = childrenCount,
                generation = generation,
                age = age,
                reproductionTimer = reproductionTimer,
                energy = energy,
                color = new Color(colorValue),
                rotation = rotation,
                scale = scale,
                sim = simulation
            };

            return creature;
        }

    }
}
