using System;

namespace YatSprint
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (YatSprintGame game = new YatSprintGame())
            {
                game.Run();
            }
        }
    }
}

