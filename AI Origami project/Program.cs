using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_Chunk_String_Matching
{
    class Program
    {
        static ArrayList patterns = null;
        static void Main(string[] args)
        {
            //Generate Patterns
            generatePatterns();

            //Continuously ask User
            bool run = true;
            while (run) {
                //Get User Input
                String userInput = "";
                Console.Write("Enter a string of characters to generate obstacles ");
                userInput = Console.ReadLine();

                for (int i = 0; i < patterns.Count; i++) {
                    Console.Write(patterns[i] + " ");
                }

                //Match patterns using Rchunk pattern matching
                ArrayList returnedObstaclesPattern = matchPattern(userInput);

                if (returnedObstaclesPattern.Count > 0)
                {
                    Console.WriteLine("Returned a matching pattern");
                    for (int i = 0; i < returnedObstaclesPattern.Count; i++)
                    {
                        Console.Write(returnedObstaclesPattern[i]);
                    }
                }
                else
                {
                    Console.WriteLine("No matching pattern found");
                }

                //nEW LINE
                Console.WriteLine();
            }

            

        }

        //Generate a pool of patterns to be used
        //Starts with a character to represent to obstacle type followed by a digit to represent size (sizes 1 to 5, 1 smallest, 5 biggest)
        //R stands for rectangle, S stands for square
        static private void generatePatterns() {
            patterns = new ArrayList();

            //Add combinations of R and T (Rectangles and Triangles) staring with T
            for (int i = 0; i < 5; i++) {
                patterns.Add("R");
                patterns.Add(i);
                for (int j = 0; j < 5; j++) {
                    patterns.Add("T");
                    patterns.Add(j);
                }
            }

            //Add combinations of R and T (Rectangles and Triangles) starting with T
            for (int i = 0; i < 5; i++)
            {
                patterns.Add("T");
                patterns.Add(i);
                for (int j = 0; j < 5; j++)
                {
                    patterns.Add("R");
                    patterns.Add(j);
                }
            }

            //Add combinations of Rectangles R1T1, R1T2, R1T3..... R5T5
            for (int i = 0; i < 5; i++)
            {
                int intCounter = 5;
                while (intCounter >= 0) {
                    patterns.Add("R");
                    patterns.Add(i);
                    patterns.Add("T");
                    patterns.Add(intCounter);
                    intCounter--;
                }
            }

            //Add combinations of Rectangles T1R1, T1R2, T1R3..... T5R5
            for (int i = 0; i < 5; i++)
            {
                int intCounter = 5;
                while (intCounter >= 0)
                {
                    patterns.Add("T");
                    patterns.Add(i);
                    patterns.Add("R");
                    patterns.Add(intCounter);
                    intCounter--;
                }
            }

            ////Add Combinations of Triangles
            //for (int i = 0; i < 5; i++)
            //{
            //    patterns.Add("T");
            //    patterns.Add(i);
            //    for (int j = 0; j < 5; j++)
            //    {
            //        patterns.Add("T");
            //        patterns.Add(j);
            //    }
            //}

            //A case of a certain number of triangles then rectangle, or a certain number of rectangle then triangle
        }

        //Pattern Matching using Rchunks, R is 6.. those are 3 obstacles max at once
        //String patterns are limited to an R-Chunk to prevent the user from bombarding the space with all shapes and sizes
        static private ArrayList matchPattern(string userInput) {
            ArrayList matchedPattern = new ArrayList();
            
            //Make sure user input is trimmed to 6 characters
            //first 6 characters
            string userString = "";
            if (userInput.Length > 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    userString += userInput[i];
                }
            }
            else {
                for (int i = 0; i < userInput.Length; i++)
                {
                    userString += userInput[i];
                }
            }

            //User string is of length 6
            //Remaining characters to loop = 
            int strLength = patterns.Count - 6;

            for (int i = 0; i <= strLength; i++) {
                //Take a chunk of the patterns Vector
                string strPatternsChunk = "";
                if (userInput.Length > 6)
                {
                    for (int w = i; w < 6 + i; w++)
                    {
                        //userString += userInput[w];
                        strPatternsChunk += patterns[w];
                    }
                }
                else
                {
                    for (int x = i; x < userInput.Length + i; x++)
                    {
                        strPatternsChunk += patterns[x];
                    }
                }
             
                //Console.WriteLine(strPatternsChunk);
                //Console.WriteLine(userString);

                //Compare it with the userInput string
                if (strPatternsChunk == userString) {
                    //Pattern Matched
                    for (int k = 0; k < userString.Length; k++) {
                        matchedPattern.Add(userString[k]);
                    }
                    return matchedPattern;
                }
            }
            //No matched Pattern
            ArrayList noMatch = new ArrayList();
            return noMatch;
        }
    }
}
