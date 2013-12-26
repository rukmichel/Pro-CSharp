using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms.ComponentModel;

namespace Traffic_Simulator
{
    delegate void Del(Grid g);
    delegate void Del2(object sender, EventArgs e);

    /// <summary>
    /// It's the main controller in the MVC model.
    /// Controlls the interaction beetween the GUI, disk and functionalities.
    /// </summary>
    public class SimulationController
    {
        /// <summary>
        /// Amount of time beetween time ticks in milliseconds.
        /// </summary>
        private int _refreshRate = 600;

        /// <summary>
        /// Object used to trigger an event every x miliseconds.
        /// </summary>
        private Timer _timer = new Timer();

        /// <summary>
        /// Current state of the simulation.
        /// </summary>
        private State _state = State.Stopped;

        public State State
        {
            get
            {
                return _state;
            }

        }

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
        public bool slotIsAvailable(string id) 
        { 
            return (Grid.getCrossing(id)==null)&&(State==State.Stopped); 
        }

        /// <summary>
        /// Removes a crossing on the position "id".
        /// </summary>
        /// <param name="id"> Indicates a position on the grid, for instance "A3". </param> 
        /// <returns>Error message, or null if successfull.</returns>
        public string removeCrossing(string id)
        {
            if (_grid.removeCrossing(id))
            {
                _fileHandler.setUnsavedData();
                return "ok";    
            }
           
            return "";
        }

        /// <summary>
        /// Selects a crossing and displays its information.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        public void selectCrossing(string id)
        {
            Crossing cr = Grid.getCrossing(id);
            _gui.displayCrossingSettings(ObjectCopier.Clone<Crossing>(cr));
        }

        /// <summary>
        /// Adds a crossing on a givven position on the grid.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        /// <param name="crossing">The class type of the crossing, either "crossing_1" or "crossing_2".</param>
        /// <returns>Error message, or null if successfull.</returns>
        public string addCrossing(string id, Type crossing)
        {
            if (_grid == null)
                _grid = new Grid();
            if (State == State.Stopped)
            {
                if (_grid.addCrossing(id, crossing))
                {
                    _fileHandler.setUnsavedData();
                    return "";
                }
                return "Slot " + id + " is not available.";
            }
            else
            {
                return "Stop simulation before adding a crossing.";
            }

        }

        /// <summary>
        /// Sets a value of a crossing's atribute.
        /// </summary>
        /// <param name="id">Indicates a position on the grid, for instance "A3".</param>
        /// <param name="sender">GUI object that holds the value.</param>
        /// <returns>Error message, or null if successfull.</returns>
        public string setCrossingProperty(string id, int[] values)
        {
            Crossing c = Grid.getCrossing(id);
            if (c != null)
            {
                if (c.GetType() == typeof(Crossing_1))
                {
                    Crossing_1 c1 = (Crossing_1)c;

                    c1.LightNtoWS._greenLightTime = values[0];
                    c1.LightStoEN._greenLightTime = values[0];
                    c1.LightNtoE._greenLightTime = values[1];
                    c1.LightStoW._greenLightTime = values[1];
                }

                if (c.GetType() == typeof(Crossing_2))
                {
                    Crossing_2 c2 = (Crossing_2)c;

                    c2.LightPedestrian._greenLightTime = values[0];
                    c2.LightNtoS._greenLightTime = values[1];
                    c2.LightStoN._greenLightTime = values[1];
                }

                c.LightWtoN._greenLightTime = values[2];
                c.LightWtoSE._greenLightTime = values[3];
                c.FlowN = values[4];
                c.FlowE = values[5];
                c.FlowS = values[6];
                c.FlowW = values[7];
                c.ProbNtoE = values[8];
                c.ProbNtoS = values[9];
                c.ProbNtoW = values[10];
                c.ProbEtoN = values[11];
                c.ProbEtoS = values[12];
                c.ProbEtoW = values[13];
                c.ProbStoN = values[14];
                c.ProbStoE = values[15];
                c.ProbStoW = values[16];
                c.ProbWtoN = values[17];
                c.ProbWtoE = values[18];
                c.ProbWtoS = values[19];

            }

            _fileHandler.setUnsavedData();
            return "";
        }

        /// <summary>
        /// Removes all crossings from the grid.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string clearGrid()
        {
            if (_grid.clearGrid())
            {
                _fileHandler.setUnsavedData();
                return "ok";
            }
            return "";
        }

