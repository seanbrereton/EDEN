﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EDEN {
    class SimMenu : State {

        public SimMenu(Application _app) : base(_app) { }

        int buttonHeight = 30;
        int buttonWidth = 200;

        //Make button
        //Pass in position, size, text

        NumInput popNum;
        NumInput foodSpawn;
        NumInput envSize;
        NumInput waterLevel;
        NumInput maxEnergy;
        NumInput hiddenLayerCount;
        NumInput hiddenLayerSize;



        public override void Start() {
            //set background
            bgColor = Color.DarkOliveGreen;

            Vector2 position = new Vector2(app.screenSize.X / 2, (app.screenSize.Y / 2) - buttonHeight * 2.4f);

            //Default simulation
            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Default Simulation", () => {
                app.SwitchState(new Simulation(app, new Settings()));
            }));

            position.Y += 50;

            //Population
            popNum = new NumInput("Population", 256, 128, 1024, position, 32);
            AddComponent(popNum);

            position.Y += 50;

            //Food ratio
            foodSpawn = new NumInput("Food spawn", 1, 0, 2, position, 0.1f);
            AddComponent(foodSpawn);

            position.Y += 50;

            //Env size
            envSize = new NumInput("Environment size", 1600, 320, 3200, position, 64);
            AddComponent(envSize);

            position.Y += 50;

            //Water
            waterLevel = new NumInput("Water Level", 0.6f, 0.1f, 0.9f, position, 0.05f);
            AddComponent(waterLevel);

            position.Y += 50;

            //Max energy
            maxEnergy = new NumInput("Max Energy", 96, 48, 192, position, 6);
            AddComponent(maxEnergy);

            position.Y += 50;

            //Layer size

            hiddenLayerCount = new NumInput("Hidden Layer Count", 2, 0, 8, position, 1);
            AddComponent(hiddenLayerCount);

            position.Y += 50;

            hiddenLayerSize = new NumInput("Hidden Layer Size", 13, 1, 26, position, 1);
            AddComponent(hiddenLayerSize);

            position.Y += 50;

            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Start Custom Sim", () => {
                Settings customSettings = new Settings(
                    popNum.value,
                    foodSpawn.value,
                    envSize.value,
                    waterLevel.value,
                    maxEnergy.value,
                    hiddenLayerCount.value,
                    hiddenLayerSize.value
                );

                app.SwitchState(new Simulation(app, customSettings));
            }));
        }
    }
}
