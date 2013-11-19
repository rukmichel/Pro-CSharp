using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Traffic_Simulator
{
    [Serializable]
    class Crossing_1 : Crossing
    {
        /// <summary>
        /// The lights only for crossing type 1.
        /// </summary>
        protected TrafficLight _lightNtoWS, _lightStoEN, _lightNtoE, _lightStoW;
        
        public TrafficLight LightNtoWS
        {
            get { return _lightNtoWS;}
            set { _lightNtoWS = value; }
        }
        
        public TrafficLight LightStoEN
        {
            get { return _lightStoEN;}
            set { _lightStoEN = value; }
        }
        
        public TrafficLight LightNtoE
        {
            get { return _lightNtoE;}
            set { _lightNtoE = value; }
        }
        
        public TrafficLight LightStoW
        {
            get { return _lightStoW;}
            set { _lightStoW  = value; }
        }
        
        /// <summary>
        /// Method to update all lights.
        /// </summary>
        protected override void updateLights() 
        {

            _lightEtoNW._color = Color.Red;
            _lightWtoSE._color = Color.Red;
            _lightEtoS._color = Color.Red;
            _lightWtoN._color = Color.Red;
            _lightNtoWS._color = Color.Red;
            _lightStoEN._color = Color.Red;
            _lightNtoE._color = Color.Red;
            _lightStoW._color = Color.Red;

            switch (_state)
            {
                case 0:
                    _lightEtoNW._color = Color.Gray;
                    _lightWtoSE._color = Color.Gray;
                    _lightEtoS._color = Color.Gray;
                    _lightWtoN._color = Color.Gray;
                    _lightNtoWS._color = Color.Gray;
                    _lightStoEN._color = Color.Gray;
                    _lightNtoE._color = Color.Gray;
                    _lightStoW._color = Color.Gray;
                    break;
                
                case 1:
                    _lightEtoNW._color = Color.Green;
                    _lightWtoSE._color = Color.Green;
                    break;

                case 2:
                    _lightWtoSE._color = Color.Green;
                    _lightEtoS._color = Color.Green;
                    break;

                case 3:
                    _lightNtoWS._color = Color.Green;
                    _lightStoEN._color = Color.Green;
                    break;

                case 4:
                    _lightNtoE._color = Color.Green;
                    _lightStoW._color = Color.Green;
                    break;
            }
        }

        /// <summary>
        /// Method that counts to next step.
        /// </summary>
        public override void timeTick() 
        {
            int t1, t2, t3, t4;
            
            ///get highest value per state
            t1 = (_lightEtoNW._greenLightTime > _lightWtoSE._greenLightTime) ? _lightEtoNW._greenLightTime : _lightWtoSE._greenLightTime; 
            t2 = t1 + ((_lightWtoN._greenLightTime > _lightEtoS._greenLightTime) ? _lightWtoN._greenLightTime : _lightEtoS._greenLightTime);
            t3 = t2 + ((_lightNtoWS._greenLightTime > _lightStoEN._greenLightTime) ? _lightNtoWS._greenLightTime : _lightStoEN._greenLightTime);
            t4 = t3 + ((_lightNtoE._greenLightTime > _lightStoW._greenLightTime) ? _lightNtoE._greenLightTime : _lightStoW._greenLightTime);
            if (_tickCount == t4)
            {
                _tickCount = 0;
                updateLights();
                _state = 1;
            }
            else
            {
                _tickCount++;
                if (_tickCount == t1) 
                {
                    _state = 2;
                    updateLights();
                }

                if (_tickCount == t2)
                {
                    _state = 3;
                    updateLights();
                }

                if (_tickCount == t3)
                {
                    _state = 4;
                    updateLights();
                }

            }          
        }
    }
}
