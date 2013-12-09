using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Traffic_Simulator
{
    [Serializable]
    public class Crossing_2 : Crossing
    {
        /// <summary>
        /// The lights only for crossing type 2.
        /// </summary>        
        private TrafficLight _lightNtoS = new TrafficLight(10), _lightStoN = new TrafficLight(10), _pedestrianLight = new TrafficLight(10);

        public TrafficLight LightNtoS
        {
            get { return _lightNtoS;}
            set { _lightNtoS = value; }
        }

        public TrafficLight LightStoN
        {
            get { return _lightStoN; }
            set { _lightStoN = value; }
        }

        public TrafficLight LightPedestrian
        {
            get { return _pedestrianLight; }
            set { _pedestrianLight = value; }
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
            _lightNtoS._color = Color.Red;
            _lightStoN._color = Color.Red;
            _pedestrianLight._color = Color.Red;

            switch (_state)
            {
                case 0:
                    _lightEtoNW._color = Color.Gray;
                    _lightWtoSE._color = Color.Gray;
                    _lightEtoS._color = Color.Gray;
                    _lightWtoN._color = Color.Gray;
                    _lightNtoS._color = Color.Gray;
                    _lightStoN._color = Color.Gray;
                    _pedestrianLight._color = Color.Gray;
                    break;

                case 1:
                    _lightEtoNW._color = Color.Green;
                    _lightWtoSE._color = Color.Green;
                    break;

                case 2:
                    _lightWtoN._color = Color.Green;
                    _lightEtoS._color = Color.Green;
                    break;

                case 3:
                    _lightNtoS._color = Color.Green;
                    _lightStoN._color = Color.Green;
                    break;

                case 4:
                    _pedestrianLight._color = Color.Green;
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
            t3 = t2 + ((_lightNtoS._greenLightTime > _lightStoN._greenLightTime) ? _lightNtoS._greenLightTime : _lightStoN._greenLightTime);
            t4 = t3 + _pedestrianLight._greenLightTime;

            _tickCount++;
            if (_tickCount == t4)
            {
                _tickCount = -1;
                _state = 1;
                updateLights();
            }
            else
            {
                if (_tickCount == 0)
                {
                    _state = 1;
                    updateLights();
                }
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

        public override  bool reset() 
        {
            base.reset();
            _lightNtoS._color = Color.Gray;
            _lightStoN._color = Color.Gray;
            _pedestrianLight._color = Color.Gray;

            return true;
        }

    }
}
