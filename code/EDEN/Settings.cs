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
            envSize = new Point (800, 800);
            maxEnergy = 96;
        }

        public Settings(int _pop, float _foodDensity, int _envSize, int _maxEnergy) {
            //Custom settings constructor
            population = _pop;
            foodDensity = _foodDensity;
            envSize = new Point(_envSize, _envSize);
            maxEnergy = _maxEnergy;
        }
        
    }
}
