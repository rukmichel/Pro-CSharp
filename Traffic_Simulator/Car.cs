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
        /// The crossing in which this car is located
        /// </summary>
        private Crossing _crossing;
        public Crossing Crossing
        {
            get { return _crossing; }
            set { _crossing = value; }
        }
        
        /// <summary>
        /// _turn gives the direction to go for the car.
        /// _direction gives the direction on the car in the lane.
        /// </summary>
        private Direction _turn, _direction;        
        public Direction Turn
        {
            get { return _turn; }
            set { _turn = value; }
        }
        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// _redLightWaitingTime gives the total time the car stopped at traffic light.
        /// _timeInsideGrid gives the total time the car spent in the grid.
        /// </summary>
        private TimeSpan _redLightWaitingTime, _timeInsideGrid;
        public TimeSpan RedLightWaitingTime
        {
            get { return _redLightWaitingTime; }
            set { _redLightWaitingTime = value; }
        }
        public TimeSpan TimeInsideGrid
        {
            get { return _timeInsideGrid; }
            set { _timeInsideGrid = value; }
        }

        /// <summary>
        /// The street this car is currently occupying
        /// </summary>
        private Street _street;
        public Street Street
        {
            get { return _street; }
            set { _street = value; }
        }

        /// <summary>
        ///  Represents the position of the car on a street lane.
        /// </summary>
        private int _streetIndex;
        public int StreetIndex
        {
            get { return _streetIndex; }
            set { _streetIndex = value; } // !TODO: validate
        }

        /// <summary>
        /// hasEnteredGrid is true if the car has entered the grid.
        /// hasExitedGrid is true if the car has exited the grid.
        /// </summary>
        private bool _hasEnteredGrid, _hasExitedGrid;
        public bool HasEnteredGrid
        {
            get { return _hasEnteredGrid; }
            set { _hasEnteredGrid = value; }
        }
        public bool HasExitedGrid
        {
            get { return _hasExitedGrid; }
            set { _hasExitedGrid = value; }
        }


        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>returns true if the car moves</returns>
        public bool move()
        {     
            // check if car is on laneExit
            if (this.Direction == Street.Position)
            {
                // check if car is exiting crossing
                if (this.StreetIndex == 0)
                {
                    String newCrossingID = "";

                    // check if Car is exiting grid. If not, get next crossing's ID
                    switch (this.Direction)
                    {
                        case Direction.North:
                            // check if we're leaving the grid
                            if (this.Crossing.ID[0] - 1 < 'A')
                            {
                                this.exitGrid();
                                return false;
                            }

                            newCrossingID = Convert.ToString(this.Crossing.ID[0] - 1) + this.Crossing.ID[1];

                            break;

                        case Direction.East:
                            // check if we're leaving the grid
                            if (this.Crossing.ID[1] + 1 > '4')
                            {
                                this.exitGrid();
                                return false;
                            }

                            newCrossingID = this.Crossing.ID[0] + Convert.ToString(this.Crossing.ID[1] + 1);

                            break;

                        case Direction.South:
                            // check if we're leaving the grid
                            if (this.Crossing.ID[0] + 1 > 'C')
                            {
                                this.exitGrid();
                                return false;
                            }

                            newCrossingID = Convert.ToString(this.Crossing.ID[0] + 1) + this.Crossing.ID[1];

                            break;

                        case Direction.West:
                            // check if we're leaving the grid
                            if (this.Crossing.ID[1] - 1 < '1')
                            {
                                this.HasExitedGrid = true;
                                return false;
                            }

                            newCrossingID = this.Crossing.ID[0] + Convert.ToString(this.Crossing.ID[1] - 1);

                            break;
                    }

                    // set new Crossing
                    this.setCrossing(newCrossingID);
                    goto EnterLane;
                }
                // check if the next position is vacant
                else if (Street.LaneExit[StreetIndex - 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneExit[StreetIndex] = null;
                    Street.LaneExit[--StreetIndex] = this;
                    return true;
                }

                return false;
            }
            
            EnterLane:

            // if car is at the intersection
            if (Street.Position == Direction.Center)
            {

            }
            // check if car is on laneEnter2
            else if ((Street.Position == Direction.North && this.Direction == Direction.East ||
                Street.Position == Direction.East  && this.Direction == Direction.South ||
                Street.Position == Direction.South && this.Direction == Direction.West ||
                Street.Position == Direction.West  && this.Direction == Direction.North) &&
                Street.LaneEnter2 != null)
            {
                if (this.StreetIndex == Street.LaneEnter2.Length - 1)
                {
                    return moveCenter();
                }
                else if (Street.LaneEnter2[StreetIndex + 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneEnter2[StreetIndex] = null;
                    Street.LaneEnter2[++StreetIndex] = this;
                    return true;
                }

                return false;
            }
            // car is on laneEnter1
            else
            {
                if (this.StreetIndex == Street.LaneEnter1.Length - 1)
                {
                    return moveCenter();
                }
                else if (Street.LaneEnter1[StreetIndex + 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneEnter1[StreetIndex] = null;
                    Street.LaneEnter1[++StreetIndex] = this;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Called when car is leaving the grid
        /// </summary>
        private void exitGrid()
        {
            this.StreetIndex = -1;
            this.HasExitedGrid = true;
            this.Street = null;
            this.Crossing = null;
            //this.Direction = null;
            //this.Turn = null;
        }

        /// <summary>
        /// Use probablity to figure out which direction the car should take
        /// </summary>
        /// <returns>Direction the car will turn at intersection</returns>
        private Direction calculateTurn()
        {

            return Direction.Center;
        }


        private void setCrossing(string crossingID)
        {
            this.Crossing = Grid.getCrossing(crossingID);

            // now that we know the Crossing we're in, time to figure out the Street
            if (this.Direction == Direction.North)
                this.Street = this.Crossing.StreetS;
            else if (this.Direction == Direction.East)
                this.Street = this.Crossing.StreetW;
            else if (this.Direction == Direction.South)
                this.Street = this.Crossing.StreetN;
            else if (this.Direction == Direction.West)
                this.Street = this.Crossing.StreetE;

            // calculate where the car is going to turn based on probability
            this.Turn = this.calculateTurn();
        }

        private bool moveCenter()
        {
            // we know we're already about to move to the center
            // therefore, there are two things blocking our path

            // first, let's check if the traffic light is green
            switch (this.Direction)
            {
                case Direction.North:


                    break;

                case Direction.East:

                    break;

                case Direction.South:

                    break;

                case Direction.West:

                    break;
            }

            // then, lets check if the  next spot is empty

            return false;
        }
    }
}
