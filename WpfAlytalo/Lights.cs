using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAlytalo
{
    //valot
    public class Lights
    {
        private bool switched;
        private int dimmer;

        public bool Switched
        {
            get { return switched; }
            set
            {
                switched = value;
            }
        }

        public int Dimmer
        {
            get { return dimmer; }
            set
            {
                // Tarkistaa, ettei himmennys mene yli rajojen (0-100)
                dimmer = Math.Max(0, Math.Min(100, value));
            }
        }

        public Lights()
        {
            // oletusarvot
            Switched = false;
            Dimmer = 0;
        }
    }
    //lämpötila
    public class Thermostat
    {
        private int targetTemperature;

        public int TargetTemperature
        {
            get { return targetTemperature; }
            set { targetTemperature = value; }
        }

        public Thermostat()
        {
            // Asetetaan oletustavoitelämpötila
            TargetTemperature = 22;
        }

        public void SetTargetTemperature(int newTemperature)
        {
            TargetTemperature = newTemperature;
        }
    }
    //sauna
    public class Sauna
    {
        private bool switched;
        private double temperature;
        private const double heatingRate = 0.5; // Lämpötilan nousun nopeus (asteina/sekunti)
        private const double coolingRate = 1.0; // Lämpötilan laskun nopeus (asteina/sekunti)


        public bool Switched
        {
            get { return switched; }
        }

        public double Temperature
        {
            get { return temperature; }
        }


        public Sauna()
        {
            // Alustetaan oletusarvot
            switched = false;
            temperature = 22;
        }

        public void TurnOn()
        {
            switched = true;
        }

        public void TurnOff()
        {
            switched = false;
        }

        public void IncreaseTemperature()
        {
            if (switched)
            {
                temperature += heatingRate;
            }
        }

        public void DecreaseTemperature()
        {
            if (temperature > 22)// Ei anneta saunan lämpötilan laskea alle talon peruslämpötilan
            {
                temperature -= coolingRate;
            }
        }
    }
}
