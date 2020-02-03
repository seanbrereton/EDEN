namespace EDEN {
    class NeuralNet {

        int[] layers;

        public NeuralNet(int[] _layers) {
            layers = _layers;
        }

        float Activate(float value) {
            return value;
        }

        public float[] FeedForward(float[] inputs) {
            return inputs;
        }

    }
}