        /// <summary>
        /// Starts simulation.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string startSimulation()
        {
            try
            {
                if (_state == Traffic_Simulator.State.Stopped)
                {
                    bool thereAreCrossings=false;
                    foreach (Crossing c in _grid.Slots)
                    {
                        if (c != null)
                            thereAreCrossings = true;
                    }

                    if (thereAreCrossings)
                    {
                        _timer.Interval = _refreshRate;//sets and starts the timer
                        _timer.Elapsed += timerHasTriggered;
                        _timer.Start();
                    }
                    else
                    {
                        return "No crossings added";
                    }
                }
                if (_state == Traffic_Simulator.State.Paused)
                {
                    _timer.Start();
                }
                _state = State.Running;
                timerHasTriggered(null, null);
                return "";
            }
            catch
            {
                return "Couldn't start simulation";
            }
        }

        /// <summary>
        /// Stops the simulation.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string stopSimulation()
        {

            try
            {
                _timer.Stop();
                _timer.Elapsed -= timerHasTriggered;
                _grid.reset();
                Grid tempCopy = ObjectCopier.Clone<Grid>(_grid); //creates a temporary copy of the object _grid
                _gui.refreshScreen(tempCopy);                   //and sends that copy as a parameter to the GUI
                _state = State.Stopped;
                return "";
            }
            catch
            {
                return "Couldn't stop simulation";
            }
        }

        /// <summary>
        /// Pauses the simulation.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string pauseSimulation()
        {
            try
            {
                _timer.Stop();
                _state = State.Paused;
                return "";
            }
            catch
            {
                return "Couldn't pause simulation";
            }
        }

        /// <summary>
        /// Saves the simulation to file.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string save()
        {
            try
            {

                if (_fileHandler.Path == "")//if saved path is nulll
                {
                    return saveAs();
                }
                else //in case user canceled saving
                {
                    Grid copyOfGrid = ObjectCopier.Clone<Grid>(_grid);
                    _fileHandler.saveToFile(copyOfGrid);
                    return "Simulation has been successfully saved";
                }
            }
            catch
            {
                return "Simulation could not be saved";
            }

        }

        /// <summary>
        /// Saves the simulation to given filename on given path.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string saveAs()
        {            
            try
            {
                string s = _gui.filePath(1); //shows save-file window
                if (s == "")
                    return "";
                else
                {
                    _fileHandler.Path = s;
                    Grid copyOfGrid = ObjectCopier.Clone<Grid>(_grid);
                    _fileHandler.saveToFile(copyOfGrid);
                    return "Simulation has been successfully saved";
                }
            }
            catch
            {
                return "Simulation could not be saved";
            }

        }

        /// <summary>
        /// Loads a simulation file to the memory.
        /// </summary>
        /// <returns>Error message, or null if successfull.</returns>
        public string load()
        {

            try
            {
                string path = _gui.filePath(0); // shows load-file window
                if (path != "")
                {
                    _fileHandler.Path = path;
                    _grid = _fileHandler.loadFromFile();
                    return "Simulation has been loaded";
                }
                return "";
            }
            catch
            {
                return "Simulation not be loaded";
            }


        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <returns>Closing message or null if user cancels.</returns>
        public string close()
        {

            pauseSimulation();

            if (_fileHandler.hasUnsavedData())
            {
                string messageResult = _gui.saveMessage("Traffic Simulator", "Save modifications?"); //opens save message dialog

                if (messageResult == "Yes") //user wants to save changes
                {
                    return save();
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

        /// <summary>
        /// This method is called whenever the timer's event is fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timerHasTriggered(object sender, ElapsedEventArgs e)
        {

            if (this.State == State.Running)
                _grid.timeTick();

            if (this.State == State.Running && _grid.CurrentNumberOfCarsInGrid == 0)
                _gui.Invoke(new Del2(_gui.buttonStop_Click), new object[] { null,null});

            Grid tempCopy = ObjectCopier.Clone<Grid>(_grid); //creates a temporary copy of the object _grid
            DateTime dt = DateTime.Now;
            _timer.Stop();
            try
            {
                if (_gui.IsReady)
                    _gui.Invoke(new Del(_gui.refreshScreen), new object[] { tempCopy });//and sends that copy as a parameter to the GUI
            }
            catch { }
            TimeSpan t = DateTime.Now - dt;
            _timer.Interval = (t.TotalMilliseconds > _refreshRate) ? 10 : _refreshRate - t.TotalMilliseconds;
            _timer.Start();
        }
    }
    
}
