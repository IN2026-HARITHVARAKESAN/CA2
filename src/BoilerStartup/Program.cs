using BoilerStartup.Controller;
using BoilerStartup.Services;

namespace BoilerStartup
{
    /// <summary>
    /// Root class of the application
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Root method of the application
        /// </summary>
        public static void Main()
        {
            BoilerStartUpManager boilerStartUpManager = new BoilerStartUpManager();

            int userChoiceOfMenu;
            do
            {
                Console.Write("Enter Your Choice of index : ");
                userChoiceOfMenu = InputGetter.GetIndex(7);

                boilerStartUpManager.ManageBolierOperation(userChoiceOfMenu);
            }
            while (userChoiceOfMenu != 7);
        }
    }
}