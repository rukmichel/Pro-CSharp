using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    [Serializable]
    class Car
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
        /// 
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

        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>return true if the car moves</returns>
        public bool move() { return true; }
    }
}
