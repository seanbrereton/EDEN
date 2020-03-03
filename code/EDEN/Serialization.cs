using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace EDEN {
    public static class Serialization {

        public static void WriteBinaryFile(string filePath, SimulationSave toWrite) {
            using (Stream stream = File.Open(filePath, FileMode.Create))
                new BinaryFormatter().Serialize(stream, toWrite);
        }

        public static SimulationSave ReadBinaryFile(string filePath) {
            using (Stream stream = File.Open(filePath, FileMode.Open))
                try {
                    return (SimulationSave)new BinaryFormatter().Deserialize(stream);
                } catch {
                    return null;
                }
        }

        public static void SaveState(Simulation toSave) {
            using (SaveFileDialog dialog = new SaveFileDialog()) {
                string initialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..\\saves");
                dialog.InitialDirectory = Path.GetFullPath(initialDirectory);
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Save Simulation State";
                dialog.ShowDialog();

                if (dialog.FileName != "")
                    WriteBinaryFile(dialog.FileName, new SimulationSave(toSave));
            }
        }

        public static bool LoadState(Application app) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                string initialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..\\saves");
                dialog.InitialDirectory = Path.GetFullPath(initialDirectory);
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Load Simulation State";

                if (dialog.ShowDialog() == DialogResult.OK) {
                    SimulationSave save = ReadBinaryFile(dialog.FileName);
                    if (save != null) {
                        app.SwitchState(ReadBinaryFile(dialog.FileName).ToSimulation(app));
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
