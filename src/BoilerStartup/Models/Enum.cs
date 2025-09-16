namespace BoilerStartup.Models
{
    /// <summary>
    /// Enum of System Statuses
    /// </summary>
    internal enum Status
    {
        /// <summary>
        /// System is in Lockout state
        /// </summary>
        Lockout,

        /// <summary>
        /// System is in Ready state
        /// </summary>
        Ready,

        /// <summary>
        /// System is in PrePurge state
        /// </summary>
        PrePurge,

        /// <summary>
        /// System is in Ignition state
        /// </summary>
        Ignition,

        /// <summary>
        /// System is in Operational state
        /// </summary>
        Operational,
    }

    /// <summary>
    /// Enum of Interlock Switch statuses
    /// </summary>
    internal enum InterlockSwitchStatus
    {
        /// <summary>
        /// Interlock switch in Open state
        /// </summary>
        Open,

        /// <summary>
        /// Interlock switch in Closed state
        /// </summary>
        Closed,
    }
}
