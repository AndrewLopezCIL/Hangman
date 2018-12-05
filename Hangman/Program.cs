using System;
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
        // parts
        static List<string> parts = new List<string>();
        // guessed letters (wrong and correct) will go in this list
        static List<string> guessedLetters = new List<string>();
        // guessed letters that are correct go in this list
        static List<char> correctLetters = new List<char>();
        // wrongly guessed letters will go in this list
        static List<string> guessedWrongLetters = new List<string>();
        // guessable words go in this list
        static List<string> words = new List<string>();
        // current word getting guessed
        static string guessWord;
        // game loop
        static bool gameRunning = true;
        // Initializing hangman
        static HangmanEntity hangman = new HangmanEntity();
        static string wordsp;

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
            words.Add("banana");
            words.Add("apple");
            words.Add("car");
            words.Add("truck");
            words.Add("plane");
            words.Add("tree");
            words.Add("computer");
            words.Add("code");
            words.Add("cpu");
            words.Add("ram");
            words.Add("keyboard");
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
            Console.WriteLine("Word chosen!");
            Console.WriteLine();
            Console.WriteLine("Begin guessing.");
            Console.WriteLine("");
            Console.WriteLine(wordsp);

            Console.WriteLine("");
            // Checks to see if the gameloop is still running - used everytime start_game is called
            gameRunningCheck();
            while (gameRunning)
            {
                // if hangman = true/gameover = true then break out of game loop and end.
                if (hangmanCheck()) {
                    gameOver();
                    playAgainPrompt();
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
            // Checking the guessed letters list for the user entry
            // If in the list, user guesses again, no appendages added.
            //
            if (guessedLetters.Contains(userResponse))
            {
                Console.WriteLine($"You already guessed {userResponse}, guess again.");
            }
            //
            // If the entry is larger than one letter or less than a letter (nothing) then guess again.
            //
            else if (userResponse.Length > 1 || userResponse.Length < 1)
            {
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
                }
                else if (!parts.Contains("torso"))
                {
                    addPart("torso");
                }
                else if (!parts.Contains("leftarm"))
                {
                    addPart("leftarm");
                }
                else if (!parts.Contains("rightarm"))
                {
                    addPart("rightarm");
                }
                else if (!parts.Contains("leftleg"))
                {
                    addPart("leftleg");
                }
                else if (!parts.Contains("rightleg"))
                {
                    addPart("rightleg");
                }
                guessedWrongLetters.Add(userResponse);
            }
            else if (guessWord.Contains(userResponse))
            {
                guessedLetters.Add(userResponse);
                if (char.TryParse(userResponse, out letterguessed)) {
                    correctLetters.Add(letterguessed);
                    Console.WriteLine(guessedWordSpaces(letterguessed)); 
                }
                    // Get the positions where the userResponse is at in the guessing word
                // Add it to the underscore position in console 
                Console.WriteLine("Correct Guess! Guess another one!");
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
        static void addPart(string part)
        {
            part = part.ToLower();
            switch (part)
            {
                case "head":
                    hangman.bpHead = true;
                    parts.Add("head");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    break;
                case "torso":
                    hangman.bpTorso = true;
                    parts.Add("torso");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    break;
                case "leftarm":
                    hangman.bpLeft_Arm = true;
                    parts.Add("leftarm");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    break;
                case "rightarm":
                    hangman.bpRight_Arm = true;
                    parts.Add("rightarm");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    break;
                case "leftleg":
                    hangman.bpLeft_Leg = true;
                    parts.Add("leftleg");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
                    break;
                case "rightleg":
                    hangman.bpRight_Leg = true;
                    parts.Add("rightleg");
                    Console.WriteLine("Wrong guess, appendage added to stickman.");
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
        static void playAgainPrompt()
        {
            string userResponse;
            Console.Clear();
            Console.WriteLine("Would you like to play again?");
            userResponse = Console.ReadLine();
            bool checking = true;
            while (checking)
            {
                switch (userResponse.ToLower())
                {
                    case "yes":
                    case "y":
                        resetGame();
                        checking = false;
                        break;
                    case "no":
                    case "n":
                        checking = false;
                        break;
                    default:
                        break;
                }
            }
        }
        //
        // Pick a random word from the guessing list
        //
        static void getGuessingWord()
        {
            Random rnd = new Random();
            int whichWord = rnd.Next(words.Count);
            guessWord = words[whichWord];
        }
        //
        // Checking each letter for the guessed letter, if it's the same then add it to the string
        // If it's wrong, then add an underscore to the string
        //
        static string guessedWordSpaces(char letterGuessed)
        {
            string guessed = "";
            // Going through the word getting the index of each letter
            for (int letter = 0; letter < guessWord.Length; letter++)
            {
                
                if (guessWord[letter] != letterGuessed && !correctLetters.Contains(guessWord[letter]))
                { 
                    guessed = guessed + " _ ";
                }
                if (correctLetters.Contains(letterGuessed) && guessWord[letter] == letterGuessed)
                    {
                        guessed = guessed + " " + letterGuessed + " ";
                    wordsp = guessed;
                    }
                 
            } 
            return wordsp;
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

        }
        //
        // Displays the correct guessed letters to the user
        //
        static void getCorrectLetters()
        {

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
        static void resetGame()
        {
            //Clear out all of the guessed letters wrong, correct and guessed
            correctLetters.Clear();
            guessedLetters.Clear();
            guessedWrongLetters.Clear();
            parts.Clear();
            guessWord = "";

            //Resetting the getters/setters
            hangman.bpHead = false;
            hangman.bpLeft_Arm = false;
            hangman.bpLeft_Leg = false;
            hangman.bpRight_Arm = false;
            hangman.bpRight_Leg = false;
            start_game();
        }
        static void gameOver()
        {
            Console.Clear();
            Console.WriteLine("Game over!");
            Console.WriteLine();
            Console.WriteLine("You guessed wrong too many times!");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void endPrompt()
        {
            Console.Clear();
            Console.WriteLine("Thank you for playing my app, Hangman.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
