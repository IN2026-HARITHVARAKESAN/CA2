namespace BoilerStartup.Services
{
    /// <summary>
    /// Gets inputs from the user
    /// </summary>
    internal class InputGetter
    {
        /// <summary>
        /// Gets input from the user
        /// </summary>
        /// <returns>Input from the user</returns>
        public static string GetInput()
        {
            bool isValidInput = false;
            string? userInput;
            do
            {
                userInput = Console.ReadLine();
                isValidInput = InputValidator.ValidateInput(userInput);

                if (!isValidInput)
                {
                    Console.Write("Invalid Input...Input cannot be null or whitespaces\n Enter your input again : ");
                }
            }
            while (!isValidInput);

            return userInput;
        }

        /// <summary>
        /// Gets index of the choice from the user
        /// </summary>
        /// <param name="count">Maximum available index</param>
        /// <returns>returns integer of user choice</returns>
        public static int GetIndex(int count)
        {
            bool isValidIndex = false;
            string userInput;
            do
            {
                userInput = GetInput();
                isValidIndex = InputValidator.ValidateIndex(userInput, count);
                if (!isValidIndex)
                {
                    Console.Write($"\nInvalid index...Index should be between 1 to {count}\nEnter your choice index again : ");
                }
            }
            while (!isValidIndex);

            return int.Parse(userInput);
        }
    }
}
