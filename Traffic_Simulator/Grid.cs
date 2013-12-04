using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Traffic_Simulator
{
    [Serializable()]
    public class Grid
    {
        /// <summary>
        /// A list of all cars on screen.
        /// </summary>
        List<Car> _listOfCars;

        public List<Car> ListOfCars
        {
            get { return _listOfCars; }
        }

        /// <summary>
        /// All available slots for Crossings on the grid.
        /// </summary>
        private Crossing[,] _slots;

        public Crossing[,] Slots
        {
            get { return _slots; }
        }

        /// <summary>
        /// The peak number of cars present on the grid at one time.
        /// </summary>
        private int _peakNumberOfCars;

        /// <summary>
        /// Number of cars currently visible on the grid.
        /// </summary>
        private int _currentNumberOfCarsInGrid;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Grid() { _slots = new Crossing[4, 3];}

        /// <summary>
        /// Keeps track of time, moves cars and changes lights
        /// 1- moves every existing car by 1 position
        /// 2-'ticks' all crossings and add new cars to crossings if necessary
        /// </summary>
        public void timeTick() 
        {
            foreach (Car c in _listOfCars) //moves every existing car by 1 position
                if(c.HasEnteredGrid && !c.HasExitedGrid)
                    c.move();

            foreach (Crossing c in _slots) //'ticks' all crossings and add new cars to crossings
            {
                c.timeTick(); //ticks the crossings clock
                Car car = new Car();

                ////////////add new cars to street E
                if (c.FlowE < c.EnteredE &&                                                 //if there are still cars to enter a crossing and
                    c.StreetE.LaneEnter1[0] == null   && c.StreetE.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
                {
                    int prob = new Random().Next() % (int) (c.ProbEtoS + c.ProbEtoN + c.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities
                    
                   // Interpreting the generated number:
                   //
                   //          _       (portion 1)     _  (portion 2)  _        (portion 3)        _
                   //          |      direction = N    | direction = S |       direction = W       |
                   //          |_______________________|_______________|___________________________|
                   //number:   0                    probN        probN + probS        probN+probS+probW = 100
                   //Example:  0                       40         40 + 15 = 55              40 + 15 + 45 = 100

                    if (prob < c.ProbEtoN)// portion 1
                        car.Direction = Direction.North;

                    if (prob >= c.ProbEtoN && prob<(c.ProbEtoN + c.ProbEtoS)) //portion 2
                        car.Direction = Direction.South;

                    if(prob >=(c.ProbEtoN + c.ProbEtoS)) //portion 3
                        car.Direction = Direction.West;
                }

                ////////////add new cars to street W
                if (c.FlowW < c.EnteredW &&                                                 //if there are still cars to enter a crossing and
                    c.StreetW.LaneEnter1[0] == null && c.StreetW.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
                {
                    int prob = new Random().Next() % (int)(c.ProbWtoS + c.ProbWtoN + c.ProbWtoE); //generates a random number beetween 0 and  the sum of probabilities

                    if (prob < c.ProbWtoN)
                        car.Direction = Direction.North;
                    if (prob >= c.ProbWtoN && prob < (c.ProbWtoN + c.ProbWtoS))
                        car.Direction = Direction.South;
                    if (prob >= (c.ProbWtoN + c.ProbWtoS))
                        car.Direction = Direction.East;
                }

                ////////add new cars to street S
                if (c.FlowS < c.EnteredS &&                                                   //if there are still cars to enter a crossing and
                    c.StreetS.LaneEnter1[0] == null                                           //there is an available slot at the entrance of lane enter 1
                    && (c.GetType() == typeof(Crossing_2) || c.StreetS.LaneEnter2[0] == null))//and, in case its a crossing 1, there is an available slot at the entrance of lane 2          
                {
                    int prob = new Random().Next() % (int)(c.ProbStoE + c.ProbStoN + c.ProbStoW); //generates a random number beetween 0 and  the sum of probabilities
                    
                    if (prob < c.ProbStoN)
                        car.Direction = Direction.North;
                    if (prob >= c.ProbStoN && prob < (c.ProbStoN + c.ProbStoW))
                        car.Direction = Direction.West;
                    if (prob >= (c.ProbStoN + c.ProbStoW))
                        car.Direction = Direction.East;
                }

                ////////add new cars to street N
                if (c.FlowN < c.EnteredN &&                                                   //if there are still cars to enter a crossing
                    c.StreetN.LaneEnter1[0] == null                                           //and if there is an available slot at the entrance of lane enter 1
                    && (c.GetType() == typeof(Crossing_2) || c.StreetN.LaneEnter2[0] == null))//and, unless it's a crossing_2, if there is an available slot at the entrance of lane 2          
                {
                    int prob = new Random().Next() % (int)(c.ProbEtoS + c.ProbEtoN + c.ProbEtoW); //generates a random number beetween 0 and  the sum of probabilities
                    
                    if (prob < c.ProbStoN)
                        car.Direction = Direction.North;
                    if (prob >= (c.ProbStoN) && prob < (c.ProbStoN + c.ProbStoE))
                        car.Direction = Direction.East;
                    if (prob >= (c.ProbStoN + c.ProbStoE))
                        car.Direction = Direction.West;
                }

            }//finish ticking crossings and adding new cars
        }

        /// <summary>
        /// Calculate the average speed of cars inside the grid
        /// </summary>
        /// <returns></returns>
        public double calculateAverageSpeed() { return 0.0d; }

        /// <summary>
        /// Calculate average wait time at crossings
        /// </summary>
        /// <returns></returns>
        public double calculateAverageWaitTime() { return 0.0d; }

        /// <summary>
        /// Add a new car to the grid
        /// </summary>
        private void addCar() { }

        /// <summary>
        /// Remove a car from the grid
        /// </summary>
        private void removeCar() { }

        /// <summary>
        /// Add a new crossing
        /// </summary>
        /// <param name="id">Grid position where crossing should be added</param>
        /// <param name="crossing">The type of crossing to add</param>
        /// <returns>If successful</returns>
        public bool addCrossing(string id, Type crossing) { return false; }

        /// <summary>
        /// Remove a crossing
        /// </summary>
        /// <param name="id">Grid position of crossing to remove</param>
        /// <returns>If successful</returns>
        public bool removeCrossing(string id) { return false; }

        /// <summary>
        /// Returns the crossing present at a certain location.
        /// If no crossing is present, returns `null`
        /// </summary>
        /// <param name="id">Grid position of crossing</param>
        /// <returns>The crossing</returns>
        public Crossing getCrossing(string id) { return new Crossing_1(); }

        /// <summary>
        /// Clears all cars and lights
        /// </summary>
        /// <returns>If successful</returns>
        public bool reset() 
        {
            _listOfCars.Clear();
            foreach (Crossing c in _slots)
            {
                c.reset();
            }
            return false; 
        
        }
    }
}