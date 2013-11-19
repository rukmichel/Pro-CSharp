using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    class FileHandler
    {
        /// <summary>
        /// If the project has data which is yet to be saved.
        /// </summary>
        private bool _hasUnsavedData;
        public bool HasUnsavedData
        {
            get { return _hasUnsavedData; }
            set { _hasUnsavedData = value; }
        }

        /// <summary>
        /// Location to which data is being saved.
        /// </summary>
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Saves data to file.
        /// </summary>
        /// <param name="grid">Grid object to serialize.</param>
        /// <returns>If save was succesful.</returns>
        public bool saveToFile(Grid grid) { return false;}



        /// <summary>
        /// Loads data from file.
        /// </summary>
        /// <returns>Deserialized Grid object.</returns>
        public Grid loadFromFile() { return new Grid(); }

        public bool hasUnsavedData() 
        {
            return _hasUnsavedData;
        }

        public void setUnsavedData()
        {
            _hasUnsavedData = true;
        }
    }
}