using System;

namespace EDEN {
    [Serializable]
    public class Settings {

        // Holds the settings used by simulations and creatures

        public int population;
        public float foodDensity;
        public int envSize;
        public float waterLevel;
        public int maxEnergy;
        public int hiddenLayerCount;
        public int hiddenLayerSize;

        public Settings() {
            //Default settings
            population = 256;
            foodDensity = 0.8f;
            envSize = 1600;
            waterLevel = 0.12f;
            maxEnergy = 96;
            hiddenLayerCount = 2;
            hiddenLayerSize = 13;
        }

        public Settings(float _pop, float _foodDensity, float _envSize, float _waterLevel, float _maxEnergy, float _hiddenLayerCount, float _hiddenLayerSize) {
            //Custom settings constructor
            population = (int)_pop;
            foodDensity = _foodDensity;
            envSize = (int)_envSize;
            waterLevel = _waterLevel / 5f;
            maxEnergy = (int)_maxEnergy;
            hiddenLayerCount = (int)_hiddenLayerCount;
            hiddenLayerSize = (int)_hiddenLayerSize;
        }
    }
}
