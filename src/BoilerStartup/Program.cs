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
                Console.WriteLine(
                    "\n[1].Start Boiler Sequence\r\n" +
                    "[2].Stop Boiler Sequence\r\n" +
                    "[3].Simulate Boiler Error.\r\n" +
                    "[4].Toggle Run Interlock Switch\r\n" +
                    "[5].Reset Lockout\r\n" +
                    "[6].View Event Log\r\n" +
                    "[7].Exit Application\r\n");
                Console.Write("Enter Your Choice of index : ");
                userChoiceOfMenu = InputGetter.GetIndex(7);

                boilerStartUpManager.ManageBoilerOperation(userChoiceOfMenu);
            }
            while (userChoiceOfMenu != 7);
        }
    }
}