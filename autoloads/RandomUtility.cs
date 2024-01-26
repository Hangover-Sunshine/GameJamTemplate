using Godot;
using Godot.Collections;

namespace HS.Autoloads
{
    public partial class RandomUtility : Node
    {
        /// <summary>
        /// A global static for the internal Godot RandomNumberGenerator. If you don't want to constantly make RandomNumberGenerator objects,
        /// this static is for you!
        /// 
        /// NOTE: This uses PSUEDO-RANDOM number generation! For secure RNG, consider the myriad of alternatives!
        /// </summary>
        public static readonly RandomNumberGenerator RNG = new RandomNumberGenerator();
        
        /// <summary>
        /// Choose a number of elements from the given list, WITH replacement!
        /// </summary>
        /// <param name="input"></param>
        /// <param name="num"></param>
        /// <returns>Either an empty list, or a list with NUM of choices randomly selected.</returns>
        Array Choose(Array input, int num = 1)
        {
            if (num <= 0) return new Array();

            Array choices = new Array();

            for(int i = 0; i < num; i++)
            {
                choices.Add(input[RNG.RandiRange(0, input.Count - 1)]);
            }
            
            return choices;
        }

        /// <summary>
        /// Choose a number of elements from the given list, WITHOUT replacement!
        /// </summary>
        /// <param name="input"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Array ChooseNoReplace(Array input, int num = 1)
        {
            if (num <= 0) return new Array();
            if (num >= input.Count) return input;

            Array choices = new Array();
            Array localInput = input.Duplicate();

            for (int i = 0; i < num; i++)
            {
                int choice = RNG.RandiRange(0, input.Count - 1);
                choices.Add(localInput[choice]);
                localInput.RemoveAt(choice);
            }

            return choices;
        }

    }
}
