using System;

namespace YatSprint
{
    /// <summary>
    /// Creates
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Index of the request
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Collection of actions to execute
        /// </summary>
        public Action[] Actions { get; set; }

        /// <summary>
        /// Strength to send request
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Create a request to a certain screen index, with a strength, and optional actions
        /// </summary>
        /// <param name="index">Index to assert next screen</param>
        /// <param name="strength">Strength to place command at</param>
        public Request(int index, int strength)
        {
            Index = index;
            Strength = strength;
        }

        /// <summary>
        /// Create a request to a certain screen index, with a strength, and optional actions
        /// </summary>
        /// <param name="index">Index to assert next screen</param>
        /// <param name="strength">Strength to place command at</param>
        /// <param name="actions">Actions to execute</param>
        public Request(int index, int strength, params Action[] actions) : this(index, strength)
        {
            Actions = actions;
        }
    }
}
