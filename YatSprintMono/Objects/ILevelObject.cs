using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.Objects
{
    public interface ILevelObject : IObject
    {
        /// <summary>
        /// Synchronizes the object with the level
        /// </summary>
        /// <param name="amount"></param>
        void ShiftX(float amount);

        /// <summary>
        /// Is the object dead or off the screen
        /// </summary>
        /// <returns>Whether it is dead</returns>
        bool Dead();
    }
}
