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
        public GUI Gui
        {
            set { _gui = value; }
        }

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
        public string removeCrossing(string id) { return ""; }

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
        public string addCrossing(string id, Type crossing) { return ""; }

        /// <summary>
        /// Sets a value of a crossing's atribute.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        /// <param name="sender">GUI object that holds the value.</param>
        /// <returns>Returns true if successfull.</returns>
        public string setCrossingProperty(string id, object sender) { return ""; }

        /// <summary>
        /// Removes all crossings from the grid.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string clearGrid() { return ""; }

        /// <summary>
        /// Starts simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string startSimulation() { return ""; }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string stopSimulation() { return ""; }

        /// <summary>
        /// Pauses the simulation.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string pauseSimulation() { return ""; }

        /// <summary>
        /// Saves the simulation to file.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string save() { return ""; }

        /// <summary>
        /// Saves the simulation to given filename on given path.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string saveAs() { return ""; }

        /// <summary>
        /// Loads a simulation file to the memory.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string load() { return ""; }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public string close() {
            //_fileHandler.setUnsavedData(); //test this method
            if (_fileHandler.hasUnsavedData()) 
            {
                string messageResult = _gui.saveMessage("Traffic Simulator", "Save modifications?"); //opens save message dialog
                if (messageResult == "Yes") //user wants to save changes
                {
                    string path = _gui.filePath(1);
                    if (path != "")//gets file path browsed by user
                    {
                        _fileHandler.Path = path;
                        _fileHandler.saveToFile(_grid);
                        return "Program is closing...";
                    }
                    else //in case user canceled saving
                    {
                        return "";
                    }
                }
                if (messageResult == "No") //user doesnt want to save changes
                {
                    return "Program is closing...";
                }
                if (messageResult == "Cancel") //user cancels closing program
                {
                    return "";
                }
            }
            return "Program is closing..."; 
        }

        /// <summary>
        /// Shows statistics such as average car speed, wait time, etc.
        /// </summary>
        public void showStatistics() { }
    }
}
