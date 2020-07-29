using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Random_File_Generator
{
    public class Program
    {
        private const int MaxByteArraySize = 2147483591;
        public static void Main(string[] argsArray)
        {
            List<string> args = new List<string>(argsArray);

            if (args.Count < 2)
            {
                Console.WriteLine("Not enough arguments supplied.");
                return;
            }

            string filePath = args[0];

            long numberOfBytesToGenerate;            
            if(!long.TryParse(args[1],out numberOfBytesToGenerate))
            {   
                Console.WriteLine($"Argument {args[1]} is not a file length.");
                return;
            }
            if(numberOfBytesToGenerate < 0)
            {
                Console.WriteLine($"Argument {numberOfBytesToGenerate} is not a file length.");
                return;
            }

            bool fileExists = File.Exists(filePath);
            if (fileExists)
            {
                Console.WriteLine("File already exists.");
                return;
            }
            else
            {   
                long numBytesLeft = numberOfBytesToGenerate;
                using(RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                using(FileStream fs = new FileStream(filePath, FileMode.Append))
                {
                    bool isDone = false;
                    while (!isDone)
                    {
                        byte[] randomBytes;
                        if(numBytesLeft >= MaxByteArraySize )
                        {
                            randomBytes = new byte[MaxByteArraySize];
                        }
                        else
                        {
                            randomBytes = new byte[numBytesLeft];
                        }

                        rng.GetBytes(randomBytes);
                        fs.Write(randomBytes);
                        numBytesLeft -= randomBytes.Length;
                        Console.WriteLine($"{randomBytes.Length} random byte(s) written to file.");
                        isDone = numBytesLeft <= 0;
                    }
                }
                Console.WriteLine("File complete!");
            }
        }
    }
}
