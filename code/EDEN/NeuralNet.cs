using System;
using System.Collections.Generic;

namespace EDEN {
    public class NeuralNet {

        int[] layers;
        float[][] neurons;
        float[][] biases;
        float[][][] weights;

        public NeuralNet(int[] _layers) {
            // Initializes the neurons, biases, and weights to random values
            layers = new int[_layers.Length];
            List<float[]> neuronList = new List<float[]>();
            List<float[]> biasList = new List<float[]>();
            List<float[][]> weightList = new List<float[][]>();

            for (int i = 0; i < layers.Length; i++) {
                layers[i] = _layers[i];
                neuronList.Add(new float[layers[i]]);
                float[] bias = new float[layers[i]];
                List<float[]> layerWeightList = new List<float[]>();

                for (int j = 0; j < layers[i]; j++) {
                    bias[j] = Rand.Range(-0.5f, 0.5f);

                    if (i > 0) {
                        float[] neuronWeights = new float[layers[i - 1]];
                        for (int k = 0; k < layers[i - 1]; k++)
                            neuronWeights[k] = Rand.Range(-0.5f, 0.5f);
                        layerWeightList.Add(neuronWeights);
                    }
                }

                biasList.Add(bias);
                if (i > 0)
                    weightList.Add(layerWeightList.ToArray());

            }

            neurons = neuronList.ToArray();
            biases = biasList.ToArray();
            weights = weightList.ToArray();
        }

        public NeuralNet (NeuralNet net1, NeuralNet net2) {
            NeuralNet[] parents = { net1, net2 };
            layers = new int[net1.layers.Length];
            List<float[]> neuronList = new List<float[]>();
            List<float[]> biasList = new List<float[]>();
            List<float[][]> weightList = new List<float[][]>();

            for (int i = 0; i < layers.Length; i++) {
                layers[i] = net1.layers[i];
                neuronList.Add(new float[layers[i]]);
                float[] bias = new float[layers[i]];
                List<float[]> layerWeightList = new List<float[]>();

                for (int j = 0; j < layers[i]; j++) {
                    bias[j] = Rand.Choice(parents).biases[i][j];

                    if (i > 0) {
                        float[] neuronWeights = new float[layers[i - 1]];
                        for (int k = 0; k < layers[i - 1]; k++)
                            neuronWeights[k] = Rand.Choice(parents).weights[i-1][j][k];
                        layerWeightList.Add(neuronWeights);
                    }
                }

                biasList.Add(bias);
                if (i > 0)
                    weightList.Add(layerWeightList.ToArray());
            }

            neurons = neuronList.ToArray();
            biases = biasList.ToArray();
            weights = weightList.ToArray();
        }

        float Activate(float value) {
            //return (float)Math.Tanh(value);
            float k = (float)Math.Exp(value);
            return -1 + (2 * k) / (1f + k);
        }

        public float[] FeedForward(float[] inputs) {
            for (int i = 0; i < inputs.Length; i++)
                neurons[0][i] = inputs[i];

            for (int i = 1; i < layers.Length; i++) {
                for (int j = 0; j < neurons[i].Length; j++) {
                    float value = 0f;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    neurons[i][j] = Activate(value + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1];
        }

    }
}
