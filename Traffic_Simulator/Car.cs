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

        /// <summary>
        /// How will the car turn at the crossing
        /// </summary>
        Direction Turn {
            get { return _turn; }
            set { _turn = value; }
        }

        /// <summary>
        /// The direction the car is moving towards on a street
        /// </summary>
        Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Total time car waited at a red light
        /// </summary>
        TimeSpan RedLightWaitingTime
        {
            get { return _redLightWaitingTime; }
            set { _redLightWaitingTime = value; }
        }

        /// <summary>
        /// Total time car spent inside the grid
        /// </summary>
        TimeSpan TimeInsideGrid
        {
            get { return _timeInsideGrid; }
            set { _timeInsideGrid = value; }
        }

        /// <summary>
        /// The street which the car is currently occupying
        /// </summary>
        Street Street
        {
            get { return _street; }
            set { _street = value; }
        }

        /// <summary>
        /// Position of the car on the street
        /// </summary>
        int StreetIndex
        {
            get { return _streetIndex; }
            set { _streetIndex = value; } // !TODO: validate
        }

        /// <summary>
        /// Is the car inside the grid?
        /// </summary>
        bool HasEnteredGrid
        {
            get { return _hasEnteredGrid; }
            set { _hasEnteredGrid = value; }
        }

        /// <summary>
        /// is the car outside the grid
        /// </summary>
        bool HasExitedGrid
        {
            get { return _hasExitedGrid; }
            set { _hasExitedGrid = value; }
        }

        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>return true if the car moves</returns>
        public bool move()
        {     
            // check if car is on laneExit
            if (this.Direction == Street.Position)
            {
                // check if car is exiting crossing
                if (this.StreetIndex == 0)
                {
                    // check which crossing comes next
                    // if possible, move car onto street on next crossing
                }
                // check if the next position is vacant
                else if (Street.LaneExit[StreetIndex - 1] == null)
                {
                    // car should move itself onto that position

                    return true;
                }
            }
            
            
            // check if car is on laneEnter2
            if (Street.Position == Direction.North && this.Direction == Direction.East ||
                Street.Position == Direction.East  && this.Direction == Direction.South ||
                Street.Position == Direction.South && this.Direction == Direction.West ||
                Street.Position == Direction.West  && this.Direction == Direction.North ||
                Street.LaneEnter2 != null)
            {
                if (this.StreetIndex == Street.LaneEnter2.Length - 1)
                {
                    // move to the next street
                }
                else if (Street.LaneEnter2[StreetIndex + 1] == null)
                {
                    // car should move itself onto that position
                    return true;
                }

                return false;
            }
            // car is on laneEnter1
            else
            {
                if (this.StreetIndex == Street.LaneEnter1.Length - 1)
                {
                    // move to the next street
                }
                else if (Street.LaneEnter1[StreetIndex + 1] == null)
                {
                    // car should move itself onto that position
                    return true;
                }

                return false;
            }
        }
    }
}
