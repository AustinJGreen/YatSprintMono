using System;
using System.Linq;
using System.Collections.Generic;

namespace YatSprint
{
    public static class Rand
    {
        private static Random rnd = new Random();

        /// <summary>
        /// Creates a random 32-bit number
        /// </summary>
        /// <param name="max">Maximum positive value</param>
        /// <returns>Random 32-bit number</returns>
        public static int Next(int max)
        {
            return rnd.Next(0, max);
        }

        /// <summary>
        /// Creates a random 32-bit number
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Random 32-bit number</returns>
        public static int Next(int min, int max)
        {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Creates a random decimal number
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Random decimal</returns>
        public static float Next(float min, float max)
        {
            float dub = (float)rnd.NextDouble();
            return (float)min + dub % (max - min);
        }

        /// <summary>
        /// An exponential random number, the further away from the max, the less of percent of occurance
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <param name="increm">Increment of occurance</param>
        /// <returns>Random number</returns>
        public static int NextExp(int min, int max, int increm)
        {
            int offset = max - min;
            int total = 0;
            for (int i = offset; i > 0; i -= increm)
                total += i;

            int[] possible = new int[total];
            int inc = offset;
            int curNum = min;
            for (int i = 0; i < possible.Length; i += inc, inc -= increm)
            {
                for (int j = i; j < i + inc; j++)
                {
                    possible[j] = curNum;
                }
                curNum++;
            }

            int rnd = Next(0, possible.Length);
            return possible[rnd];
        }

        /// <summary>
        /// Randomizes and creates a collection of a certain length of numbers equal to a sum
        /// </summary>
        /// <param name="length">Length of the collection</param>
        /// <param name="sum">Desired sum of the collection</param>
        /// <param name="zeroAllowed">Are zero's allowed</param>
        /// <returns>Collection</returns>
        public static int[] Interpolate(int length, int sum, bool zeroAllowed)
        {
            if (!zeroAllowed && length > sum)
                throw new ArgumentException("No whole number possibilities.", "length");
            List<int> indices = new List<int>();
            for (int j = 0; j < length; j++)
                indices.Add(j);
            int[] array = new int[length];
            int total = 0;
            int i = 0;
            while (i < length - 1)
            {
                float max = (sum - total) / (float)length;
                int imax = (int)Math.Round(max) + 1;
                int num = Next((zeroAllowed) ? 0 : 1, imax);
                int index = Next(0, indices.Count);
                array[indices[index]] = num;
                total += num;
                ++i;
                indices.RemoveAt(index);
            }
            array[indices[0]] = sum - total;
            return array;
        }
    }
}
