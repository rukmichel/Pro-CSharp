using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Traffic_Simulator
{
    /// <summary>
    /// Represents a traffic light.
    /// </summary>
    [Serializable]
    public struct TrafficLight
    {
        /// <summary>
        /// Represents the current color of a traffic light.
        /// </summary>
        public Color _color;

        /// <summary>
        /// Amount of seconds that a traffic light should be green on a cycle.
        /// </summary>
        public int _greenLightTime;
    }
}
