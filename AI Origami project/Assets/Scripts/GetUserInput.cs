using UnityEngine;
using System.Collections;
using System;

//DANIEL OGWOK

public class GetUserInput : MonoBehaviour
{
    //Variables
    private ArrayList patternStringsList = new ArrayList();  //List of Strings that Ruan will access
    public ArrayList patterns = null;
    private int intCounter = 1;  //From Ruan to increase Difficulty
    public ArrayList userInputs = new ArrayList();

    //Get and Set Counter
    public int IntCounter
    {
        get => intCounter; set => intCounter = value;
    } //Counter to keep track of how many obstacles generated
    public ArrayList PatternStringsList
    {
        get => patternStringsList; set => patternStringsList = value;
    } //List of Valid Strings from User Input

    void Start()
    {

    }

    void Update()
    {

    }

    // ----------------MY FUNCTIONS ---------------------//
    //Generate a pool of patterns to be used
    //Starts with a character to represent to obstacle type followed by a digit to represent size (sizes 1 to 5, 1 smallest, 5 biggest)
    //R stands for rectangle, S stands for square
    public void generatePatterns()
    {
        patterns = new ArrayList();

        //Add combinations of W padded with extra characters
        for(int i = 3; i <= 6; i++)
        {
            for(int j = 3; j <= 9; j++)
            {
                for(int k = 3; k <= 6; k++)
                {
                    for(int l = 3; l <= 6; l++)
                    {
                        //W
                        patterns.Add("W");
                        //(3-6)
                        patterns.Add(i);
                        //(3-9)
                        patterns.Add(j);
                        //(3-6)
                        patterns.Add(k);
                        //(3-6)
                        patterns.Add(l);
                    }
                }
            }
        }

        //Add combinations of R padded with extra characters
        for(int i = 3; i <= 6; i++)
        {
            for(int j = 3; j <= 9; j++)
            {
                for(int k = 3; k <= 6; k++)
                {
                    for(int l = 3; l <= 6; l++)
                    {
                        //R
                        patterns.Add("R");
                        //(3-6)
                        patterns.Add(i);
                        //(3-9)
                        patterns.Add(j);
                        //(3-6)
                        patterns.Add(k);
                        //(3-6)
                        patterns.Add(l);
                    }
                }
            }
        }


    }

    //Pattern Matching using Rchunks, R is 6.. those are 3 obstacles max at once
    //String patterns are limited to an R-Chunk to prevent the user from bombarding the space with all shapes and sizes
    public ArrayList matchPattern(string userInput)
    {
        ArrayList matchedPattern = new ArrayList();

        //ArrayList of user strings converted to size 6 each
        ArrayList userStrings = new ArrayList();

        //Make sure user input is trimmed to 6 characters
        //first 6 characters
        string userString = "";
        if(userInput.Length > 5)
        {
            for(int i = 0; i < userInput.Length; i++)
            {
                //Reset string 
                userString = "";
                //Remaining String longer than 5
                if(userInput.Length - i >= 5)
                {
                    //Get Chunk of 5
                    for(int j = i; j < 5 + i; j++)
                    {
                        userString += userInput[j];
                    };
                    //Add chunk to ArrayList
                    userStrings.Add(userString);
                }
            }
        }
        else
        {
            for(int i = 0; i < userInput.Length; i++)
            {
                userString += userInput[i];
            }
            //Add to List of Strings
            userStrings.Add(userString);
        }

        //User string is of length 5
        //Remaining characters to loop = 
        int strLength = patterns.Count - 5;

        for(int i = 0; i <= strLength; i++)
        {
            //Take a chunk of the patterns Vector
            string strPatternsChunk = "";
            if(userInput.Length > 5)
            {
                for(int w = i; w < 5 + i; w++)
                {
                    //userString += userInput[w];
                    strPatternsChunk += patterns[w];
                }
            }
            else
            {
                for(int x = i; x < userInput.Length + i; x++)
                {
                    strPatternsChunk += patterns[x];
                }
            }

            //Console.WriteLine(strPatternsChunk);
            //Console.WriteLine(userString);

            //Loop thru all User Strings
            for(int s = 0; s < userStrings.Count; s++)
            {
                //Current String
                String currentString = (string)userStrings[s];
                //Compare it with the userInput string
                if(strPatternsChunk.ToUpper() == currentString.ToUpper() && (currentString[0].CompareTo('W') == 0 || currentString[0].CompareTo('R') == 0))
                {
                    //Pattern Matched
                    for(int k = 0; k < currentString.Length; k++)
                    {
                        matchedPattern.Add(currentString[k]);
                    }
                    //return matchedPattern;
                    //Console.WriteLine(currentString);
                }
            }

        }
        //Return mATCH lIST
        if(matchedPattern.Count > 1)
        {
            return matchedPattern;
        }
        //No matched Pattern
        ArrayList noMatch = new ArrayList();
        return noMatch;
    }

    /**
     * Increase difficulty of game
     * This is based on how far the player is in the game, probably based on an integer that keeps track of the frame value
     * or some other counter
     * When difficulty increases, the user input will not determine how big the obstacles are.. they automatically become bigger,
     * starting with the smallest
     * It will mutate values to be sent to obstacle graphics generator
     **/
    public String increaseDifficulty(ArrayList returnedObstaclesPattern, int frameValue)
    {
        string newValues = "";
        int newSize = 0;
        //Start from index 1 as W/R does not matter
        for(int i = 1; i < returnedObstaclesPattern.Count; i++)
        {
            //Get Value that represents size, eg. 3 - 6 second index[1]
            newSize = (int)returnedObstaclesPattern[i] * (frameValue / 10); //Takes about 10 obstacles for it to increase in size
            //Set new value to that index
            //newValues[i] = newSize;
            newValues += newSize;
        }
        return newValues;
    }


}