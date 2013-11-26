using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    /// <summary>
    /// Represents the current state of the simulation
    /// </summary>
    [Serializable]
    public enum State
    {
        Running,
        Paused,
        Stopped
    }
}
