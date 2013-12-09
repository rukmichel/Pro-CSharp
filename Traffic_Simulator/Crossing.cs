using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Traffic_Simulator
{
    [Serializable]
    public abstract class Crossing
    {   
        /// <summary>
        /// Represents the current state of the crossing.
        /// 0 - Not running
        /// 1 - EtoNW and WtoSE green
        /// 2 - WtoN and EtoS green
        /// 3 - NtoS and StoN green
        /// 4 - Pedestrian green
        /// </summary>
        protected int _state;

        /// <summary>
        /// Keeps track of the times ticks
        /// </summary>
        protected int _tickCount = -1;

        /// <summary>
        /// Gives the specific street on the lane on each crossing.
        /// </summary>
        protected Street _intersection,  _streetN, _streetE, _streetS, _streetW;
  
        public Street Intersection
        {
            get { return _intersection; }
            set { _intersection = value; }
        }

        public Street StreetN
        {
            get { return _streetN; }
            set { _streetN = value; }
        }

        public Street StreetE
        {
            get { return _streetE; }
            set { _streetE = value; }
        }

        public Street StreetS
        {
            get { return _streetS; }
            set { _streetS = value; }
        }

        public Street StreetW
        {
            get { return _streetW; }
            set { _streetW = value; }
        }

        /// <summary>
        /// Represents traffic lights for each direction on the street,for instance _lightEtoW: light for the car coming from East to West.
        /// </summary>
        protected TrafficLight _lightEtoNW = new TrafficLight(10),
            _lightEtoS = new TrafficLight(10), 
            _lightWtoN = new TrafficLight(10),
            _lightWtoSE = new TrafficLight(10);

       public TrafficLight LightEtoNW
       {
           get { return _lightEtoNW; }
           set { _lightEtoNW = value; }

       }

       public TrafficLight LightWtoSE
       {
           get { return _lightWtoSE; }
           set { _lightWtoSE = value; }
       }

       public TrafficLight LightWtoN
       {
           get { return _lightWtoN; }
           set { _lightWtoN = value; }
       }

       public TrafficLight LightEtoS
       {
           get { return _lightEtoS; }
           set { _lightEtoS = value; }
       }

        /// <summary>
        /// Indicates a position on the grid, for instance "A3".
        /// </summary>
       protected string _ID;
       public string ID
        {
            get { return _ID; }
        }
        
        /// <summary>
        /// Gives the flow of the car in each entry of the crossing.
        /// </summary>
       protected int _flowN, _flowE, _flowS, _flowW;

       public int FlowN 
       {
           get { return _flowN;}
           set { _flowN = value; }
       } 

       public int FlowE
       {
           get { return _flowE; }
           set { _flowE = value; }
       } 
        
       public int FlowS 
       {
           get { return _flowS;}
           set { _flowS = value; }
       } 

       public int FlowW 
       {
           get { return _flowW;}
           set { _flowW = value; }
       }

       /// <summary>
       /// Stores the amount of Cars that have already entered this crossing
       /// </summary>
       protected int _enteredN, _enteredE, _enteredS, _enteredW;

       public int EnteredN
       {
           get { return _enteredN; }
           set { _enteredN = value; }
       }

       public int EnteredE
       {
           get { return _enteredE; }
           set { _enteredE = value; }
       }

       public int EnteredS
       {
           get { return _enteredS; }
           set { _enteredS = value; }
       }

       public int EnteredW
       {
           get { return _enteredW; }
           set { _enteredW = value; }
       } 
        
        /// <summary>
        ///  Determines the turning probability of the car from one street to another.
        /// </summary>
       protected float _probEtoW = 1, _probEtoN = 1, _probEtoS = 1, _probWtoN = 1, _probWtoS = 1, _probWtoE = 1,
            _probStoN = 1, _probStoE = 1, _probStoW = 1, _probNtoS = 1, _probNtoW = 1, _probNtoE = 1;

        public float ProbEtoW 
        {
            get { return _probEtoW; }
           set { _probEtoW = value; }
        }

        public float ProbEtoN 
        {
            get { return _probEtoN; }
           set { _probEtoN = value; }
        }

        public float ProbEtoS 
        {
            get { return _probEtoS; }
           set { _probEtoS = value; }
        }

        public float ProbWtoN
        {
            get { return _probWtoN; }
           set { _probWtoN = value; }
        }

        public float ProbWtoS
        {
            get { return _probWtoS; }
           set { _probWtoS = value; }
        }

        public float ProbWtoE
        {
            get { return _probWtoE; }
            set { _probWtoE = value; }
        }

        public float ProbStoN
        {
            get { return _probStoN; }
           set { _probStoN = value; }
        }

        public float ProbStoE
        {
            get { return _probStoE; }
           set { _probStoE = value; }
        }
        
        public float ProbStoW
        {
            get { return _probStoW; }
           set { _probStoW = value; }
        }
       
        public float ProbNtoS
        {
            get { return _probNtoS; }
           set { _probNtoS = value; }
        }

        public float ProbNtoW
        {
            get { return _probNtoW; }
           set { _probNtoW = value; }
        }

         public float ProbNtoE
        {
            get { return _probNtoE; }
           set { _probNtoE = value; }
        }
        
       /// <summary>
       ///  Update all traffic light.
       /// </summary>
       protected abstract void updateLights();

       public virtual bool reset ()
       {
           //clears all streets (remove all cars)
           _intersection = new Street(this.GetType(), Direction.Center);
            _streetN = new Street(this.GetType(), Direction.North);
            _streetE = new Street(this.GetType(), Direction.East);
            _streetS = new Street(this.GetType(), Direction.South); 
            _streetW = new Street(this.GetType(), Direction.West);

           //clears all lights
            _lightEtoNW._color = Color.Gray;
            _lightWtoSE._color = Color.Gray;
            _lightEtoS._color = Color.Gray;
            _lightWtoN._color = Color.Gray;

           _state = 0;
           _tickCount = -1;
           return true;
       }
        
       /// <summary>
       ///  the time for light to change
       /// </summary>
       public abstract void timeTick();

        /// <summary>
        /// constructor
        /// </summary>
        public Crossing(string id)
        {
            this._ID = id;
            reset();  
        }

    }  
}
