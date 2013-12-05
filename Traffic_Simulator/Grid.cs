using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Traffic_Simulator
{
    [Serializable()]
    public class Grid
    {
        /// <summary>
        /// A list of all cars on screen.
        /// </summary>
        List<Car> _listOfCars = new List<Car>();

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
        /// 2-'ticks' all crossings 
        /// 3- adds new cars to crossings if necessary
        /// </summary>
        public void timeTick() 
        {
            foreach (Car c in _listOfCars) //moves every existing car by 1 position
                if(c!=null)
            //        if(c.HasEnteredGrid && !c.HasExitedGrid)
                        c.move();

            foreach (Crossing c in _slots) //'ticks' all crossings and add new cars to crossings
            {
                if (c != null)
                {
                    c.timeTick(); //ticks the crossings clock
                    addCars(c);  //adds cars (if necessary) to the crossings
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
        /// Add a new cars the given crossing
        /// </summary>
        private void addCars(Crossing c) 
        {

            ////////////add new cars to street E
            if (c.FlowE > c.EnteredE &&                                                 //if there are still cars to enter a crossing and
                c.StreetE.LaneEnter1[0] == null && c.StreetE.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
            {
                Car car = new Car();

                car.Street = c.StreetE;
                car.Crossing = c;
                car.Direction = Direction.West;
                car.StreetIndex[0] = 0;
                car.StreetIndex[1] = 0;
                car.HasEnteredGrid = false;
                car.HasExitedGrid = false;

                c.EnteredE++;
                _listOfCars.Add(car);
                //car.move();
            }

            ////////////add new cars to street W
            if (c.FlowW > c.EnteredW &&                                                 //if there are still cars to enter a crossing and
                c.StreetW.LaneEnter1[0] == null && c.StreetW.LaneEnter2[0] == null)   //there is an available slot at the entrance of the street
            {

                Car car = new Car();

                car.Street = c.StreetW;
                car.Crossing = c;
                car.Direction = Direction.East;
                car.StreetIndex[0] = 0;
                car.StreetIndex[1] = 0;
                car.HasEnteredGrid = false;
                car.HasExitedGrid = false;

                c.EnteredW++;
                _listOfCars.Add(car);
                //car.move();
            }

            ////////add new cars to street S
            if (c.FlowS > c.EnteredS &&                                                   //if there are still cars to enter a crossing and
                c.StreetS.LaneEnter1[0] == null                                           //there is an available slot at the entrance of lane enter 1
                && (c.GetType() == typeof(Crossing_2) || c.StreetS.LaneEnter2[0] == null))//and, in case its a crossing 1, there is an available slot at the entrance of lane 2          
            {

                Car car = new Car();

                car.Street = c.StreetS;
                car.Crossing = c;
                car.Direction = Direction.North;
                car.StreetIndex[0] = 0;
                car.StreetIndex[1] = 0;
                car.HasEnteredGrid = false;
                car.HasExitedGrid = false;

                c.EnteredS++;
                _listOfCars.Add(car);
                //car.move();
            }

            ////////add new cars to street N
            if (c.FlowN > c.EnteredN &&                                                   //if there are still cars to enter a crossing
                c.StreetN.LaneEnter1[0] == null                                           //and if there is an available slot at the entrance of lane enter 1
                && (c.GetType() == typeof(Crossing_2) || c.StreetN.LaneEnter2[0] == null))//and, unless it's a crossing_2, if there is an available slot at the entrance of lane 2          
            {
                Car car = new Car();

                car.Street = c.StreetN;
                car.Crossing = c;
                car.Direction = Direction.South;
                car.StreetIndex[0] = 0;
                car.StreetIndex[1] = 0;
                car.HasEnteredGrid = false;
                car.HasExitedGrid = false;

                c.EnteredN++;
                _listOfCars.Add(car);
                //car.move();
            }
        }

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
        public static Crossing getCrossing(string id) { return new Crossing_1(); }

        /// <summary>
        /// Clears all cars and lights
        /// </summary>
        /// <returns>If successful</returns>
        public bool reset() 
        {
            _listOfCars.Clear();//deletes all cars
            foreach (Crossing c in _slots)//resets all crossings
            {
                if(c!=null)
                    c.reset();
            }
            return false;         
        }
    }
}