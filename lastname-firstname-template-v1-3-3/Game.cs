// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;
using System.Collections.Generic;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        private WordManager wordManager;
        private VisualEffects visualEffects;
        private int attempts;
        private const int MaxAttempts = 5;

        public Game()
        {
            wordManager = new WordManager();
            visualEffects = new VisualEffects();
            attempts = 0;
        }

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Console.WriteLine("Welcome to Horror Wordle! Guess the 5-letter word.");
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            if (attempts < MaxAttempts)
            {
                Console.Write("Enter your guess: ");
                string? guess = Console.ReadLine()?.ToUpper();
                
                if (!string.IsNullOrEmpty(guess) && wordManager.CheckGuess(guess))
                {
                    visualEffects.DisplayWin();
                    return;
                }
                else
                {
                    visualEffects.DisplayScare();
                    attempts++;
                    Console.WriteLine($"Attempts left: {MaxAttempts - attempts}");
                }
            }
            else
            {
                Console.WriteLine("Game Over! The word was: " + wordManager.GetWord());
            }
        }
    }

    class WordManager
    {
        private List<string> words = new List<string> { "FEAR", "GHOST", "SCARY", "DEATH", "NIGHT" };
        private string chosenWord;
        private static readonly Random random = new Random();

        public WordManager()
        {
            chosenWord = words[random.Next(words.Count)];
        }

        public bool CheckGuess(string guess)
        {
            return guess == chosenWord;
        }

        public string GetWord()
        {
            return chosenWord;
        }
    }

    class VisualEffects
    {
        public void DisplayScare()
        {
            Console.WriteLine("\n!!! JUMPSCARE !!!");
            Console.WriteLine("A terrifying ghost appears!\n");
        }

        public void DisplayWin()
        {
            Console.WriteLine("\nCongratulations! You guessed the word correctly!");
            Console.WriteLine("The atmosphere turns bright and cheerful!\n");
        }
    }
}
