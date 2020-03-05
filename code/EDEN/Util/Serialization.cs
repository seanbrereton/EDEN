using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace EDEN {
    public static class Serialization {

        public static void WriteBinaryFile(string filePath, SimulationSave toWrite) {
            // Creates a new file, with a serialized simulation save
            using (Stream stream = File.Open(filePath, FileMode.Create))
                new BinaryFormatter().Serialize(stream, toWrite);
        }

        public static SimulationSave ReadBinaryFile(string filePath) {
            // Tries to load a simulation save from a binary file, returns null if an error is encountered
            using (Stream stream = File.Open(filePath, FileMode.Open))
                try {
                    return (SimulationSave)new BinaryFormatter().Deserialize(stream);
                } catch {
                    return null;
                }
        }

        public static void SaveState(Simulation toSave) {
            // Opens up a file browser window, to create a simulation save file

            using (SaveFileDialog dialog = new SaveFileDialog()) {
                // Filters what files are shown, to only show *.bin files
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Save Simulation State";
                dialog.ShowDialog();

                // If a file name is entered and saved, create a simulation save object,
                // and write it as a binary file
                if (dialog.FileName != "")
                    WriteBinaryFile(dialog.FileName, new SimulationSave(toSave));
            }
        }

        public static bool LoadState(Application app) {
            // Opens up a file browser window, to choose a simulation save file

            using (OpenFileDialog dialog = new OpenFileDialog()) {
                // Filters what files are shown, to only show *.bin files
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Load Simulation State";

                if (dialog.ShowDialog() == DialogResult.OK) {
                    SimulationSave save = ReadBinaryFile(dialog.FileName);
                    if (save != null) {
                        // If the file is read correctly, switch the app state to the loaded simulation
                        app.SwitchState(ReadBinaryFile(dialog.FileName).ToSimulation(app));
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
