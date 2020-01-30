using System;

namespace EDEN {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var app = new Application())
                app.Run();
        }
    }
}
