using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected Street _intersection, _streetN, _streetE, _streetS, _streetW;
       
        /// <summary>
        /// Represents traffic lights for each direction on the street,for instance _lightEtoW: light for the car coming from East to West.
        /// </summary>
       protected TrafficLight _lightEtoNW, _lightEtoS, _lightWtoN, _lightWtoSE;
        
        /// <summary>
        /// Indicates a position on the grid, for instance "A3".
        /// </summary>
       protected string _ID;
        /// <summary>
        /// Gives the flow of the car in each entry of the crossing.
        /// </summary>

       protected int _flowN, _flowE, _flowS, _flowW;
        
        /// <summary>
        ///  Determines the turning probability of the car from one street to another.
        /// </summary>
        protected float _probEtoW, _probEtoN, _probEtoS, _probWtoN, _probWtoS, _probStoN,
           _probStoE, _probStoW, _probNtoS, _probNtoW, _probNtoE;

       /// <summary>
       ///  Update all traffic light.
       /// </summary>
       protected abstract void updateLights();

       /// <summary>
       ///  the time for light to change
       /// </summary>
       public abstract void timeTick();

       public Street[] getStreets() { 
           Street[] st= {_intersection, _streetN, _streetE, _streetS, _streetW};
           return st;
       }

       public abstract TrafficLight[] getTrafficLights()
       {
           TrafficLight[] tf = { _lightEtoNW, _lightEtoS, _lightWtoN, _lightWtoSE };
           return tf;
       }

       public string getId() {
           return _ID;
       }

       public int [] getFlows()
       {
           int [] fl = {_flowN, _flowE, _flowS,_flowW};
           return fl;
       }

       public float[] getProbabilities() {

           float[] pb = { _probEtoW, _probEtoN, _probEtoS, _probWtoN, _probWtoS, _probStoN,
                            _probStoE, _probStoW, _probNtoS, _probNtoW, _probNtoE };
           return pb;
       }
    
    }  
}
