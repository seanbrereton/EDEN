﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    class MainMenu : State {

        public MainMenu(Application _app) : base(_app) {
        }

        int buttonHeight = 30;
        int buttonWidth = 200;

        //Make button
        //Pass in position, size, text

        public override void Start() {
            //set background
            bgColor = Color.DarkOliveGreen;

            //initial position for button
            Vector2 position = new Vector2(app.screenSize.X / 2, (app.screenSize.Y / 2) - buttonHeight * 2.4f);

            //Start new simulation button
            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "New Simulation", () => {
                app.SwitchState(new SimMenu(app)); 
            }));

            //changing position for next button
            position.Y += buttonHeight * 1.4f;
            Vector2 popupPosition = new Vector2(position.X + buttonWidth, position.Y);

            //Load button
            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Load Simulation", () => {
                if (!Serialization.LoadState(app))
                    AddComponent(new PopUp(popupPosition, Color.DarkRed, (int)(buttonWidth * 0.8f), (int)(buttonHeight * 1.6f), "Invalid Save File"));
            }));

            position.Y += buttonHeight * 1.4f;

            //Quit button
            AddComponent(new Button(buttonWidth, buttonHeight, Color.White, position, "Quit", () => { 
                System.Environment.Exit(1); 
            }));
        }
        
            
    }
}
