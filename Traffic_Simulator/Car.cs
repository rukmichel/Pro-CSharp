using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
        private int[] _streetIndex = new int[2];
        public int[] StreetIndex
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

        private void setFirstLane()
        {
            Console.WriteLine(this.Street.GetType().ToString());

            switch (this.Turn)
            {
                case Direction.North:
                    if (this.Direction == Direction.East)
                        this.Street.LaneEnter1[0] = this;
                    if (this.Direction == Direction.South)
                        this.Street.LaneEnter1[0] = this;
                    if (this.Direction == Direction.West)
                        this.Street.LaneEnter2[0] = this;
                    break;

                case Direction.East:
                    if (this.Direction == Direction.North)
                        this.Street.LaneEnter2[0] = this;
                    else if (this.Direction == Direction.South)
                        this.Street.LaneEnter1[0] = this;
                    else if (this.Direction == Direction.West)
                        this.Street.LaneEnter1[0] = this;

                    break;

                case Direction.South:
                    if (this.Direction == Direction.North)
                        this.Street.LaneEnter1[0] = this;
                    else if (this.Direction == Direction.East)
                        this.Street.LaneEnter2[0] = this;
                    else if (this.Direction == Direction.West)
                        this.Street.LaneEnter1[0] = this;

                    break;

                case Direction.West:
                    if (this.Direction == Direction.North)
                        this.Street.LaneEnter2[0] = this;
                    else if (this.Direction == Direction.East)
                        this.Street.LaneEnter1[0] = this;
                    else if (this.Direction == Direction.South)
                        this.Street.LaneEnter1[0] = this;

                    break;
            }

            this.StreetIndex[0] = 0;
            this.StreetIndex[1] = 0;
        }


        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>returns true if the car moves</returns>
        public bool move()
        {
            if (this.HasEnteredGrid == false && this.HasEnteredGrid == false)
            {
                this.calculateTurn();
                this.HasEnteredGrid = true;
                this.setFirstLane();
            }

            // check if car is on laneExit
            if (this.Direction == Street.Position)
            {
                // check if car is exiting crossing
                if (this.StreetIndex[1] == 0)
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
                else if (Street.LaneExit[StreetIndex[1] - 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneExit[StreetIndex[1]] = null;
                    Street.LaneExit[--StreetIndex[1]] = this;
                    return true;
                }

                return false;
            }
            
            EnterLane:

            // if car is at the intersection
            if (Street.Position == Direction.Center)
            {
                // first find which street the car is on
                int currentStreetID = -1;

                if (this.Street.LaneEnter1[this.StreetIndex[1]] == this)
                    currentStreetID = 0;
                else if (this.Street.LaneEnter2[this.StreetIndex[1]] == this)
                    currentStreetID = 1;
                else if (this.Street.LaneExit[this.StreetIndex[1]] == this)
                    currentStreetID = 2;

                switch (currentStreetID)
                {
                    // if car is on LaneEnter1
                    case 0:
                        // in both cases, car needs to move towards the East
                        if (this.Turn == Direction.North || this.Turn == Direction.East)
                        {
                            // if we're on crossing1
                            if (this.Street.LaneEnter2 != null)
                            {
                                if (this.Street.LaneEnter2[this.StreetIndex[1]] == null) 
                                {
                                    this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = this;
                                    return true;
                                }

                                // cannot move cos LaneEnter2 is occupied
                                return false;
                            }
                            // if we're on Crossing2
                            else
                            {
                                if (this.Street.LaneExit[this.StreetIndex[1]] == null)
                                {
                                    this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                                    this.Street.LaneExit[this.StreetIndex[1]] = this;
                                    return true;
                                }

                                // cannot move cos LaneExit is occupied
                                return false;
                            }
                        }
                        else if (this.Turn == Direction.South)
                        {
                            // if car has reached end of the street
                            if (this.StreetIndex[1] == 2 && this.Crossing.StreetS.LaneExit[2] == null)
                            {
                                this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                                this.Street = this.Crossing.StreetS;
                                this.Street.LaneExit[2] = this;
                                this.StreetIndex[1] = 2;
                                return true;
                            }
                            // if we're continuing to move inside the lane
                            else if (this.Street.LaneEnter1[this.StreetIndex[1] + 1] == null)
                            {
                                this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                                this.Street.LaneEnter1[++this.StreetIndex[1]] = this;
                                return true;
                            }

                            return false;
                        }
                        else if (this.Turn == Direction.West)
                        {
                            // check if car can move to StreetW
                            if (this.Crossing.StreetW.LaneExit[2] == null)
                            {
                                this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                                this.Street = this.Crossing.StreetW;
                                this.Street.LaneExit[2] = this;
                                this.StreetIndex[1] = 2;
                                return true;
                            }

                            return false;
                        }

                        break;

                    // if car is on LaneEnter2
                    //   the car can never move to another street from this lane
                    case 1:
                        if (this.Turn == Direction.North)
                        {
                            if (this.Street.LaneExit[this.StreetIndex[1]] == null)
                            {
                                this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                this.Street.LaneExit[this.StreetIndex[1]] = this;
                                return true;
                            }

                            return false;

                        }
                        else if (this.Turn == Direction.East)
                        {
                            // if we're at the end of the lane, change lanes
                            if (this.StreetIndex[1] == 2)
                            {
                                if (this.Street.LaneExit[this.StreetIndex[1]] == null)
                                {
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                    this.Street.LaneExit[this.StreetIndex[1]] = this;
                                    return true;
                                }
                            }
                            // else continue down the street
                            else
                            {
                                if (this.Street.LaneEnter2[this.StreetIndex[1] + 1] == null)
                                {
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                    this.Street.LaneEnter2[++this.StreetIndex[1]] = this;
                                    return true;
                                }
                            }

                            return false;
                        }
                        else if (this.Turn == Direction.South)
                        {
                            if (this.Street.LaneEnter1[this.StreetIndex[1]] == null)
                            {
                                this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                this.Street.LaneEnter1[this.StreetIndex[1]] = this;
                                return true;
                            }

                            return false;
                        }
                        else if (this.Turn == Direction.West)
                        {
                            if (this.StreetIndex[1] == 0)
                            {
                                if (this.Street.LaneEnter1[this.StreetIndex[1]] == null)
                                {
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                    this.Street.LaneEnter1[this.StreetIndex[1]] = this;

                                    return true;
                                }
                            }
                            else
                            {
                                if (this.Street.LaneEnter2[this.StreetIndex[1] - 1] == null)
                                {
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                                    this.Street.LaneEnter2[--this.StreetIndex[1]] = this;
                                    return true;
                                }
                            }
                        }
                        
                        break;

                    // if car is on LaneExit
                    case 2:
                        if (this.Turn == Direction.North)
                        {
                            if (this.StreetIndex[1] == 0)
                            {
                                if (this.Crossing.StreetN.LaneExit[2] == null)
                                {
                                    this.Street.LaneExit[this.StreetIndex[1]] = null;
                                    this.Street = this.Crossing.StreetN;
                                    this.Street.LaneExit[2] = this;
                                    this.StreetIndex[1] = 2;
                                    return true;
                                }
                            }
                            else if (this.Street.LaneExit[this.StreetIndex[1] - 1] == null)
                            {
                                this.Street.LaneExit[this.StreetIndex[1]] = null;
                                this.Street.LaneExit[--this.StreetIndex[1]] = this;
                                return true;
                            }

                            return false;

                        }
                        else if (this.Turn == Direction.East)
                        {
                            if (this.Crossing.StreetE.LaneExit[2] == null)
                            {
                                this.Street.LaneExit[this.StreetIndex[1]] = null;
                                this.Street = this.Crossing.StreetE;
                                this.Street.LaneExit[2] = this;
                                this.StreetIndex[1] = 2;
                                return true;
                            }

                            return false;
                        }
                        else if (this.Turn == Direction.South || this.Turn == Direction.West)
                        {
                            if (this.Street.LaneEnter2 != null)
                            {
                                if (this.Street.LaneEnter2[this.StreetIndex[1]] == null)
                                {
                                    this.Street.LaneExit[this.StreetIndex[1]] = null;
                                    this.Street.LaneEnter2[this.StreetIndex[1]] = this;
                                    return true;
                                }

                                return false;
                            }
                        }

                        break;

                    default:
                        Console.WriteLine("INVALID POSITION AT INTERSECTION");
                        break;
                }

            }
            // check if car is on laneEnter2
            else if ((Street.Position == Direction.North && this.Direction == Direction.East ||
                Street.Position == Direction.East  && this.Direction == Direction.South ||
                Street.Position == Direction.South && this.Direction == Direction.West ||
                Street.Position == Direction.West  && this.Direction == Direction.North) &&
                Street.LaneEnter2 != null)
            {
                if (this.StreetIndex[1] == Street.LaneEnter2.Length - 1)
                {
                    return moveCenter();
                }
                else if (Street.LaneEnter2[StreetIndex[1] + 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneEnter2[StreetIndex[1]] = null;
                    Street.LaneEnter2[++StreetIndex[1]] = this;
                    return true;
                }

                return false;
            }
            // car is on laneEnter1
            else
            {
                if (this.StreetIndex[1] == Street.LaneEnter1.Length - 1)
                {
                    return moveCenter();
                }
                else if (Street.LaneEnter1[StreetIndex[1] + 1] == null)
                {
                    // car should move itself onto that position
                    Street.LaneEnter1[StreetIndex[1]] = null;
                    Street.LaneEnter1[++StreetIndex[1]] = this;
                    return true;
                }

                return false;
            }

            return false;
        }


        /// <summary>
        /// Called when car is leaving the grid
        /// </summary>
        private void exitGrid()
        {
            this.StreetIndex[1] = -1;
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
        private void calculateTurn()
        {
            if (this.Crossing.FlowE < this.Crossing.EnteredE &&                                                 //if there are still cars to enter a crossing and
                this.Crossing.StreetE.LaneEnter1[0] == null && this.Crossing.StreetE.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbEtoS + this.Crossing.ProbEtoN + this.Crossing.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities

                // Interpreting the generated number:
                //
                //          _       (portion 1)     _  (portion 2)  _        (portion 3)        _
                //          |      direction = N    | direction = S |       direction = W       |
                //          |_______________________|_______________|___________________________|
                //number:   0                    probN        probN + probS        probN+probS+probW = 100
                //Example:  0                       40         40 + 15 = 55              40 + 15 + 45 = 100

                if (prob < this.Crossing.ProbEtoN)// portion 1
                    this.Direction = Direction.North;

                if (prob >= this.Crossing.ProbEtoN && prob < (this.Crossing.ProbEtoN + this.Crossing.ProbEtoS)) //portion 2
                    this.Direction = Direction.South;

                if (prob >= (this.Crossing.ProbEtoN + this.Crossing.ProbEtoS)) //portion 3
                    this.Direction = Direction.West;
            }

            ////////////add new cars to street W
            if (this.Crossing.FlowW < this.Crossing.EnteredW &&                                                 //if there are still cars to enter a crossing and
                this.Crossing.StreetW.LaneEnter1[0] == null && this.Crossing.StreetW.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbWtoS + this.Crossing.ProbWtoN + this.Crossing.ProbWtoE); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbWtoN)
                    this.Direction = Direction.North;
                if (prob >= this.Crossing.ProbWtoN && prob < (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                    this.Direction = Direction.South;
                if (prob >= (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                    this.Direction = Direction.East;
            }

            ////////add new cars to street S
            if (this.Crossing.FlowS < this.Crossing.EnteredS &&                                                   //if there are still cars to enter a crossing and
                this.Crossing.StreetS.LaneEnter1[0] == null                                           //there is an available slot at the entrance of lane enter 1
                && (this.Crossing.GetType() == typeof(Crossing_2) || this.Crossing.StreetS.LaneEnter2[0] == null))//and, in case its a crossing 1, there is an available slot at the entrance of lane 2          
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbStoE + this.Crossing.ProbStoN + this.Crossing.ProbStoW); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbStoN)
                    this.Direction = Direction.North;
                if (prob >= this.Crossing.ProbStoN && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                    this.Direction = Direction.West;
                if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                    this.Direction = Direction.East;
            }

            ////////add new cars to street N
            if (this.Crossing.FlowN < this.Crossing.EnteredN &&                                                   //if there are still cars to enter a crossing
                this.Crossing.StreetN.LaneEnter1[0] == null                                           //and if there is an available slot at the entrance of lane enter 1
                && (this.Crossing.GetType() == typeof(Crossing_2) || this.Crossing.StreetN.LaneEnter2[0] == null))//and, unless it's a crossing_2, if there is an available slot at the entrance of lane 2          
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbEtoS + this.Crossing.ProbEtoN + this.Crossing.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbStoN)
                    this.Direction = Direction.North;
                if (prob >= (this.Crossing.ProbStoN) && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                    this.Direction = Direction.East;
                if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                    this.Direction = Direction.West;
            }
        }

        /// <summary>
        /// Set the new crossing for the car
        /// Only called when a car is moving from one crossing to another
        /// </summary>
        /// <param name="crossingID">The new crossing's ID</param>
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
            this.calculateTurn();
        }

        /// <summary>
        /// Checks if the car can move to the center of the crossing i.e. the intersection
        /// 
        /// This is done by first checking if the light is green,
        /// and then if the next spot is vacant
        /// </summary>
        /// <returns>if car can move to center</returns>
        private bool moveCenter()
        {
            // we know we're already about to move to the center
            // therefore, there are two things blocking our path

            bool trafficLightGreen = false;

            // first, let's check if the traffic light is green
            switch (this.Direction)
            {
                // car is on street south
                case Direction.North:
                    // if on crossing 2, car can only move to StreetN
                    if (this.Crossing.GetType() == typeof(Crossing_2) && this.Turn == Direction.North)
                    {
                        Crossing_2 crossing = (Crossing_2)this.Crossing;
                        trafficLightGreen = crossing.LightStoN.Color == Color.Green;
                    }

                    if (this.Crossing.GetType() == typeof(Crossing_1))
                    {
                        Crossing_1 crossing = (Crossing_1)this.Crossing;

                        if (this.Turn == Direction.North || this.Turn == Direction.East)
                            trafficLightGreen = crossing.LightStoEN.Color == Color.Green;
                        else if (this.Turn == Direction.West)
                            trafficLightGreen = crossing.LightStoW.Color == Color.Green;
                    }

                    break;

                // car is on street west
                case Direction.East:
                    // w.n
                    // w.se
                    if (this.Turn == Direction.East || this.Turn == Direction.South)
                        trafficLightGreen = this.Crossing.LightWtoSE.Color == Color.Green;
                    else if (this.Turn == Direction.North)
                        trafficLightGreen = this.Crossing.LightWtoN.Color == Color.Green;
                    
                    break;

                // car is on street north
                case Direction.South:
                    // if car is on crossing2, it can only move to StreetS
                    if (this.Crossing.GetType() == typeof(Crossing_2) && this.Turn == Direction.South)
                    {
                        Crossing_2 crossing = (Crossing_2)this.Crossing;
                        trafficLightGreen = crossing.LightNtoS.Color == Color.Green;
                    }

                    if (this.Crossing.GetType() == typeof(Crossing_1))
                    {
                        Crossing_1 crossing = (Crossing_1)this.Crossing;

                        if (this.Turn == Direction.South || this.Turn == Direction.West)
                            trafficLightGreen = crossing.LightNtoWS.Color == Color.Green;
                        else if (this.Turn == Direction.East)
                            trafficLightGreen = crossing.LightNtoE.Color == Color.Green;
                    }

                    break;

                // car is on street east
                case Direction.West:
                    // E.S
                    // E.NW
                    if (this.Turn == Direction.North || this.Turn == Direction.West)
                        trafficLightGreen = this.Crossing.LightEtoNW.Color == Color.Green;
                    else if (this.Turn == Direction.South)
                        trafficLightGreen = this.Crossing.LightEtoS.Color == Color.Green;

                    break;
            }

            // check if the car can move to the center only if the traffic light is green
            if (trafficLightGreen)
            {
                // since the traffic light is green and we're moving to the center,
                // we can get the new street
                this.Street = this.Crossing.Intersection;

                // we once again need to know the direction of the car, as well as the turn
                
                // If car is moving from NORTH
                if (this.Direction == Direction.South)
                {
                    // if car is moving to SOUTH or WEST
                    // in both cases, the first postion on the center will be the same
                    if (this.Turn == Direction.South || this.Turn == Direction.West)
                    {
                        if (this.Street.LaneEnter1[0] == null)
                        {
                            this.Street.LaneEnter1[0] = this;
                            this.StreetIndex[1] = 0;
                        }
                    }

                    // If we're moving EAST
                    if (this.Turn == Direction.East)
                    {
                        if (this.Street.LaneEnter2[0] == null)
                        {
                            this.Street.LaneEnter2[0] = this;
                            this.StreetIndex[1] = 0;
                        }
                    }
                }

                // If we're moving from EAST
                if (this.Direction == Direction.West)
                {
                    // if car is moving WEST or NORTH
                    if (this.Turn == Direction.West || this.Turn == Direction.North)
                    {
                        if (this.Street.LaneExit[0] == null)
                        {
                            this.StreetIndex[1] = 0;
                            this.Street.LaneExit[0] = this;
                        }
                    }

                    // if car is moving SOUTH
                    if (this.Turn == Direction.South)
                    {
                        if (this.Street.LaneExit[1] == null)
                        {
                            this.StreetIndex[1] = 1;
                            this.Street.LaneExit[1] = this;
                        }
                    }
                }

                // if we're moving from SOUTH
                if (this.Direction == Direction.North)
                {
                    // if car is moving NORTH or EAST
                    if (this.Turn == Direction.North || this.Turn == Direction.East)
                    {
                        if (this.Street.LaneExit[2] == null)
                        {
                            this.StreetIndex[1] = 2;
                            this.Street.LaneExit[2] = this;
                        }
                    }

                    // if car is moving WEST
                    if (this.Turn == Direction.West)
                    {
                        if (this.Street.LaneEnter2[2] == null)
                        {
                            this.StreetIndex[1] = 2;
                            this.Street.LaneEnter2[2] = this;
                        }
                    }
                }

                // if we're moving from WEST
                if (this.Direction == Direction.East)
                {
                    // if car is moving EAST or SOUTH
                    if (this.Turn == Direction.East || this.Turn == Direction.South)
                    {
                        if (this.Street.LaneEnter1[2] == null)
                        {
                            this.StreetIndex[1] = 2;
                            this.Street.LaneEnter1[2] = this;
                        }
                    }

                    // if car is moving NORTH
                    if (this.Turn == Direction.North)
                    {
                        if (this.Street.LaneEnter1[1] == null)
                        {
                            this.StreetIndex[1] = 1;
                            this.Street.LaneEnter1[1] = this;
                        }
                    }
                }
            }

            return false;
        }
    }
}
