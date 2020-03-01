using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EDEN {
    class Settings {

        public int population;
        public float foodDensity;
        public Point envSize;
        public int maxEnergy;

        public Settings() {
            //Default settings
            population = 256;
            foodDensity = 0.8f;
            envSize = new Point (1600, 1600);
            maxEnergy = 96;
        }

        public Settings(float _pop, float _foodDensity, float _envSize, float _maxEnergy) {
            //Custom settings constructor
            population = (int)_pop;
            foodDensity = _foodDensity;
            envSize = new Point((int)_envSize, (int)_envSize);
            maxEnergy = (int)_maxEnergy;
        }
    }
}
