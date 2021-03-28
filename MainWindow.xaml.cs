using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace mapOverview
{
    public partial class MainWindow : Window
    {
        private List<List<string>> population;
        private List<int> cases;
        private Stopwatch stopWatch = new Stopwatch();
        private int clickTime;
        private bool landClicked;
        public MainWindow()
        {
            InitializeComponent();

            //Define random value, to put in 
            var randomValue = new Random();
            heatmapValues = new Dictionary<string, double>();

            //Used for syncing the number put in as heatmap value and the actual cases 
            cases = new List<int>();
            for (int a = 0; a < 402; a++)
            {
                int tempValue = randomValue.Next(10000);
                Cases.Add(tempValue);
                heatmapValues[a.ToString()] = tempValue;
            }

            //Reading in another file, to extract the population of each district
            //Kreisgrenzen.kml has the district name and the population connected to it
            Population = PopulationReturner("Kreisgrenzen.kml");


            //The map, which is loaded on the window
            LiveCharts.Wpf.GeoMap geomap = new LiveCharts.Wpf.GeoMap();
            
            //Defining the looks of the map
            //Landkreise.xml has the district name and the federal state connected to it
            geomap.Source = "Landkreise.xml";
            geomap.HeatMap = heatmapValues;
            geomap.EnableZoomingAndPanning = true;
            geomap.ClipToBounds = true;
            geomap.LandStrokeThickness = 0.1;

            //Defining the colors which are used
            //Black outline, yellow the lowest and red the highest heatmap value
            geomap.LandStroke = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#000000"); 
            geomap.GradientStopCollection[0].Color = System.Windows.Media.Color.FromArgb(255, 245, 255, 0);
            geomap.GradientStopCollection[1].Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);

            //Defining action listener
            //Mousebutton listeners + landclick listener
            //You have to cover all cases:
            //1: Map clicked
            //1.1: Map clicked shortly, to display information
            //1.2: Map clicked long, to move the map
            //2: Empty Space clicked (No need to differentiate between long and short clicks, because pop up wont be displayed anyway)
            geomap.MouseLeftButtonDown += Geomap_MouseLeftButtonDown;
            geomap.MouseLeftButtonUp += Geomap_MouseLeftButtonUp;
            geomap.LandClick += Geomap_LandClick;
            gridGeoMap.Children.Add(geomap);

        }
        static public List<List<string>> PopulationReturner(string filePath)
        {
            List<List<string>> population = new List<List<string>>();

            XDocument doc = new XDocument();

            //Load the Kml file + exception handling
            try
            {
                doc = XDocument.Load(filePath);
            }
            // Catch "File Not Found" errors
            catch (System.IO.FileNotFoundException ew)
            {
                Environment.Exit(1);
            }
            // Catch Argument Exceptions
            catch (System.ArgumentException)
            {
                Console.WriteLine("Invalid path detected!");
                Environment.Exit(1);
            }
            // Catch all other errors, and print them to console.
            catch (Exception err)
            {
                Console.WriteLine("An Exception has been caught:");
                Console.WriteLine(err);
                Environment.Exit(1);
            }

            XElement root = doc.Root;
            XNamespace nameSpace = root.GetDefaultNamespace();

            var placemarks = doc.Root.Descendants(nameSpace + "Placemark");
            //Loop over all existing placemarks in the Kml file
            foreach (XElement actualPlacemark in placemarks)
            {
                List<string> districtPopulation = new List<string>();
                List<XElement> extendedDatas = actualPlacemark.Descendants(nameSpace + "ExtendedData").ToList();
                List<XElement> simpleFields = extendedDatas.Descendants(nameSpace + "SimpleData").ToList();
                //Extract population and districtname out of Kreisgrenzen.kml . districtname will be used in string matching later
                districtPopulation.Add(simpleFields[4].Value);
                districtPopulation.Add(simpleFields[17].Value);
                population.Add(districtPopulation);

            }
            return population;
        }
        private void Geomap_LandClick(object arg1, LiveCharts.Maps.MapData arg2)
        {
            LandClicked = true;
            MyPopup.IsOpen = false;

            //Defining values, to display in the pop up
            var r = new Random();
            double randomValue;

            //Overview of variables which need to be displayed
            string districtName;
            double casesPerHundredK;
            double casesSevenDaysHundredK;
            int deaths;
            int population = 0;
            string federalState;
            int casesNumber = 0;
            string districtIdString = arg2.Id;
            int districtId = int.Parse(districtIdString);

            for (int a = 0; a < 402; a++)
            {
                if (districtId == a)
                {
                    casesNumber = cases[a];
                }
            }


            //District name is read in with the Landkreise.xml file and stored in the name
            districtName = arg2.Name.Substring(0, arg2.Name.IndexOf(" | "));

            //5000 indicates the maximum number of infected
            randomValue = 0.0 + (5000 - 0.0) * r.NextDouble();
            casesPerHundredK = Math.Round(randomValue, 1);

            randomValue = 0.0 + (1000 - 0.0) * r.NextDouble();
            casesSevenDaysHundredK = Math.Round(randomValue, 1);

            deaths = r.Next(100);

            //Getting the population number out of Kreisgrenzen.kml file
            for (int a = 0; a < Population.Count; a++)
            {
                if (districtName.Equals(Population[a][0]))
                {
                    population = int.Parse(Population[a][1]);
                }

            }

            //Setting the federal state name, which is also stored in arg2 after the district name and the seperator |
            int stringLength = districtName.Length + 3;
            federalState = arg2.Name.Substring(stringLength);
            
            //Concetinating the final string to display on the pop up
            string finalOutputString = "Fälle: " + casesNumber + "\nFälle / 100.000 EW: " + casesPerHundredK + "\nFälle letzte 7 Tage/ 100.000 EW: " + casesSevenDaysHundredK + "\nTodesfälle: " + deaths + "\nEinwohnerzahl: " + population + "\nBundesland: " + federalState;

            //This is the final output in the pop up
            districtNamePop.Text = districtName;
            districtInfo.Text = finalOutputString;
        }
        private void Geomap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            ClickTime = ts.Milliseconds / 10;
            //If its a short clicktime, it indicates the user wanted to click a land, if its long, the user just wanted to move the map
            //LandClicked is only set true, if Geomap_Landclick fires, which then sets the value to true
            if(ClickTime < 14 && LandClicked == true)
            {
                MyPopup.IsOpen = true;
            }
            LandClicked = false;
        }
        private void Geomap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            //Close any popup instance
            MyPopup.IsOpen = false;
            stopWatch.Reset();
            stopWatch.Start();
        }
        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = false;
        }
        public Dictionary<string, double> heatmapValues { get; set; }
        public List<List<string>> Population { get => population; set => population = value; }
        public List<int> Cases { get => cases; set => cases = value; }
        public Stopwatch StopWatch { get => stopWatch; set => stopWatch = value; }
        public int ClickTime { get => clickTime; set => clickTime = value; }
        public bool LandClicked { get => landClicked; set => landClicked = value; }
    }
}
