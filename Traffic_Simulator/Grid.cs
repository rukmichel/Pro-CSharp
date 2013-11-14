using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    class Grid
    {
        /// <summary>
        /// A list of all cars on screen.
        /// </summary>
        List<Car> _listOfCars;

        /// <summary>
        /// All available slots for Crossings on the grid.
        /// </summary>
        private Crossing[,] _slots;

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
        /// </summary>
        public void timeTick() { }

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
        public bool reset() { return false; }
    }
}