using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    /// <summary>
    /// Represents a portion of a crossing.
    /// </summary>
    struct Street
    {
        /// <summary>
        /// Represents the position of the street on the crossing.
        /// </summary>
        private Direction _position;

        /// <summary>
        /// Represents the first entrance lanes of the crossing.
        /// </summary>
        private Car[] _laneEnter1;

        /// <summary>
        /// Represents the second entrance lane of the crossing.
        /// </summary>
        private Car[] _laneEnter2;

        /// <summary>
        /// Represents the exit lane of the crossing.
        /// </summary>
        private Car[] _laneExit;

        /// <summary>
        /// Assigns atributes.
        /// </summary>
        /// <param name="t">The of crossing it belongs to.</param>
        /// <param name="d">Position of the street in the crossing.</param>
        public Street(Type t, Direction d) {
        }
    }
}
