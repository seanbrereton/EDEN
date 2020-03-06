using System;

namespace EDEN {
    public static class Program {
        [STAThread]

        // Launches app
        static void Main() {
            using (var app = new Application())
                app.Run();
        }
    }
}
