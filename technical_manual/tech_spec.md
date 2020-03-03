# 0. Table of Contents

{:toc}

## 1. Introduction

### 1.1 Overview

The system that we developed is an application that allows users to run simulation to show the evolution of artificial creatures that make decisions through a neural network. It ......

### 1.2 Glossary

#### *Artificial neural Network*

- Artificial neural networks are computing systems that are inspired by the networks of neurons that make up biological brains. They have inputs that go through some amount of hidden layers, the weights and biases of which determine the outputs.

#### *Feed Forawrd Neural Network*

- A feedforward neural network is an artificial neural network wherein connections between the nodes do not form a cycle. As such, it is different from recurrent neural networks

#### *C#*

- C# is a general-purpose, multi-paradigm programming language encompassing strong typing, lexically scoped, imperative, declarative, functional, generic, object-oriented, and component-oriented programming disciplines.

#### *Monogame*

- Monogame is a C# framework based commonly used by developers for its graphical capabilities.

#### *QuadTree*

- A quadtree is a tree data structure in which each internal node has exactly four children. Quadtrees are the two-dimensional analog of octrees and are most often used to partition a two-dimensional space by recursively subdividing it into four quadrants or regions.

## 2. System Architecture

- system architecture diagram?

## 3. High-Level Design

- DFD
- Object model

## 4. Problems and Resolution

- **Problem:** The biggest problem we encountered was the efficiency of the program. **Resolution** We found that our intitial method of collision detection used a lot of computation power when there were a lot of entities in the simulation. Every entity was checked against each other each frame which got very slow with large number of entities. . Quad tree
- lived too long going in circle. Speed takes more energy
- not reproducing. made easier to reproduce
- no challenge. Water
- very similar results. Custom sims
- **Problem:** Saying goodbye to fallen nermals. **Resolution:** There is no resolution. It gets harder everyday.

## 5. Installation Guide

- Download zip and open .exe

OR

- need c# (c# download steps)
- windows or linux
- download program from repo (download link)
- run in visual studio, monogame or as a standalone program (newest version link)
- need monogame (mongame download steps)
