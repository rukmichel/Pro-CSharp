using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    [Serializable]
    public class Car
    {
        /// <summary>
        /// _turn gives the direction to go for the car.
        /// _direction gives the direction on the car in the lane.
        /// </summary>
        Direction _turn, _direction;

        /// <summary>
        /// _redLightWaitingTime gives the total time the car stopped at traffic light.
        /// _timeInsideGrid gives the total time the car spent in the grid.
        /// </summary>
        TimeSpan _redLightWaitingTime, _timeInsideGrid;

        /// <summary>
        /// The street this car is currently occupying
        /// </summary>
        Street _street;

        /// <summary>
        ///  Represents the position of the car on a street lane.
        /// </summary>
        int _streetIndex;

        /// <summary>
        /// Returns true if the car has entered the grid.
        /// </summary>
        bool _hasEnteredGrid;

        /// <summary>
        /// Returns true if the car has exited the grid.
        /// </summary>
        bool _hasExitedGrid;

        Direction Turn {
            get { return _turn; }
            set { _turn = value; }
        }

        Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        TimeSpan RedLightWaitingTime
        {
            get { return _redLightWaitingTime; }
            set { _redLightWaitingTime = value; }
        }

        TimeSpan TimeInsideGrid
        {
            get { return _timeInsideGrid; }
            set { _timeInsideGrid = value; }
        }

        Street Street
        {
            get { return _street; }
            set { _street = value; }
        }

        int StreetIndex
        {
            get { return _streetIndex; }
            set { _streetIndex = value; } // !TODO: validate
        }

        bool HasEnteredGrid
        {
            get { return _hasEnteredGrid; }
            set { _hasEnteredGrid = value; }
        }

        bool HasExitedGrid
        {
            get { return _hasExitedGrid; }
            set { _hasExitedGrid = value; }
        }

        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>return true if the car moves</returns>
        public bool move() { return true; }
    }
}
