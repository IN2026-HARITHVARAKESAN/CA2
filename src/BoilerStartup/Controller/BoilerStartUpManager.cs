using System.Text;
using System.Timers;
using BoilerStartup.Models;
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

        public BoilerStartUpManager()
        {
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

        private Logger Logger { get; set; }

        private StringBuilder SystemLog { get; set; }

        private Thread RunningBoiler { get; set; }

        private CancellationTokenSource TokenSource { get; set; }

        private CancellationToken Token { get; set; }

        public void ManageBolierOperation(int choice)
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

        public void ToggleRunInterlockSwitch()
        {
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

        public void ResetLockout()
        {
            if (this.InterlockSwitch == InterlockSwitchStatus.Closed)
            {
                this.SystemStatus = Status.Ready;
                Console.WriteLine("Boiler status updated to Ready.");
                return;
            }

            Console.WriteLine("Cannot reset lockout as interlock switch is opened...Try closing the switch");
        }

        public void ViewEventLog()
        {
            Console.WriteLine(this.SystemLog);
        }

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

                this.SystemStatus++;
                Console.WriteLine($"{this.SystemStatus} phase started");
                if (phase == 3)
                {
                    break;
                }

                Thread.Sleep(10000);
            }
        }

        public void StopBoiler()
        {
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

        public void SimulateError()
        {
            if (this.SystemStatus != Status.Operational)
            {
                Console.WriteLine("Cannot simulate error.\nYou can only simulate error when the boiler state is operational.");
                return;
            }

            this.SystemStatus = Status.Lockout;
            this.InterlockSwitch = InterlockSwitchStatus.Open;
        }

        public void StatusChangeHandler()
        {
            this.SystemLog.AppendLine($"{DateTime.Now.ToString()}, Boiler Status Update, Boiler Status changed to {this.SystemStatus.ToString()}.");
        }

        public void InterlockSwitchChangeHandler()
        {
            this.SystemLog.AppendLine($"{DateTime.Now.ToString()}, Toggle Interlock, Interlock Switch toggled to {this.InterlockSwitch.ToString()}.");
        }
    }
}
