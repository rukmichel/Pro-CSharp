using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Traffic_Simulator
{
    abstract class Crossing
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
        protected int _tickCount = 0;

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
        protected TrafficLight _lightEtoNW = new TrafficLight(),
            _lightEtoS = new TrafficLight(), 
            _lightWtoN = new TrafficLight(),
            _lightWtoSE = new TrafficLight();

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
           set { _ID = value; }
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
        ///  Determines the turning probability of the car from one street to another.
        /// </summary>
        protected float _probEtoW, _probEtoN, _probEtoS, _probWtoN, _probWtoS, _probStoN,
           _probStoE, _probStoW, _probNtoS, _probNtoW, _probNtoE;

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

       /// <summary>
       ///  the time for light to change
       /// </summary>
       public bool reset ()
       {
           _state = 0;
           _tickCount = 0;
           return true;
       }
        
       /// <summary>
       ///  the time for light to change
       /// </summary>
       public abstract void timeTick();

        /// <summary>
        /// constructor
        /// </summary>
        public Crossing()
        { 
            _intersection = new Street(this.GetType(), Direction.Center);
            _streetN = new Street(this.GetType(), Direction.North);
            _streetE = new Street(this.GetType(), Direction.East);
            _streetS = new Street(this.GetType(), Direction.South); 
            _streetW = new Street(this.GetType(), Direction.West);
  
        }

    }  
}
