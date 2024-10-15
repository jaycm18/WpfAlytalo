using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfAlytalo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Lights livingRoomLights = new Lights();
        private Lights kitchenLights = new Lights();
        private Thermostat thermostat = new Thermostat();
        private Sauna sauna = new Sauna();
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer decreaseTimer = new DispatcherTimer();



        public MainWindow()
        {
            InitializeComponent();
            txtCurrentTemperature.Text = thermostat.TargetTemperature.ToString();

            // Alustetaan timer ja decreaseTimer
            timer.Interval = TimeSpan.FromSeconds(1); // Timerin päivitysväli on 1 sekunti
            timer.Tick += Timer_Tick; // Asetetaan tapahtumankäsittelijä

            decreaseTimer.Interval = TimeSpan.FromSeconds(1);
            decreaseTimer.Tick += DecreaseTimer_Tick;

        }
        // Olohuoneen valot
        private void btnLivingRoomLights_Click(object sender, RoutedEventArgs e)
        {
            livingRoomLights.Switched = !livingRoomLights.Switched;// Kääntää valojen tilan päinvastaiseksi (ON -> OFF tai OFF -> ON)
            if (livingRoomLights.Switched)
            {
                btnIndicator.Background = Brushes.Yellow;
                sliderLivingRoomLights.Value = 100;//slider menee 100, kun valot laittaa päälle
            }
            else
            {
                btnIndicator.Background = Brushes.Transparent;
                sliderLivingRoomLights.Value = 0;//slider menee 0, kun valot laittaa pois päältä
            }
        }
        // Keittiön valot
        private void btnKitchenLights_Click(object sender, RoutedEventArgs e)
        {
            kitchenLights.Switched = !kitchenLights.Switched;// Kääntää valojen tilan päinvastaiseksi (ON -> OFF tai OFF -> ON)
            if (kitchenLights.Switched)
            {
                btnIndicator2.Background = Brushes.Yellow;
                sliderKitchenLights.Value = 100;//slider menee 100, kun valot laittaa päälle
            }
            else
            {
                btnIndicator2.Background = Brushes.Transparent;
                sliderKitchenLights.Value = 0;//slider menee 0, kun valot laittaa pois päältä
            }
        }
        // Valaistuksen säätäminen olohuoneessa
        private void sliderLivingRoomLights_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double dimmerValue = sliderLivingRoomLights.Value;
            int dimmerIntValue = (int)dimmerValue; // Muunnetaan desimaaliluku suoraan int-tyyppiseksi
            livingRoomLights.Dimmer = Math.Max(0, Math.Min(100, dimmerIntValue)); // Rajoitetaan arvo 1-100 välille
            txtSliderValue.Text = livingRoomLights.Dimmer.ToString(); // Näytetään arvo TextBoxissa

            if (livingRoomLights.Switched)
            {
                if (livingRoomLights.Dimmer > 0)
                {
                    btnIndicator.Background = Brushes.Yellow;
                }
                else
                {
                    btnIndicator.Background = Brushes.Transparent;
                }
            }
            else
            {
                btnIndicator.Background = Brushes.Transparent;
            }
        }
        // Valaistuksen säätäminen keittiössä
        private void sliderKitchenLights_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double dimmerValue = sliderKitchenLights.Value;
            int dimmerIntValue = (int)dimmerValue; // Muunnetaan desimaaliluku suoraan int-tyyppiseksi
            kitchenLights.Dimmer = Math.Max(0, Math.Min(100, dimmerIntValue)); // Rajoitetaan arvo 0-100 välille
            txtSliderValue2.Text = kitchenLights.Dimmer.ToString(); // Näytetään arvo TextBoxissa

            if (kitchenLights.Switched)
            {
                if (kitchenLights.Dimmer > 0)
                {
                    btnIndicator2.Background = Brushes.Yellow;
                }
                else
                {
                    btnIndicator2.Background = Brushes.Transparent;
                }
            }
            else
            {
                btnIndicator2.Background = Brushes.Transparent;
            }
        }
        // Aseta tavoitelämpötila
        private void btnSetTemperature_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtTargetTemperature.Text, out int newTemperature))
            {
                thermostat.SetTargetTemperature(newTemperature);
                txtCurrentTemperature.Text = thermostat.TargetTemperature.ToString() + " °C";
                txtTargetTemperature.Text = string.Empty; // tyhjentää kentän uutta syöttöä varten
            }
            else
            {
                MessageBox.Show("Virheellinen lämpötila-arvo. Syötä kelvollinen kokonaisluku.");
            }
        }
        // Timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (sauna.Switched)
            {
                sauna.IncreaseTemperature(); // Saunan lämpötilaa kasvatetaan
                txtTemperature.Text = sauna.Temperature + " °C";
            }
        }
        // Käynnistä sauna ja timer saunan lämpötilan kasvattamiseksi
        private void btnStartSauna_Click(object sender, RoutedEventArgs e)
        {
            sauna.TurnOn();
            txtSaunaStatus.Text = "SAUNA PÄÄLLÄ";
            timer.Start(); // Käynnistetään timer
        }
        // Sammuta sauna ja käynnistä decreaseTimer lämpötilan laskua varten
        private void btnStopSauna_Click(object sender, RoutedEventArgs e)
        {
            sauna.TurnOff();
            txtSaunaStatus.Text = ""; // Tyhjennetään sauna päällä-teksti
            timer.Stop(); // Sammutetaan timer

            if (sauna.Temperature > 22)
            {
                decreaseTimer.Start();
            }
        }
        //decreaseTimer
        private void DecreaseTimer_Tick(object sender, EventArgs e)
        {
            sauna.DecreaseTemperature();
            txtTemperature.Text = sauna.Temperature + " °C";

            // Tarkistetaan, onko lämpötila alle 22astetta ja pysäytetään timer tarvittaessa
            if (sauna.Temperature <= 22)
            {
                decreaseTimer.Stop();
            }
        }
    }
}
   