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
            if (this.HasEnteredGrid == false)
            {
                this.calculateTurn();
                this.HasEnteredGrid = true;
      //          this.Direction = Direction.South;
                this.Turn = Direction.South;
                this.setFirstLane();

                Console.WriteLine("Car has been added to " 
                    + this.Crossing.GetType().ToString()
                    + " at street "
                    + this.Street.Position.ToString()
                    + " and index "
                    + this.StreetIndex[1]);
            }

            // first, lets check if the car is at intersection
            if (this.Street.Position == Direction.Center)
                this.moveInIntersection();
            // if the turn and direction are the same, car needs to exit street
            else if (this.Turn == this.Direction)
                this.moveInLaneExit();
            else
                this.moveInLaneEnter();
            

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
                    this.Turn = Direction.North;

                if (prob >= this.Crossing.ProbEtoN && prob < (this.Crossing.ProbEtoN + this.Crossing.ProbEtoS)) //portion 2
                    this.Turn = Direction.South;

                if (prob >= (this.Crossing.ProbEtoN + this.Crossing.ProbEtoS)) //portion 3
                    this.Turn = Direction.West;
            }

            ////////////add new cars to street W
            if (this.Crossing.FlowW < this.Crossing.EnteredW &&                                                 //if there are still cars to enter a crossing and
                this.Crossing.StreetW.LaneEnter1[0] == null && this.Crossing.StreetW.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbWtoS + this.Crossing.ProbWtoN + this.Crossing.ProbWtoE); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbWtoN)
                    this.Turn = Direction.North;
                if (prob >= this.Crossing.ProbWtoN && prob < (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                    this.Turn = Direction.South;
                if (prob >= (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                    this.Turn = Direction.East;
            }

            ////////add new cars to street S
            if (this.Crossing.FlowS < this.Crossing.EnteredS &&                                                   //if there are still cars to enter a crossing and
                this.Crossing.StreetS.LaneEnter1[0] == null                                           //there is an available slot at the entrance of lane enter 1
                && (this.Crossing.GetType() == typeof(Crossing_2) || this.Crossing.StreetS.LaneEnter2[0] == null))//and, in case its a crossing 1, there is an available slot at the entrance of lane 2          
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbStoE + this.Crossing.ProbStoN + this.Crossing.ProbStoW); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbStoN)
                    this.Turn = Direction.North;
                if (prob >= this.Crossing.ProbStoN && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                    this.Turn = Direction.West;
                if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                    this.Turn = Direction.East;
            }

            ////////add new cars to street N
            if (this.Crossing.FlowN < this.Crossing.EnteredN &&                                                   //if there are still cars to enter a crossing
                this.Crossing.StreetN.LaneEnter1[0] == null                                           //and if there is an available slot at the entrance of lane enter 1
                && (this.Crossing.GetType() == typeof(Crossing_2) || this.Crossing.StreetN.LaneEnter2[0] == null))//and, unless it's a crossing_2, if there is an available slot at the entrance of lane 2          
            {
                int prob = new Random().Next() % (int)(this.Crossing.ProbEtoS + this.Crossing.ProbEtoN + this.Crossing.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities

                if (prob < this.Crossing.ProbStoN)
                    this.Turn = Direction.North;
                if (prob >= (this.Crossing.ProbStoN) && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                    this.Turn = Direction.East;
                if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                    this.Turn = Direction.West;
            }
        }

        private bool moveInLaneEnter()
        {
            // if car is on laneEnter2
            if ((Street.Position == Direction.North && this.Direction == Direction.East ||
                Street.Position == Direction.East && this.Direction == Direction.South ||
                Street.Position == Direction.South && this.Direction == Direction.West ||
                Street.Position == Direction.West && this.Direction == Direction.North) &&
                Street.LaneEnter2 != null)
            {
                if (this.StreetIndex[1] == Street.LaneEnter2.Length - 1)
                {
                    return this.moveToIntersection();
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
                    return this.moveToIntersection();
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
        }

        private bool moveInLaneExit()
        {
            // if the car is moving out of the crossing, the direction it was supposed to turn
            // will be the same as it's current direction
            if (this.Turn == this.Direction)
            {
                if (this.StreetIndex[1] == 0)
                {
                    return this.moveToAnotherCrossing();
                }
                else if (this.Street.LaneExit[this.StreetIndex[1] - 1] == null)
                {
                    this.Street.LaneExit[this.StreetIndex[1]] = null;
                    this.Street.LaneExit[--this.StreetIndex[1]] = this;
                    return true;
                }
            }

            return false;
        }

        private bool moveToIntersection()
        {
            // first we check if the traffic light is green
            bool isTrafficLightGreen = false;

            // we first need to know which traffic light the car is at
            // before that, we need to know which street the car is at
            switch (this.Street.Position)
            {
                case Direction.North:
                    if (this.Crossing.GetType() == typeof(Crossing_1))
                    {
                        Crossing_1 Crossing = (Crossing_1)this.Crossing;

                        if (this.Turn == Direction.South || this.Turn == Direction.West)
                            isTrafficLightGreen = Crossing.LightNtoWS.Color == Color.Green;
                        else if (this.Turn == Direction.East)
                            isTrafficLightGreen = Crossing.LightNtoE.Color == Color.Green;

                        this.Crossing = Crossing;
                    }
                    else if (this.Crossing.GetType() == typeof(Crossing_2))
                    {
                        Crossing_2 Crossing = (Crossing_2)this.Crossing;

                        isTrafficLightGreen = Crossing.LightNtoS.Color == Color.Green;

                        this.Crossing = Crossing;
                    }

                    break;

                case Direction.East:
                    if (this.Turn == Direction.North || this.Turn == Direction.West)
                        isTrafficLightGreen = this.Crossing.LightEtoNW.Color == Color.Green;
                    else if (this.Turn == Direction.South)
                        isTrafficLightGreen = this.Crossing.LightEtoS.Color == Color.Green;

                    break;

                case Direction.South:
                    if (this.Crossing.GetType() == typeof(Crossing_1))
                    {
                        Crossing_1 Crossing = (Crossing_1)this.Crossing;

                        if (this.Turn == Direction.North || this.Turn == Direction.East)
                            isTrafficLightGreen = Crossing.LightStoEN.Color == Color.Green;
                        else if (this.Turn == Direction.West)
                            isTrafficLightGreen = Crossing.LightStoW.Color == Color.Green;

                        this.Crossing = Crossing;
                    }
                    else if (this.Crossing.GetType() == typeof(Crossing_2))
                    {
                        Crossing_2 Crossing = (Crossing_2)this.Crossing;

                        isTrafficLightGreen = Crossing.LightStoN.Color == Color.Green;

                        this.Crossing = Crossing;
                    }

                    break;

                case Direction.West:
                    if (this.Turn == Direction.East || this.Turn == Direction.South)
                        isTrafficLightGreen = this.Crossing.LightWtoSE.Color == Color.Green;
                    else if (this.Turn == Direction.North)
                        isTrafficLightGreen = this.Crossing.LightWtoN.Color == Color.Green;

                    break;
            }

            // now that we know the traffic light is green, we can move the car to the Intersection
            if (true)
            {
                Street newStreet = this.Crossing.Intersection;
                bool carHasMoved = false;

                // once more, we need to know which street the car is on
                switch(this.Street.Position)
                {
                    case Direction.North:
                        if ((this.Turn == Direction.South || this.Turn == Direction.West) &&
                            newStreet.LaneEnter1[0] == null)
                        {
                            this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 0;
                            newStreet.LaneEnter1[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }
                        else if (this.Turn == Direction.East &&
                            newStreet.LaneEnter2[0] == null)
                        {
                            this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 0;
                            newStreet.LaneEnter2[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }

                        break;

                    case Direction.East:
                        if ((this.Turn == Direction.North || this.Turn == Direction.West) &&
                            newStreet.LaneExit[0] == null)
                        {
                            this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 0;
                            newStreet.LaneExit[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }
                        else if (this.Turn == Direction.South && newStreet.LaneExit[1] == null)
                        {
                            this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 1;
                            newStreet.LaneExit[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }

                        break;

                    case Direction.South:
                        if ((this.Turn == Direction.North || this.Turn == Direction.East) &&
                            newStreet.LaneExit[2] == null)
                        {
                            this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 2;
                            newStreet.LaneExit[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }
                        else if (this.Turn == Direction.West && newStreet.LaneEnter2[2] == null)
                        {
                            this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 2;
                            newStreet.LaneEnter2[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }

                        break;

                    case Direction.West:
                        if ((this.Turn == Direction.East || this.Turn == Direction.South) &&
                            newStreet.LaneEnter1[2] == null)
                        {
                            this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 2;
                            newStreet.LaneEnter1[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }
                        else if (this.Turn == Direction.North && newStreet.LaneEnter1[1] == null)
                        {
                            this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                            this.StreetIndex[1] = 1;
                            newStreet.LaneEnter1[this.StreetIndex[1]] = this;

                            carHasMoved = true;
                        }

                        break;
                }

                if (carHasMoved)
                {
                    this.Street = newStreet;
                    return true;
                }
            }

            return false;
        }

        private bool moveNorthInIntersection(String lane)
        {
            Console.WriteLine("TRY MOVE NORTH");
            switch (lane)
            {
                case "laneEnter1":
                    if (this.StreetIndex[1] > 0 && this.Street.LaneEnter1[this.StreetIndex[1] - 1] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[--this.StreetIndex[1]] = this;
                        Console.WriteLine("moved north in le1");
                        return true;
                    }

                    break;
                
                case "laneEnter2":
                    if (this.StreetIndex[1] > 0 && this.Street.LaneEnter2[this.StreetIndex[1] - 1] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[--this.StreetIndex[1]] = this;
                        Console.WriteLine("moved north in le2");
                        return true;
                    }

                    break;

                case "laneExit":
                    if (this.StreetIndex[1] > 0 && this.Street.LaneExit[this.StreetIndex[1] - 1] == null)
                    {
                        this.Street.LaneExit[this.StreetIndex[1]] = null;
                        this.Street.LaneExit[--this.StreetIndex[1]] = this;
                        Console.WriteLine("moved north in lex");
                        return true;
                    }
                    // if we're at position 0 on laneExit and trying to move north,
                    // we can move into StreetN
                    else if (this.Turn == Direction.North && this.StreetIndex[1] == 0 &&
                        this.Crossing.StreetN.LaneExit[2] == null)
                    {
                        this.Direction = Direction.North;
                        this.Street.LaneExit[0] = null;
                        this.Street = this.Crossing.StreetN;
                        this.StreetIndex[1] = 2;
                        this.Street.LaneExit[this.StreetIndex[1]] = this;
                        Console.WriteLine("moved street north in le2");
                        return true;
                    }

                    break;
            }

            return false;
        }

        private bool moveEastInIntersection(String lane)
        {
            Console.WriteLine("TRY MOVE EAST");
            switch (lane)
            {
                case "laneEnter1":
                    if (this.Crossing.GetType() == typeof(Crossing_2) &&
                        this.Street.LaneExit[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneExit[this.StreetIndex[1]] = this;
                        Console.WriteLine("moved east in le1");
                        return true;
                    }
                    else if (this.Crossing.GetType() == typeof(Crossing_1) &&
                        this.Street.LaneEnter2[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter2[this.StreetIndex[1]] = this;
                        Console.WriteLine("moved east in le1");
                        return true;
                    }

                    break;

                case "laneEnter2":
                    if (this.Street.LaneExit[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneExit[this.StreetIndex[1]] = this;
                        Console.WriteLine("moved east in le2");
                        return true;
                    }

                    break;

                // if we're trying to move East on laneExit, and we're at poisiton 2,
                // we can move to StreetE
                case "laneExit":
                    if (this.Turn == Direction.East)
                    {
                        if (this.StreetIndex[1] < 2)
                            return this.moveSouthInIntersection(lane);
                        else if (this.StreetIndex[1] == 2 &&
                            this.Crossing.StreetE.LaneExit[2] == null)
                        {
                            this.Direction = Direction.East;
                            this.Street.LaneExit[2] = null;
                            this.Street = this.Crossing.StreetE;
                            this.StreetIndex[1] = 2;
                            this.Street.LaneExit[this.StreetIndex[1]] = this;
                            Console.WriteLine("moved street east in lex");
                            return true;
                        }
                    }

                    break;
            }

            return false;
        }

        private bool moveSouthInIntersection(String lane)
        {
            switch (lane)
            {
                case "laneEnter1":
                    if (this.StreetIndex[1] < 2 && this.Street.LaneEnter1[this.StreetIndex[1] + 1] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[++this.StreetIndex[1]] = this;
                        return true;
                    }
                    else if (this.StreetIndex[1] == 2 && this.Turn == Direction.South &&
                        this.Crossing.StreetS.LaneExit[2] == null)
                    {
                        this.Direction = Direction.South;
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street = this.Crossing.StreetS;
                        this.StreetIndex[1] = 2;
                        this.Street.LaneExit[this.StreetIndex[1]] = this;
                        return true;
                    }

                    break;

                case "laneEnter2":
                    if (this.StreetIndex[1] < 2 && this.Street.LaneEnter2[this.StreetIndex[1] + 1] == null)
                    {
                        this.Street.LaneEnter1[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[++this.StreetIndex[1]] = this;
                        return true;
                    }

                    break;

                case "laneExit":
                    if (this.StreetIndex[1] < 2 && this.Street.LaneExit[this.StreetIndex[1] + 1] == null)
                    {
                        this.Street.LaneExit[this.StreetIndex[1]] = null;
                        this.Street.LaneExit[++this.StreetIndex[1]] = this;
                        return true;
                    }

                    break;
            }

            return false;
        }

        private bool moveWestInIntersection(String lane)
        {
            switch (lane)
            {
                case "laneEnter1":
                    if (this.Turn == Direction.West)
                    {
                        if (this.StreetIndex[1] > 0)
                            return this.moveNorthInIntersection(lane);
                        else if (this.StreetIndex[1] == 0 &&
                            this.Crossing.StreetW.LaneEnter1[0] == null)
                        {
                            this.Direction = Direction.West;
                            this.Street.LaneEnter1[0] = null;
                            this.Street = this.Crossing.StreetW;
                            this.StreetIndex[1] = 2;
                            this.Street.LaneExit[this.StreetIndex[1]] = this;
                            return true;
                        }

                    }

                    break;

                case "laneEnter2":
                    if (this.Street.LaneEnter1[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneEnter2[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[this.StreetIndex[1]] = this;
                        return true;
                    }

                    break;

                case "laneExit":
                    if (this.Crossing.GetType() == typeof(Crossing_1) &&
                        this.Street.LaneEnter2[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneExit[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter2[this.StreetIndex[1]] = this;
                        return true;
                    }
                    else if (this.Crossing.GetType() == typeof(Crossing_2) &&
                        this.Street.LaneEnter1[this.StreetIndex[1]] == null)
                    {
                        this.Street.LaneExit[this.StreetIndex[1]] = null;
                        this.Street.LaneEnter1[this.StreetIndex[1]] = this;
                    }

                    break;
            }

            return false;
        }

        private bool moveInIntersection()
        {
            // if car is at the intersection
            if (Street.Position == Direction.Center)
            {
                // first find which street the car is on
                String currentLane = "";

                if (this.Street.LaneEnter1[this.StreetIndex[1]] == this)
                    currentLane = "laneEnter1";
                else if (this.Street.LaneEnter2[this.StreetIndex[1]] == this)
                    currentLane = "laneEnter2";
                else if (this.Street.LaneExit[this.StreetIndex[1]] == this)
                    currentLane = "laneExit";

                // move car based on where it wants to go
                // both possibilites will allow the car to get closer to where it wants to go
                switch (this.Turn)
                {
                    case Direction.North:
                        return this.moveNorthInIntersection(currentLane) ||
                            this.moveEastInIntersection(currentLane);

                    case Direction.East:
                        return this.moveEastInIntersection(currentLane) ||
                            this.moveSouthInIntersection(currentLane);

                    case Direction.South:
                        return this.moveSouthInIntersection(currentLane) ||
                            this.moveWestInIntersection(currentLane);

                    case Direction.West:
                        return this.moveWestInIntersection(currentLane) ||
                            this.moveNorthInIntersection(currentLane);
                }
            }

            return false;
        }

        // will move car to another crossing,
        // or will let it exit the grid
        private bool moveToAnotherCrossing()
        {
            // check if Car is exiting grid. If not, get next crossing's ID
            String newCrossingID = "";

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
                        this.exitGrid();
                        return false;
                    }

                    newCrossingID = this.Crossing.ID[0] + Convert.ToString(this.Crossing.ID[1] - 1);

                    break;
            }

            this.Crossing = Grid.getCrossing(newCrossingID);

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

            return true;
        }
    }
}
