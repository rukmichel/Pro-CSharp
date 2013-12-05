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
    [Serializable]
    public class Street
    {
        /// <summary>
        /// Represents the position of the street on the crossing.
        /// </summary>
        private Direction _position;

        public Direction Position
        {
            get { return _position ; }
            set { _position = value; }
        }

        /// <summary>
        /// Represents the first entrance lanes of the crossing.
        /// </summary>
        private Car[] _laneEnter1;

        public Car[] LaneEnter1
        {
            get { return _laneEnter1; }

            set 
            {
                _laneEnter1 = value;
            }
        }

        /// <summary>
        /// Represents the second entrance lane of the crossing.
        /// </summary>
        private Car[] _laneEnter2;

        public Car[] LaneEnter2
        {
            get { return _laneEnter2; }

            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    _laneEnter2[i] = value[i];
                }
            }
        }

        /// <summary>
        /// Represents the exit lane of the crossing.
        /// </summary>
        private Car[] _laneExit;

        public Car[] LaneExit
        {
            get { return _laneExit; }

            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    _laneExit[i] = value[i];
                }
            }
        }

        private Car[][] _lanes = new Car[3][];

        public Car[][] Lanes
        {
            get { return _lanes; }
            set { _lanes = value; }
        }
        /// <summary>
        /// Assigns atributes.
        /// </summary>
        /// <param name="t">The type of crossing it belongs to.</param>
        /// <param name="d">Position of the street in the crossing.</param>
        public Street(Type t, Direction d)
        {
            _laneEnter1 = new Car[3];
            _laneEnter2 = new Car[3];
            _laneExit = new Car[3];
            this._position = d;

            if (t == typeof(Crossing_2)  &&
                (d == Direction.North || d== Direction.Center || d==Direction.South))
            {
                
                _laneEnter2 = null;
            }
            _lanes[0] = _laneEnter1;
            _lanes[1] = _laneEnter2;
            _lanes[2] = _laneExit;
         }
    }
}
