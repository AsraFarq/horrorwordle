using System;
using System.Collections.Generic;
using Raylib_cs;

namespace MohawkGame2D

// Updated namespace
{
    public class Game
    {
        // Core game logic
        private WordManager _wordManager;
        private int _currentRow;
        private int _currentCol;
        private string _chosenWord;
        private char[,] _guessedLetters = new char[MaxAttempts, WordLength];
        private Color[,] _boxColors = new Color[MaxAttempts, WordLength];

        // Game settings
        private const int WordLength = 5;
        private const int MaxAttempts = 6;
        // Adjust grid placement based on screen size
        private const int ScreenWidth = 800;
        private const int ScreenHeight = 600;
        private const int BoxSize = 80;  
        private const int BoxSpacing = 10; 

// Grid dimensions
        private const int GridWidth = (WordLength * (BoxSize + BoxSpacing)) - BoxSpacing;
        private const int GridHeight = (MaxAttempts * (BoxSize + BoxSpacing)) - BoxSpacing;

// Centering calculations
        private const int StartX = (ScreenWidth - GridWidth) / 2;
        private const int StartY = (ScreenHeight - GridHeight) / 2;


        public Game()
        {
            _wordManager = new WordManager();
            _chosenWord = _wordManager.GetWord();
        }

        public void Setup()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Horror Wordle");
            Raylib.SetTargetFPS(60);

            // Initialize the grid
            for (int row = 0; row < MaxAttempts; row++)
            {
                for (int col = 0; col < WordLength; col++)
                {
                    _boxColors[row, col] = Raylib_cs.Color.White;
                }
            }
        }

        public void Update()
        {
            while (!Raylib.WindowShouldClose())
            {
                HandleInput();
                DrawGame();
            }
        }

        private void HandleInput()
        {
            int key = Raylib.GetCharPressed();

            if (key >= 65 && key <= 90 && _currentCol < WordLength)
            {
                _guessedLetters[_currentRow, _currentCol] = (char)key;
                _currentCol++;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && _currentCol > 0)
            {
                _currentCol--;
                _guessedLetters[_currentRow, _currentCol] = '\0';
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Enter) && _currentCol == WordLength)
            {
                ProcessGuess();
            }
        }

        private void ProcessGuess()
        {
            string guess = "";
            for (int i = 0; i < WordLength; i++)
            {
                guess += _guessedLetters[_currentRow, i];
            }

            if (guess == _chosenWord)
            {
                for (int i = 0; i < WordLength; i++)
                    _boxColors[_currentRow, i] = Color.Green;

                return;
            }

            for (int i = 0; i < WordLength; i++)
            {
                if (guess[i] == _chosenWord[i])
                {
                    _boxColors[_currentRow, i] = Color.Green;
                }
                else if (_chosenWord.Contains(guess[i]))
                {
                    _boxColors[_currentRow, i] = Color.Yellow;
                }
                else
                {
                    _boxColors[_currentRow, i] = Color.DarkGray;
                }
            }

            _currentRow++;
            _currentCol = 0;

            if (_currentRow >= MaxAttempts)
            {
                Console.WriteLine($"Game Over! The word was: {_chosenWord}");
                Raylib.DrawText("Game Over!", 300, 500, 30, Color.Red);
            }
        }

        private void DrawGame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int row = 0; row < MaxAttempts; row++)
            {
                for (int col = 0; col < WordLength; col++)
                {
                    int x = StartX + col * (BoxSize + BoxSpacing);
                    int y = StartY + row * (BoxSize + BoxSpacing);

                    Raylib.DrawRectangle(x, y, BoxSize, BoxSize, _boxColors[row, col]);

                    if (_guessedLetters[row, col] != '\0')
                    {
                        Raylib.DrawText(_guessedLetters[row, col].ToString(), x + 25, y + 20, 40, Color.White);
                    }

                    Raylib.DrawRectangleLines(x, y, BoxSize, BoxSize, Color.White);
                }
            }

            Raylib.EndDrawing();
        }
    }

    class WordManager
    {
        private List<string> _words = ["FEAR", "GHOST", "SCARY", "DEATH", "NIGHT"];
        private string _chosenWord;
        private static readonly System.Random _random = new System.Random();

        public WordManager()
        {
            _chosenWord = _words[_random.Next(_words.Count)];
        }

        public string GetWord()
        {
            return _chosenWord;
        }
    }
}
