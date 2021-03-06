﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        //
        // Variables
        //
       
        // body parts already added to the stickman
        static List<string> parts = new List<string>();
        // guessed letters (wrong and correct) will go in this list
        static List<string> guessedLetters = new List<string>();
        // guessed letters that are correct go in this list
        static List<char> correctLetters = new List<char>(); 
        // wrongly guessed letters will go in this list
        static List<string> guessedWrongLetters = new List<string>(); 
        // current word getting guessed
        static string guessWord;
        // game loop boolean
        static bool gameRunning = true;
        // Initializing hangman
        static HangmanEntity hangman = new HangmanEntity();
        // word but in spaces/with the _ 
        static string wordsp;
        // guessing words list
        static string[] wordlist = new string[10]; 
        static char[] guessChar;
        // hangman's current stickman phase (the phase in which how many body parts are added in text form)
        static string stickmanPhase = "0";

        //
        // Text Stick Man
        //

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            welcome();
            start_game();
            endPrompt();
        }
        //
        // Welcoming Screen
        //
        static void welcome()
        {
            Console.WriteLine("Hello, welcome to Hangman.");
            Console.WriteLine("");
            Console.WriteLine("Press any key to start the game...");
            Console.ReadKey();
        }
        static void addWords()
        {
            wordlist[0] = "banana";
            wordlist[1] = "apple";
            wordlist[2] = "car";
            wordlist[3] = "truck";
            wordlist[4] = "plane";
            wordlist[5] = "tree";
            wordlist[6] = "computer";
            wordlist[7] = "code";
            wordlist[8] = "cpu";
            wordlist[9] = "ram";  
        }
        //
        // Starts the game
        //
        static void start_game()
        { 
            addWords();
            // Randomly choose the word to guess from the words list
            getGuessingWord();
            // getting the spaces for correction
            wordsp = getWordSpaces(guessWord);
             
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Word chosen, begin guessing!");
            Console.WriteLine();
            Console.WriteLine(wordsp);
            Console.WriteLine();
            Console.WriteLine("Guessed Letters: ");
            Console.WriteLine();
            getGuessedLetters();
            Console.WriteLine();
            Console.WriteLine("Correctly Guessed Letters: ");
            getCorrectLetters();
            Console.WriteLine();
            Console.WriteLine("Guess Result: \t\t" + getStickManPhase(stickmanPhase));
            Console.WriteLine();
            // Checks to see if the gameloop is still running - used everytime start_game is called
            gameRunningCheck();
            while (gameRunning)
            {
                // if hangman = true/gameover = true then break out of game loop and end.
                if (hangmanCheck()) {
                    gameOver(); 
                    break;
                }
                // Have the user guess a letter  

                pickLetter();
            }
        }
        //
        // Evaluates the picked letter
        //
        static void pickLetter()
        {
            string userResponse = "";
            char letterguessed;
            Console.WriteLine("Guess a letter.");
            userResponse = Console.ReadLine();
            //
            // Checking the guessed letters list for the user entry
            // If in the list, user guesses again, no appendages added.
            //
            if (guessedLetters.Contains(userResponse))
            {
                Console.WriteLine();
                Console.WriteLine($"You already guessed {userResponse}, guess again.");
            }
            //
            // If the entry is larger than one letter or less than a letter (nothing) then guess again.
            //
            else if (userResponse.Length > 1 || userResponse.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("You can only guess one letter, guess again.");
            }
            //
            // If the guessing word doesn't contain the guess from the user, add an appendage.
            //
            else if (!guessWord.Contains(userResponse))
            {
                if (!parts.Contains("head"))
                {
                    addPart("head");
                    stickmanPhase = "1";
                }
                else if (!parts.Contains("torso"))
                {
                    addPart("torso");
                    stickmanPhase = "2";
                }
                else if (!parts.Contains("leftarm"))
                {
                    addPart("leftarm");
                    stickmanPhase = "3";
                }
                else if (!parts.Contains("rightarm"))
                {
                    addPart("rightarm");
                    stickmanPhase = "4";
                }
                else if (!parts.Contains("leftleg"))
                {
                    addPart("leftleg");
                    stickmanPhase = "5";
                }
                else if (!parts.Contains("rightleg"))
                {
                    addPart("rightleg");
                    stickmanPhase = "6";
                }
                //
                // Add the wrongly guessed letter to the guessedWrongLetters and guessed letters list 
                // Then send the letter to guessedWordSpaces method
                // After, return the correctly guessed letters and guessed letters list
                //
                guessedWrongLetters.Add(userResponse);
                guessedLetters.Add(userResponse);
                if (char.TryParse(userResponse, out letterguessed)) {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Word:");
                    Console.WriteLine();
                    guessedWordSpaces(letterguessed);
                    Console.WriteLine();
                    Console.WriteLine("Guessed Letters: ");
                    Console.WriteLine();
                    getGuessedLetters();
                    Console.WriteLine();
                    Console.WriteLine("Correctly Guessed Letters: ");
                    getCorrectLetters();
                    Console.WriteLine();
                    Console.WriteLine("Guess Result: \t\t" + getStickManPhase(stickmanPhase));
                    Console.WriteLine();
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    Console.WriteLine();
                }
                }
            //
            // if the word-to-guess contains the guessed letter, then run the correct guessed block of code
            // Add the letter guessed to guessedLetters list, correctLetters list, and send it to the guessedWordSpaces method
            // Then return the correct & guessed letters lists
            //
            else if (guessWord.Contains(userResponse))
            {
                guessedLetters.Add(userResponse);
                if (char.TryParse(userResponse, out letterguessed)) {
                    correctLetters.Add(letterguessed);
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Word:");
                    Console.WriteLine();
                    guessedWordSpaces(letterguessed);
                    Console.WriteLine();
                    Console.WriteLine("Guessed Letters: ");
                    Console.WriteLine();
                    getGuessedLetters();
                    Console.WriteLine();
                    Console.WriteLine("Correctly Guessed Letters: ");
                    getCorrectLetters();
                    Console.WriteLine();
                    Console.WriteLine("Guess Result: \t\t" + getStickManPhase(stickmanPhase));
                    Console.WriteLine();
                    Console.WriteLine("Correct Guess!");
                    Console.WriteLine();
                }
                //
                // Checking if the user has won
                //
                if (winCheck()) {
                    winMessage();
                } 
            }

        }
        //
        // checks if the game is running
        //
        static bool gameRunningCheck()
        {
            bool gameover = false;
            if (hangman.bpHead && hangman.bpLeft_Arm && hangman.bpLeft_Leg && hangman.bpRight_Arm && hangman.bpRight_Leg && hangman.bpTorso)
            {
                gameRunning = false;
                gameover = true;
            }
            else
            {
                gameRunning = true;
                gameover = false;
            }
            return gameover;
        }
        //
        // This method will be called if the picked letter isn't right
        // This method will add a part to the hangman 
        // Parts consist of Head, Torso, Left arm, Right Arm, Left Leg, Right Leg
        //
        static string getStickManPhase(string phase)
        { 
            switch (phase)
            {
                case "0":
                    phase = hangman.smPhase0;
                    break;
                case "1":
                    phase = hangman.smPhase1;
                    break;
                case "2":
                    phase = hangman.smPhase2;
                    break;
                case "3":
                    phase = hangman.smPhase3;
                    break;
                case "4":
                    phase = hangman.smPhase4;
                    break; 
                case "5":
                    phase = hangman.smPhase5;
                    break;
                case "6":
                    phase = hangman.smPhase6;
                    break;
                default:
                    break;
            } 
            return phase;
        }
        //
        // Add a body part to the hangman
        //
        static void addPart(string part)
        {
            part = part.ToLower();
            switch (part)
            {
                case "head":
                    hangman.bpHead = true;
                    parts.Add("head");
                   
                    break;
                case "torso":
                    hangman.bpTorso = true;
                    parts.Add("torso"); 
                    break;
                case "leftarm":
                    hangman.bpLeft_Arm = true;
                    parts.Add("leftarm"); 
                    break;
                case "rightarm":
                    hangman.bpRight_Arm = true;
                    parts.Add("rightarm"); 
                    break;
                case "leftleg":
                    hangman.bpLeft_Leg = true;
                    parts.Add("leftleg"); 
                    break;
                case "rightleg":
                    hangman.bpRight_Leg = true;
                    parts.Add("rightleg"); 
                    break;
                default:
                    break;
            }
        }
        //
        // Check hangman state
        //
        static bool hangmanCheck()
        {
            bool hangmanAllParts = false;
            if (hangman.bpHead && hangman.bpLeft_Arm && hangman.bpLeft_Leg && hangman.bpRight_Arm && hangman.bpRight_Leg && hangman.bpTorso)
            {
                hangmanAllParts = true;
            }
            else if (hangman.bpHead == false && hangman.bpLeft_Arm == false && hangman.bpLeft_Leg == false && hangman.bpRight_Arm == false && hangman.bpRight_Leg == false)
            {
                hangmanAllParts = false;
            }

            return hangmanAllParts;
        }
         
        //
        // Pick a random word from the guessing list
        //
        static void getGuessingWord()
        {
            Random rnd = new Random();
            int whichWord = rnd.Next(wordlist.Length); 
            guessWord = wordlist[whichWord];
            guessChar = new char[guessWord.Length];
            for (int p = 0; p < guessWord.Length; p++)
            {
                guessChar[p] = '_';
            }
        }
        //
        // Checking each letter for the guessed letter, if it's the same then add it to the string
        // If it's wrong, then add an underscore to the string
        //
        static string guessedWordSpaces(char letterGuessed)
        { 
            while (true)
            { 
                for (int j = 0; j < guessWord.Length; j++)
                {
                    if (letterGuessed == guessWord[j]) {  
                        guessChar[j] = letterGuessed;
                    }
                }
                 
                break;
            }
            foreach (char item in guessChar)
            {
                string uppercaseItem;
                uppercaseItem = item.ToString().ToUpper();
                Console.Write(" ");
                Console.Write(uppercaseItem);
                Console.Write(" "); 
            }
            Console.WriteLine();
            string returnString = guessChar.ToString();

            return returnString;
        }
        //
        // Getting the underscore/spaces for each letter in the word
        //
        static string getWordSpaces(string stringToSize)
        {
            string spaces = ""; 
            for (int index = 0; index < stringToSize.Length; index++)
            { 
                 spaces = spaces + " _ "; 
            }
            return spaces;
        }
        //
        // Displays the guessed letters to the user
        //
        static void getGuessedLetters()
        { 
            foreach (string guessedLetters in guessedWrongLetters)
            {
                Console.Write(" ");
                Console.Write(guessedLetters.ToUpper());
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        //
        // Displays the correct guessed letters to the user
        //
        static void getCorrectLetters()
        {
            Console.WriteLine();
            foreach (char letter in correctLetters)
            {
                string uppercaseItem;
                uppercaseItem = letter.ToString().ToUpper();
                Console.Write(" ");
                Console.Write(uppercaseItem);
                Console.Write(" "); 
            }
            Console.WriteLine();
        }
        //
        // This method will return the parts of the hangman left (from the parts array)
        //
        static void partsLeft()
        {
                Console.WriteLine($"Limbs check - Head: {hangman.bpHead}, Torso: {hangman.bpTorso}, Left Arm: {hangman.bpLeft_Arm}, Right Arm: {hangman.bpRight_Arm}, Left Leg: {hangman.bpLeft_Leg}, Right Leg: {hangman.bpRight_Leg}");
        }
        //
        // This method will reset the game
        //
        static bool winCheck()
        {
            bool gameWon = false;
            foreach (char letter in guessWord)
            {

                if (correctLetters.Contains(letter))
                {
                    gameWon = true;
                }
                else
                {
                    gameWon = false;
                    break;
                }
            } 
            return gameWon;
        }
        static void winMessage()
        {
            string userResponse;
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Congratulations, you've guessed every letter! The word was: " + guessWord.ToUpper() + "!");
            Console.WriteLine();
            Console.WriteLine("Would you like to play again? Enter <Yes> or <No>");
            while (true)
            {
                userResponse = Console.ReadLine();
                if (userResponse.ToLower() == "yes" || userResponse.ToLower() == "y")
                {
                    resetGame();
                    break;
                }
                else if (userResponse.ToLower() == "no" || userResponse.ToLower() == "n")
                {
                    gameRunning = false;
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter yes or no");
                    Console.WriteLine();
                }
            }


        } 
        static void resetGame()
        {
            //
            //Clear out all of the guessed letters wrong, correct and guessed
            //
            correctLetters.Clear();
            guessedLetters.Clear();
            guessedWrongLetters.Clear(); 
            parts.Clear();
            guessWord = "";
            //
            // Resetting the getters/setters
            //
            hangman.bpHead = false;
            hangman.bpLeft_Arm = false;
            hangman.bpLeft_Leg = false;
            hangman.bpRight_Arm = false;
            hangman.bpRight_Leg = false;
            stickmanPhase = "0"; 
            //
            // Starts the game again
            //
            start_game();
        }
        //
        // Guessed too many times/Game over prompt message
        //
        static void gameOver()
        {
            string userResponse;
            Console.Clear();
            Console.WriteLine("Game over!");
            Console.WriteLine();
            Console.WriteLine("You guessed wrong too many times!");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Would you like to play again? Enter <Yes> or <No>");
            while (true)
            {
                userResponse = Console.ReadLine();
                if (userResponse.ToLower() == "yes" || userResponse.ToLower() == "y")
                {
                    resetGame();
                    break;
                }
                else if (userResponse.ToLower() == "no" || userResponse.ToLower() == "n")
                {
                    gameRunning = false;
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter yes or no");
                    Console.WriteLine();
                }
            }
        }
        //
        // Exiting game prompt/message
        //
        static void endPrompt()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Thank you for playing my app, Hangman.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
