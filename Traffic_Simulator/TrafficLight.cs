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
    struct TrafficLight
    {
        /// <summary>
        /// Represents the current color of a traffic light.
        /// </summary>
        private Color _color;

        /// <summary>
        /// Amount of seconds that a traffic light should be green on a cycle.
        /// </summary>
        private int _greenLightTime;
    }
}
