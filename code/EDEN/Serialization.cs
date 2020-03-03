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
                return (SimulationSave)new BinaryFormatter().Deserialize(stream);
        }

        public static void SaveState(Simulation toSave) {
            using (SaveFileDialog dialog = new SaveFileDialog()) {
                dialog.InitialDirectory = "C:\\";
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Save Simulation State";
                dialog.ShowDialog();

                if (dialog.FileName != "")
                    WriteBinaryFile(dialog.FileName, new SimulationSave(toSave));
            }
        }

        public static void LoadState(Application app) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.InitialDirectory = "C:\\";
                dialog.Filter = "Save State|*.bin";
                dialog.Title = "Load Simulation State";
                // ? dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK) {
                    app.SwitchState(ReadBinaryFile(dialog.FileName).ToSimulation(app));
                }
            }
        }

    }
}
