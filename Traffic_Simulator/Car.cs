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

        private List<int[]> _path = new List<int[]>();
        private int nextSlot = -1;

        public Car(Crossing c) 
        {
            _crossing = c;
        }

        /// <summary>
        /// Moves the car to the next position.
        /// </summary>
        /// <returns>returns true if the car moves</returns>
        public bool move() 
        {

            Street[] streets = new Street[5];
            Crossing cr = this.Crossing;
            streets[0] = this.Crossing.StreetN;
            streets[1] = this.Crossing.StreetE;
            streets[2] = this.Crossing.StreetS;
            streets[3] = this.Crossing.StreetW;
            streets[4] = this.Crossing.Intersection;

            
            nextSlot++;//increments next slot
            if (nextSlot > _path.Count-1)//if car is entering a crossin or leaving the grid
            {
                if (HasEnteredGrid)//gets new crossing
                {
                    _crossing = getNextCrossing();
                    Street.Lanes[StreetIndex[0]][StreetIndex[1]] = null;//leaves previous position
                }

                if (this.Crossing == null)//if car is leaving the grid
                {
                    this.HasExitedGrid = true;
                    this.Street.Lanes[this.StreetIndex[0]][this.StreetIndex[1]] = null; //clears the cars last position
                    this.StreetIndex[0] = -1;
                    this.StreetIndex[1] = -1;
                    _path = null;
                    nextSlot = 0;
                    Street = null;
                    Console.WriteLine("car is leaving the grid");
                    return true;
                }
                else//if car is entering a new crossing
                {

                    Console.WriteLine("car will enter a new crossing");

                    _path.Clear();
                    nextSlot = 0;


                    if (this.Street.Position == this.Turn && this.HasEnteredGrid)
                    {
                        Console.WriteLine("car has entered a new crossing");
                        streets[0] = this.Crossing.StreetN;
                        streets[1] = this.Crossing.StreetE;
                        streets[2] = this.Crossing.StreetS;
                        streets[3] = this.Crossing.StreetW;
                        streets[4] = this.Crossing.Intersection;

                        cr = this.Crossing;
                        this.Street = streets[((int)this.Turn + 2) % 4];
                    }

                    calculateTurn();
                    calculateEntrance(streets);
                    calculateIntersection(streets);
                    calculateExit(streets);

                }
            }//end if(car is entering a new crosisng/leaving grid)


            int str = _path[nextSlot][0], lane = _path[nextSlot][1], pos = _path[nextSlot][2];

            //check traffic lights

            if (streets[str].Lanes[lane][pos] == null  && (nextSlot != 3 || checkLights()))//if next position is available and light is green
            {

                if (HasEnteredGrid == false)
                    this.HasEnteredGrid = true;
                else
                    Street.Lanes[StreetIndex[0]][StreetIndex[1]] = null;//leaves previous position
                
                streets[str].Lanes[lane][pos] = this;//enters new position                

                StreetIndex[0] = lane;
                StreetIndex[1] = pos;

                //updates the street
                if (nextSlot == 3)
                {
                    this.Street = this.Crossing.Intersection;
                }
                if ((nextSlot == 4 && _path.Count == 7) || (nextSlot == 6 && _path.Count == 9))//if car left intersection zone
                {
                    this.Street = streets[(int)this.Turn];
                    this.Direction = this.Turn;
                }


                Console.WriteLine("car moved");
            }
            else//if car didnt move
            {
                Crossing_1 c1;
                nextSlot--;
                if (Crossing.GetType() == typeof(Crossing_1))
                {
                    c1 = (Crossing_1)Crossing;
                    if (c1.LightNtoWS._color == Color.Green)
                    {
                        SimulationController._timer.Stop();
                        int i = this.StreetIndex[0];
                        SimulationController._timer.Start();
                    }
                }
                Console.WriteLine("car did not move");                
                return false;
            }          
            
            return false;  
        }

        private bool checkLights()
        {
            bool lightIsGreen = false;
            if (nextSlot == 3)
            {

                if (
                    (this.Street.Position == Direction.West && (this.Turn == Direction.East || this.Turn == Direction.South) && this.Crossing.LightWtoSE._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.West && (this.Turn == Direction.North) && this.Crossing.LightWtoN._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.East && (this.Turn == Direction.North || this.Turn == Direction.West) && this.Crossing.LightEtoNW._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.East && (this.Turn == Direction.South) && this.Crossing.LightEtoS._color == Color.Green)
                    )
                    lightIsGreen = true;

                if (this.Crossing.GetType() == typeof(Crossing_1))
                {
                    Crossing_1 c1 = (Crossing_1)this.Crossing;
                    if (
                    (this.Street.Position == Direction.North && (this.Turn == Direction.West || this.Turn == Direction.South) && c1.LightNtoWS._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.North && (this.Turn == Direction.East) && c1.LightNtoE._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.South && (this.Turn == Direction.North || this.Turn == Direction.East) && c1.LightStoEN._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.South && (this.Turn == Direction.West) && c1.LightStoW._color == Color.Green)
                    )
                        lightIsGreen = true;
                }
                if (this.Crossing.GetType() == typeof(Crossing_2))
                {
                    Crossing_2 c1 = (Crossing_2)this.Crossing;
                    if (
                    (this.Street.Position == Direction.North && c1.LightNtoS._color == Color.Green)
                    ||
                    (this.Street.Position == Direction.South && c1.LightStoN._color == Color.Green)
                    )
                        lightIsGreen = true;
                }
            }
            return lightIsGreen;
        }

        private void calculateEntrance(Street[] streets) 
        {                                                       //Dir: N=0, E=1, S=2, W=3

            _path.Add(new int[3] { (int)this.Street.Position, Convert.ToInt32(((int)this.Street.Position + 1) % 4 == (int)this.Turn), 0 });//pos Dir,0,0
            _path.Add(new int[3] { (int)this.Street.Position, Convert.ToInt32(((int)this.Street.Position + 1) % 4 == (int)this.Turn), 1 });
            _path.Add(new int[3] { (int)this.Street.Position, Convert.ToInt32(((int)this.Street.Position + 1) % 4 == (int)this.Turn), 2 });   
        }

        private void calculateIntersection(Street[] streets)
        {
            switch (this.Street.Position)
            { 
                case Direction.North:                    
                    if (this.Turn == Direction.East)
                    {
                        _path.Add(new int[3] { 4, 1, 0 });
                        _path.Add(new int[3] { 4, 2, 1 });
                        _path.Add(new int[3] { 4, 2, 2 });
                    }
                    if (this.Turn == Direction.South)
                    {
                        _path.Add(new int[3] { 4, 0, 0 });
                        _path.Add(new int[3] { 4, 0, 1 });
                        _path.Add(new int[3] { 4, 0, 2 });
                    }
                    if (this.Turn == Direction.West)
                    {
                        _path.Add(new int[3] { 4, 0, 0 });
                    }
                    return;
                
                case Direction.East:
                    if (this.Turn == Direction.West)
                    {
                        _path.Add(new int[3] { 4, 2, 0 });
                        _path.Add(new int[3] { 4, 1, 0 });
                        _path.Add(new int[3] { 4, 0, 0 });
                    }
                    if (this.Turn == Direction.South)
                    {
                        _path.Add(new int[3] { 4, 2, 1 });
                        _path.Add(new int[3] { 4, 1, 2 });
                        _path.Add(new int[3] { 4, 0, 2 });
                    }
                    if (this.Turn == Direction.North)
                    {
                        _path.Add(new int[3] { 4, 2, 0 });
                    }
                    return;

                case Direction.South:

                    if (this.Turn == Direction.North)
                    {
                        _path.Add(new int[3] { 4, 2, 2 });
                        _path.Add(new int[3] { 4, 2, 1 });
                        _path.Add(new int[3] { 4, 2, 0 });
                    }
                    if (this.Turn == Direction.West)
                    {
                        _path.Add(new int[3] { 4, 1, 2 });
                        _path.Add(new int[3] { 4, 0, 1 });
                        _path.Add(new int[3] { 4, 0, 0 });
                    }
                    if (this.Turn == Direction.East)
                    {
                        _path.Add(new int[3] { 4, 2, 2 });
                    }
                    return;

                case Direction.West:
                    if (this.Turn == Direction.East)
                    {
                        _path.Add(new int[3] { 4, 0, 2 });
                        _path.Add(new int[3] { 4, 1, 2 });
                        _path.Add(new int[3] { 4, 2, 2 });
                    }
                    if (this.Turn == Direction.North)
                    {
                        _path.Add(new int[3] { 4, 0, 1 });
                        _path.Add(new int[3] { 4, 1, 0 });
                        _path.Add(new int[3] { 4, 2, 0 });
                    }
                    if (this.Turn == Direction.South)
                    {
                        _path.Add(new int[3] { 4, 0, 2 });
                    }
                    return;
            }
        }

        private void calculateExit(Street[] streets)
        {

            _path.Add(new int[3] { (int)this.Turn, 2, 2 });
            _path.Add(new int[3] { (int)this.Turn, 2, 1 });
            _path.Add(new int[3] { (int)this.Turn, 2, 0 });

        }

        private Crossing getNextCrossing()
        {
              //check first position in the next crossing
            char[] id = new char[2];
                if (this.Direction == Direction.North)
                {
                    id[0] = this.Crossing.ID[0];
                    id[1] = (char)((int)this.Crossing.ID[1] - 1);
                }
                if (this.Direction == Direction.East)
                {
                    id[0] = (char)((int)this.Crossing.ID[0] + 1);
                    id[1] = this.Crossing.ID[1];
                }
                if (this.Direction == Direction.South)
                {
                    id[0] = this.Crossing.ID[0];
                    id[1] = (char)((int)this.Crossing.ID[1] + 1);
                }
                if (this.Direction == Direction.West)
                {
                    id[0] = (char)((int)this.Crossing.ID[0] - 1);
                    id[1] = this.Crossing.ID[1];
                }

                //checks wether it left the grid
                if (id[0] > 'D' || id[0] < 'A' || id[1] > '3' || id[1] < '0')//if id is out of bounds
                {
                    return null;
                }
                else 
                { 
                    return  Grid.getCrossing(new string(id));
                }
        }
        
        /// <summary>
        /// Use probablity to figure out which direction the car should take
        /// </summary>
        /// <returns>Direction the car will turn at intersection</returns>
        private void calculateTurn()
        {

            if ((this.Street.Position == Direction.North || this.Street.Position == Direction.South) && this.Crossing.GetType() == typeof(Crossing_2))
            {
                this.Turn = this.Direction;
            }
            else
            {

                if (this.Street.Position == Direction.East)
                {
                    int prob = (int)(this.Crossing.ProbEtoS + this.Crossing.ProbEtoN + this.Crossing.ProbEtoW);

                    if (prob==0)//if all probabilities are set to 0, car doesnt turn
                    {
                        this.Turn=this.Direction;
                        return;
                    }

                    prob = new Random().Next() % prob; //generates a random number beetween 0 and  the sum of probabilities

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

                if (this.Street.Position == Direction.West)
                {

                    int prob = (int)(this.Crossing.ProbWtoS + this.Crossing.ProbWtoN + this.Crossing.ProbWtoE); //generates a random number beetween 0 and  the sum of probabilities

                    if (prob == 0)//if all probabilities are set to 0, car doesnt turn
                    {
                        this.Turn = this.Direction;
                        return;
                    }

                    prob = new Random().Next() % prob; //generates a random number beetween 0 and  the sum of probabilities

                    if (prob < this.Crossing.ProbWtoN)
                        this.Turn = Direction.North;
                    if (prob >= this.Crossing.ProbWtoN && prob < (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                        this.Turn = Direction.South;
                    if (prob >= (this.Crossing.ProbWtoN + this.Crossing.ProbWtoS))
                        this.Turn = Direction.East;
                }
                
                if (this.Street.Position == Direction.South)
                {
                    int prob =(int)(this.Crossing.ProbStoE + this.Crossing.ProbStoN + this.Crossing.ProbStoW); //generates a random number beetween 0 and  the sum of probabilities

                    if (prob == 0)//if all probabilities are set to 0, car doesnt turn
                    {
                        this.Turn = this.Direction;
                        return;
                    }

                    prob = new Random().Next() % prob; //generates a random number beetween 0 and  the sum of probabilitie

                    if (prob < this.Crossing.ProbStoN)
                        this.Turn = Direction.North;
                    if (prob >= this.Crossing.ProbStoN && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                        this.Turn = Direction.West;
                    if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoW))
                        this.Turn = Direction.East;
                }

                if (this.Street.Position == Direction.North)
                {
                    int prob = (int)(this.Crossing.ProbEtoS + this.Crossing.ProbEtoN + this.Crossing.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities

                    if (prob == 0)//if all probabilities are set to 0, car doesnt turn
                    {
                        this.Turn = this.Direction;
                        return;
                    }

                    prob = new Random().Next() % prob; //generates a random number beetween 0 and  the sum of probabilitie

                    if (prob < this.Crossing.ProbStoN)
                        this.Turn = Direction.North;
                    if (prob >= (this.Crossing.ProbStoN) && prob < (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                        this.Turn = Direction.East;
                    if (prob >= (this.Crossing.ProbStoN + this.Crossing.ProbStoE))
                        this.Turn = Direction.West;
                }
            }
        }

        
    }
}
