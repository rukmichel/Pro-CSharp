using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Simulator
{
    [Serializable]
    class Crossing_2 : Crossing
    {
        /// <summary>
        /// The light for the pedestrian and you can find it on crossing type 2 only.
        /// </summary>        
        private TrafficLight pedestrianLight;

        /// <summary>
        /// Method to update all lights.
        /// </summary>
        protected override void updateLights() { }

        /// <summary>
        /// Method that counts to next step.
        /// </summary>
        public override void timeTick() { }
    }
}
