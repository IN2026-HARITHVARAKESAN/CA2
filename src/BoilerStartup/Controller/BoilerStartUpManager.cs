using System.Text;
using System.Timers;
using BoilerStartup.Interface;
using BoilerStartup.Models;
using ConsoleTables;
using Timer = System.Timers.Timer;

namespace BoilerStartup.Controller
{
    /// <summary>
    /// Manages Boiler startup controller
    /// </summary>
    internal class BoilerStartUpManager
    {
        private Status _systemStatus;

        private InterlockSwitchStatus _interlockSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerStartUpManager"/> class.
        /// </summary>
        /// <param name="fileHandler">Instance of FileHandler</param>
        public BoilerStartUpManager(IFileHandler fileHandler)
        {
            this.FileHandler = fileHandler;
            this.SystemLog = new StringBuilder();
            this.Logger = new Logger();
            this.Logger.OnStatusChange += StatusChangeHandler;
            this.Logger.OnInterlockSwitchChange += InterlockSwitchChangeHandler;
            this.SystemStatus = Status.Lockout;
            this.InterlockSwitch = InterlockSwitchStatus.Open;
            this.TokenSource = new CancellationTokenSource();
            this.Token = this.TokenSource.Token;
        }

        private Status SystemStatus
        {
            get
            {
                return this._systemStatus;
            }

            set
            {
                this._systemStatus = value;
                this.Logger.HandleStatusChange();
            }
        }

        private InterlockSwitchStatus InterlockSwitch
        {
            get
            {
                return this._interlockSwitch;
            }

            set
            {
                this._interlockSwitch = value;
                this.Logger.HandleInterlockSwitchChange();
            }
        }

        private IFileHandler FileHandler { get; set; }

        private Logger Logger { get; set; }

        private StringBuilder SystemLog { get; set; }

        private Thread RunningBoiler { get; set; }

        private CancellationTokenSource TokenSource { get; set; }

        private CancellationToken Token { get; set; }

        /// <summary>
        /// Manages Boiler operations and navigate
        /// </summary>
        /// <param name="choice">User choice of menu</param>
        public void ManageBoilerOperation(int choice)
        {
            switch (choice)
            {
                case 1:
                    this.RunningBoiler = new Thread(() => this.StartBoiler(this.Token));
                    this.RunningBoiler.Start();
                    break;
                case 2:
                    this.StopBoiler();
                    break;
                case 3:
                    this.SimulateError();
                    break;
                case 4:
                    this.ToggleRunInterlockSwitch();
                    break;
                case 5:
                    this.ResetLockout();
                    break;
                case 6:
                    this.ViewEventLog();
                    break;
                case 7:
                    return;
            }
        }

        /// <summary>
        /// Toggles Run interlock switch to open / close
        /// </summary>
        public void ToggleRunInterlockSwitch()
        {
            Console.Clear();

            if (this.InterlockSwitch == InterlockSwitchStatus.Open)
            {
                this.InterlockSwitch = InterlockSwitchStatus.Closed;
            }
            else
            {
                this.InterlockSwitch = InterlockSwitchStatus.Open;
            }

            Console.WriteLine($"Run Interlock switch toggled to {this.InterlockSwitch.ToString()}");
        }

        /// <summary>
        /// Resets Lockout to ready if interlock switch is closed
        /// </summary>
        public void ResetLockout()
        {
            Console.Clear();
            if (this.InterlockSwitch == InterlockSwitchStatus.Closed)
            {
                this.SystemStatus = Status.Ready;
                Console.WriteLine("Boiler status updated to Ready.");
                return;
            }

            Console.WriteLine("Cannot reset lockout as interlock switch is opened...Try closing the switch");
        }

        /// <summary>
        /// Displays event log to the user
        /// </summary>
        public void ViewEventLog()
        {
            string[] logs = this.SystemLog.ToString().Split("\n");
            var table = new ConsoleTable("Date Time", "Event", "Event Data");
            foreach (string line in logs)
            {
                string[] logData = line.Split(",");
                if (logData.Length < 3)
                {
                    continue;
                }

                table.AddRow(logData[0], logData[1], logData[2]);
            }

            table.Write();
        }

        /// <summary>
        /// Start Boiler operation like pre-purge, ignition and operational
        /// </summary>
        /// <param name="token">Cancellation token to abort the process</param>
        public void StartBoiler(CancellationToken token)
        {
            if (this.SystemStatus != Status.Ready)
            {
                Console.WriteLine("To start the boiler Interlock switch should be closed and System status must be ready....\n" +
                    "Try again after changing the boiler state.");
                return;
            }

            for (int phase = 0; phase < 3; phase++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                this.SystemStatus = this.SystemStatus + 1;
                if (phase == 3)
                {
                    break;
                }

                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Stops Boiler when it is running
        /// </summary>
        public void StopBoiler()
        {
            Console.Clear();
            if (this.SystemStatus == Status.Ready || this.SystemStatus == Status.Lockout)
            {
                Console.WriteLine("Boiler is not yet started.");
            }
            else
            {
                this.TokenSource.Cancel();
                this.SystemStatus = Status.Lockout;
                this.InterlockSwitch = InterlockSwitchStatus.Open;

                Console.WriteLine("Boiler stopped");
            }
        }

        /// <summary>
        /// Simulate Errors in boiler
        /// </summary>
        public void SimulateError()
        {
            Console.Clear();
            if (this.SystemStatus != Status.Operational)
            {
                Console.WriteLine("Cannot simulate error.\nYou can only simulate error when the boiler state is operational.");
                return;
            }

            this.SystemStatus = Status.Lockout;
            this.InterlockSwitch = InterlockSwitchStatus.Open;
        }

        /// <summary>
        /// System status change handler
        /// </summary>
        private void StatusChangeHandler()
        {
            string logData = $"{DateTime.Now.ToString()}, Boiler Status Update, Boiler Status changed to {this.SystemStatus.ToString()}.\n";
            this.SystemLog.Append(logData);
            this.FileHandler.AppendDataToFile(logData);
            this.UpdateStatusInConsole();
        }

        /// <summary>
        /// Interlock switch update handler
        /// </summary>
        private void InterlockSwitchChangeHandler()
        {
            string logData = $"{DateTime.Now.ToString()}, Toggle Interlock, Interlock Switch toggled to {this.InterlockSwitch.ToString()}.\n";

            this.SystemLog.Append(logData);
            this.FileHandler.AppendDataToFile(logData);
        }

        /// <summary>
        /// Updates status to console
        /// </summary>
        private void UpdateStatusInConsole()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            Console.CursorLeft = 0;
            Console.CursorTop = 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"System Status : {this.SystemStatus.ToString()}                ");
            Console.ResetColor();

            Console.CursorLeft = left;
            Console.CursorTop = top;
        }
    }
}
