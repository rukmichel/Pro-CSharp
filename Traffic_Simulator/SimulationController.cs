using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Traffic_Simulator
{
    /// <summary>
    /// It's the main controller in the MVC model.
    /// Controlls the interaction beetween the GUI, disk and functionalities.
    /// </summary>
    class SimulationController
    {   
        /// <summary>
        /// Amount of time beetween time ticks.
        /// </summary>
        private int _refreshRate = 100; 

        /// <summary>
        /// Object used to trigger an event every x miliseconds.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Event triggered by timer.
        /// </summary>
        private Action _onTimeTick;

        /// <summary>
        /// Current state of the simulation.
        /// </summary>
        private State _state;

        /// <summary>
        /// User interface used by the simulator.
        /// </summary>
        private GUI _gui;

        /// <summary>
        /// Object that represents the space where the crossings are located.
        /// </summary>
        private Grid _grid = new Grid();

        /// <summary>
        /// Object that handles all disk operations.
        /// </summary>
        private FileHandler _fileHandler = new FileHandler();
        

        /// <summary>
        /// Checks if a position on the grid is available for adding a crossing.
        /// </summary>
        /// <param name="id">indicates a position on the grid, for instance "A3".</param>
        /// <returns>true if available, false if not.</returns>
        public bool gridIsAvailable(string id) { return true; }

        /// <summary>
        /// Removes a crossing on the position "id".
        /// </summary>
        /// <param name="id"> Indicates a position on the grid, for instance "A3". </param> 
        /// <returns>True if successfull</returns>
        public bool removeCrossing(string id) { return true; }

        /// <summary>
        /// Selects a crossing and displays its information.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        public void selectCrossing(string id){}

        /// <summary>
        /// Adds a crossing on a givven position on the grid.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        /// <param name="crossing">The class type of the crossing, either "crossing_1" or "crossing_2".</param>
        /// <returns>Returns true if successfull.</returns>
        public bool addCrossing(string id, Type crossing) { return true; }

        /// <summary>
        /// Sets a value of a crossing's atribute.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        /// <param name="sender">GUI object that holds the value.</param>
        /// <returns>Returns true if successfull.</returns>
        public bool setCrossingProperty(string id, object sender) { return true; }

        /// <summary>
        /// Removes all crossings from the grid.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool clearGrid() { return true; }

        /// <summary>
        /// Starts simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool startSimulation() { return true; }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool stopSimulation() { return true; }

        /// <summary>
        /// Pauses the simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool pauseSimulation() { return true; }

        /// <summary>
        /// Saves the simulation to file.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool save() { return true; }

        /// <summary>
        /// Saves the simulation to given filename on given path.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool saveAs() { return true; }

        /// <summary>
        /// Loads a simulation file to the memory.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool load() { return true; }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public bool close() { return true; }

        /// <summary>
        /// Shows statistics such as average car speed, wait time, etc.
        /// </summary>
        public void showStatistics() { }
    }
}
