using System.Runtime.CompilerServices;

namespace BoilerStartup.Controller
{
    internal class Logger
    {
        public delegate void Log();

        public event Log OnStatusChange;

        public event Log OnInterlockSwitchChange;

        public void HandleStatusChange()
        {
            this.OnStatusChange?.Invoke();
        }

        public void HandleInterlockSwitchChange()
        {
            this.OnInterlockSwitchChange?.Invoke();
        }
    }
}
