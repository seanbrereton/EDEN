using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    class MainMenu : State {

        public MainMenu(Application _app) : base(_app) {}

        public override void HandleInput() {
            if (Input.Press(Keys.X))
                app.SwitchState(new Simulation(app));
        }

    }
}
