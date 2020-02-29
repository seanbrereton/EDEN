using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    class MainMenu : State {

        public MainMenu(Application _app) : base(_app) {}

        //Make button
        //Pass in position, size, text

        public List<Component> buttons = new List<Component>();
        public Button startButton;
        public Button quitButton;

        public override void Start() {
            //set background
            bgColor = Color.DarkOliveGreen;
            //make start button
            startButton = new Button(200, 30, Color.White, new Vector2(400, 400), "New Simulation", () => {
                app.SwitchState(new Simulation(app)); 
            });
            buttons.Add(startButton);
            components.Add(startButton);


            //quit button
            quitButton = new Button(200,30, Color.White, new Vector2(400, 500), "Quit", () => { 
                System.Environment.Exit(1); 
            });
            buttons.Add(quitButton);
            components.Add(quitButton);
        }
        
            
    }
}
