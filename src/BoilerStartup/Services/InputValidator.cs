namespace BoilerStartup.Services
{
    /// <summary>
    /// Validates the input given by user
    /// </summary>
    internal class InputValidator
    {
        /// <summary>
        /// Validate input of the user
        /// </summary>
        /// <param name="userInput">Input from the user</param>
        /// <returns>Returns true if the input is valid else return false</returns>
        public static bool ValidateInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateIndex(string userInput, int count)
        {
            if (int.TryParse(userInput, out int index) && index > 0 && index <= count)
            {
                return true;
            }

            return false;
        }
    }
}
