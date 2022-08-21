# Cryptopals

This repository contains C# implementations of the [Cryptopals Challengs](https://cryptopals.com). This repository will be continually updated as I complete more of the Challenges.

## Current Status

Current status as of 08/21/2022: 9/66 Challenges Completed.

## Building and Executing

To building the solution, simply clone the repository and open the solution in Visual Studio. There are no external requirements, just build the solution.

To execute the challenges, you can use the `ChallengeRunner()` class. This class takes an optional integer parameter upon instantiation that will specify the "set" of challenges to load. Not specifying a set number will load every set available; all sets are loaded based on Namespace (`Cryptopals.Challenges.SetX`).

The challenges will execute and print their results to the console when you call `ChallengeRunner.Execute()`.

## Answers

If you'd like to view the answers to each challenge before doing them yourself, you can find them all loaded in a static class found in `Challenges/Answers.cs`.

# Disclaimer

THIS CODE IS AN EXERCISE IN CRYPTOGRAPHY AND DOES NOT INDICATE CODE THAT IS PRODUCTION READY OR PRODUCTION SAFE. ANY USE OF THIS CODE IN A PRODUCTION PROJECT/SOLUTION IS DONE AT YOUR OWN RISK.
