using System.Runtime.CompilerServices;

namespace BoilerStartup.Controller
{
    /// <summary>
    /// Logs the updates
    /// </summary>
    internal class Logger
    {
        /// <summary>
        /// methods to log
        /// </summary>
        public delegate void Log();

        /// <summary>
        /// Event triggered when status is updated
        /// </summary>
        public event Log OnStatusChange;

        /// <summary>
        /// Event triggered when Interlock Switch is updated
        /// </summary>
        public event Log OnInterlockSwitchChange;

        /// <summary>
        /// Handles changes in state
        /// </summary>
        public void HandleStatusChange()
        {
            this.OnStatusChange?.Invoke();
        }

        /// <summary>
        /// Handles changes in InterlockSwitch
        /// </summary>
        public void HandleInterlockSwitchChange()
        {
            this.OnInterlockSwitchChange?.Invoke();
        }
    }
}
