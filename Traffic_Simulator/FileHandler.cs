using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    public class FileHandler
    {
        /// <summary>
        /// If the project has data which is yet to be saved.
        /// </summary>
        private bool _hasUnsavedData;
        //public bool HasUnsavedData
        //{
        //    get { return _hasUnsavedData; }
        //    set { _hasUnsavedData = value; }
        //}
        
        /// <summary>
        /// Location to which data is being saved.
        /// </summary>
        private string _path = "";
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
        public bool saveToFile(Grid grid) {
            
            FileStream fileStream=null;
            
            try
            {
                fileStream = new FileStream(_path, FileMode.Create);
                BinaryFormatter binaryFormater = new BinaryFormatter();
                _hasUnsavedData = false;

                binaryFormater.Serialize(fileStream, grid.Slots);

                fileStream.Close();
                //Grid objectToLoad = (Grid)binaryFormater.Deserialize(fileStream);
                return true;
            }
            catch(Exception)
            {
                if (fileStream != null)
                    fileStream.Close();
                return false;
            }

        }



        /// <summary>
        /// Loads data from file.
        /// </summary>
        /// <returns>Deserialized Grid object.</returns>
        public Grid loadFromFile() { 
            
            if (string.IsNullOrEmpty(_path)) { return default(Grid); }

            Crossing[,] objectToLoad;// = default(Grid);

            try
            {
                Stream stream = File.Open(_path, FileMode.Open);
                BinaryFormatter binaryFormater = new BinaryFormatter();
                objectToLoad = (Crossing[,])binaryFormater.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                throw new FileLoadException();
            }
            Grid g = new Grid();
            g.Slots = objectToLoad;
            return g;
            
        }
        
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