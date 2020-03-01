using System;
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
        NumInput maxEnergy;


        public override void Start() {
            //set background
            bgColor = Color.DarkOliveGreen;

            Vector2 position = new Vector2(app.screenSize.X / 2, (app.screenSize.Y / 2) - buttonHeight * 2.4f);

            //Random simulation
            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Random Simulation", () => {
                app.SwitchState(new Simulation(app));
            }));

            position.Y += 50;

            //Population
            popNum = new NumInput("Population", 512, 128, 1024, position, 32);
            AddComponent(popNum);

            position.Y += 50;
            
            //Food ratio
            foodSpawn = new NumInput("Food spawn", 1024, 64, 2048, position, 64);
            AddComponent(foodSpawn);
            
            position.Y += 50;

            //Env size
            envSize = new NumInput("Environment size",);

            //H20 ratio

            //Max energy
            maxEnergy = new NumInput("Max Energy", 96, 48, 192, position, 6);
            AddComponent(maxEnergy);
            
            position.Y += 50;

            //Layer size??
            position.Y += 50;

            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Start Custom Sim", () => {
                StartSimulation();
            }));
        }

        void StartSimulation() {
            app.SwitchState(new Simulation(app));
        }
    }
}
