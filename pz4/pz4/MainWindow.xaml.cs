using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace pz4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NetworkModel nm;
        public CubeBuilder codeBuilder;

        private Point start = new Point();
        private Point diffOffset = new Point();
        private int zoomMax = 20;
        private int zoomCurent = 1;
        private int zoomMin = 1;

        public MainWindow()
        {
            InitializeComponent();

            Helper.InitTransofms();
            Map.Transform = Helper.group;
            MapOtherSide.Transform = Helper.group;

            CubeBuilder cubeBuilder = new CubeBuilder(Color.FromRgb(255, 0, 0));
            nm = new NetworkModel();
            nm = Helper.Deserialize("Geographic.xml");

            /*List<SubstationEntity> substationEntities = nm.Substations;
            List<NodeEntity> nodes = nm.Nodes;
            List<SwitchEntity> switches = nm.Switches;
            List<LineEntity> lines = nm.Lines;*/

            double minLat = 45.2325;
            double minLon = 19.793909;
            double maxLat = 45.277031;
            double maxLon = 19.894459;

            double lat = 0;
            double lon = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            double scale = (Helper.mapSize - 0) / (maxLat - minLat);
            double scale1 = (Helper.mapSize - 0) / (maxLon - minLon);
            string info = String.Empty;

            //Substations

            foreach (SubstationEntity se in nm.Substations)
            {
                Helper.ToLatLon(se.X, se.Y, Helper.UtmZone, out lat, out lon);
                if (lat < minLat || lat > maxLat || lon < minLon || lon > maxLon)
                    continue;

                z = (double)Helper.mapSize - ((0 + ((lat - minLat) * scale)) / Helper.windowSize) * Helper.windowSize;
                x = (double)((0 + ((lon - minLon) * scale1)) / Helper.windowSize) * Helper.windowSize;
                info = "SubstationEntity\nId: " + se.Id + "\nName: " + se.Name + "\nX: " + se.X + "\nY: " + se.Y + "\nLat: " + lat + "\nLng: " + lon;

                mainViewport.Children.Add(cubeBuilder.Create(Helper.cubeSize, x, y, z, info));
            }

            //Nodes
            cubeBuilder = new CubeBuilder(Color.FromRgb(0, 255, 0));
            foreach (NodeEntity ne in nm.Nodes)
            {
                Helper.ToLatLon(ne.X, ne.Y, Helper.UtmZone, out lat, out lon);
                if (lat < minLat || lat > maxLat || lon < minLon || lon > maxLon)
                    continue;

                z = (double)Helper.mapSize - ((0 + ((lat - minLat) * scale)) / Helper.windowSize) * Helper.windowSize;
                x = (double)((0 + ((lon - minLon) * scale1)) / Helper.windowSize) * Helper.windowSize;
                info = "NodeEntity\nId: " + ne.Id + "\nName: " + ne.Name + "\nX: " + ne.X + "\nY: " + ne.Y + "\nLat: " + lat + "\nLng: " + lon ;
                mainViewport.Children.Add(cubeBuilder.Create(Helper.cubeSize, x, y, z, info));
            }

            //switches
            cubeBuilder = new CubeBuilder(Color.FromRgb(0, 0, 255));
            foreach (SwitchEntity se in nm.Switches)
            {
                Helper.ToLatLon(se.X, se.Y, Helper.UtmZone, out lat, out lon);
                if (lat < minLat || lat > maxLat || lon < minLon || lon > maxLon)
                    continue;

                z = (double)Helper.mapSize - ((0 + ((lat - minLat) * scale)) / Helper.windowSize) * Helper.windowSize;
                x = (double)((0 + ((lon - minLon) * scale1)) / Helper.windowSize) * Helper.windowSize;
                info = "SwitchEntity\nId: " + se.Id + "\nName: " + se.Name + "\nStatus: " + se.Status + "\nX: " + se.X + "\nY: " + se.Y + "\nLat: " + lat + "\nLng: " + lon;
                mainViewport.Children.Add(cubeBuilder.Create(Helper.cubeSize, x, y, z, info));
            }

            //lines
            cubeBuilder = new CubeBuilder(Color.FromRgb(255,131,51));
            
            List<Point> points = new List<Point>();
            Model3DGroup l;
            foreach (LineEntity line in nm.Lines)
            {
                //cubeBuilder = Helper.FindColor(line.R);
                l = new Model3DGroup();
                foreach (PointEntity p in line.Vertices)
                {
                    Helper.ToLatLon(p.X, p.Y, Helper.UtmZone, out lat, out lon);
                    if (lat < minLat || lat > maxLat || lon < minLon || lon > maxLon)
                        continue;

                    z = (double)Helper.mapSize - ((0 + ((lat - minLat) * scale)) / Helper.windowSize) * Helper.windowSize;
                    x = (double)((0 + ((lon - minLon) * scale1)) / Helper.windowSize) * Helper.windowSize;
                    Point pt = new Point();
                    pt.X = x;
                    pt.Y = z;
                    points.Add(pt);
                }
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Point pt = points[i];
                    Point ptNext = points[i + 1];

                    Point3DCollection pointsCollection = new Point3DCollection();
                    pointsCollection.Add(new Point3D(pt.X, y, pt.Y));
                    pointsCollection.Add(new Point3D(pt.X + Helper.sizeOfRoute, y, pt.Y));
                    pointsCollection.Add(new Point3D(pt.X, y, pt.Y + Helper.sizeOfRoute));
                    pointsCollection.Add(new Point3D(pt.X + Helper.sizeOfRoute, y, pt.Y + Helper.sizeOfRoute));

                    pointsCollection.Add(new Point3D(ptNext.X, Helper.sizeOfRoute, ptNext.Y));
                    pointsCollection.Add(new Point3D(ptNext.X + Helper.sizeOfRoute, Helper.sizeOfRoute, ptNext.Y));
                    pointsCollection.Add(new Point3D(ptNext.X, Helper.sizeOfRoute, ptNext.Y + Helper.sizeOfRoute));
                    pointsCollection.Add(new Point3D(ptNext.X + Helper.sizeOfRoute, Helper.sizeOfRoute, ptNext.Y + Helper.sizeOfRoute));

                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[2], pointsCollection[3], pointsCollection[1]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[2], pointsCollection[1], pointsCollection[0]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[7], pointsCollection[1], pointsCollection[3]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[7], pointsCollection[5], pointsCollection[1]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[6], pointsCollection[5], pointsCollection[7]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[6], pointsCollection[4], pointsCollection[5]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[6], pointsCollection[2], pointsCollection[4]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[2], pointsCollection[0], pointsCollection[4]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[2], pointsCollection[7], pointsCollection[3]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[2], pointsCollection[6], pointsCollection[7]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[0], pointsCollection[1], pointsCollection[5]));
                    l.Children.Add(cubeBuilder.CreateTriangle(pointsCollection[0], pointsCollection[5], pointsCollection[4]));
                }

                ModelVisual3D model = new ModelVisual3D();
                model.Content = l;
                model.Transform = Helper.group;
                mainViewport.Children.Add(model);
                points.Clear();
            }
        }

        private void mainViewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainViewport.CaptureMouse();
            start = e.GetPosition(this);
            diffOffset.X = Helper.translacija.OffsetX;
            diffOffset.Y = Helper.translacija.OffsetY;
        }

        private void mainViewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainViewport.ReleaseMouseCapture();
        }

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (mainViewport.IsMouseCaptured)
            {
                Point end = e.GetPosition(this);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                double w = this.Width;
                double h = this.Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                Helper.translacija.OffsetX = diffOffset.X + (translateX / (100 * Helper.skaliranje.ScaleX));
                Helper.translacija.OffsetY = diffOffset.Y + (translateY / (100 * Helper.skaliranje.ScaleX));
            }
        }

        private void mainViewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleZ = 1;
            double scaleY = 1;

            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = Helper.skaliranje.ScaleX + 0.1;
                scaleZ = Helper.skaliranje.ScaleZ + 0.1;
                scaleY = Helper.skaliranje.ScaleY + 0.1;
                zoomCurent++;
                Helper.skaliranje.ScaleX = scaleX;
                Helper.skaliranje.ScaleZ = scaleZ;
                Helper.skaliranje.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMax)
            {
                scaleX = Helper.skaliranje.ScaleX - 0.1;
                scaleZ = Helper.skaliranje.ScaleZ - 0.1;
                scaleY = Helper.skaliranje.ScaleY - 0.1;
                zoomCurent--;
                Helper.skaliranje.ScaleX = scaleX;
                Helper.skaliranje.ScaleZ = scaleZ;
                Helper.skaliranje.ScaleY = scaleY;
            }
            
        }

        void mainViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {

            System.Windows.Point mouseposition = e.GetPosition(mainViewport);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D     
            //hitgeo = null;
            VisualTreeHelper.HitTest(mainViewport, null, HTResult, pointparams);
        }

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {
                bool gasit = false;
                for (int i = 0; i < Helper.models.Count; i++)
                {
                    if ((ModelVisual3D)Helper.models[i] == rayResult.VisualHit)
                    {
                        //hitgeo = (ModelVisual3D)rayResult.VisualHit;
                        gasit = true;
                        MessageBox.Show(Helper.tooltips[i].ToString());
                    }
                }
                if (!gasit)
                {
                    //hitgeo = null;
                }
            }

            return HitTestResultBehavior.Stop;
        }

        /* //From UTM to Latitude and longitude in decimal
         public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
         {
             bool isNorthHemisphere = true;

             var diflat = -0.00066286966871111111111111111111111111;
             var diflon = -0.0003868060578;

             var zone = zoneUTM;
             var c_sa = 6378137.000000;
             var c_sb = 6356752.314245;
             var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
             var e2cuadrada = Math.Pow(e2, 2);
             var c = Math.Pow(c_sa, 2) / c_sb;
             var x = utmX - 500000;
             var y = isNorthHemisphere ? utmY : utmY - 10000000;

             var s = ((zone * 6.0) - 183.0);
             var lat = y / (c_sa * 0.9996);
             var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
             var a = x / v;
             var a1 = Math.Sin(2 * lat);
             var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
             var j2 = lat + (a1 / 2.0);
             var j4 = ((3 * j2) + a2) / 4.0;
             var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
             var alfa = (3.0 / 4.0) * e2cuadrada;
             var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
             var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
             var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
             var b = (y - bm) / v;
             var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
             var eps = a * (1 - (epsi / 3.0));
             var nab = (b * (1 - epsi)) + lat;
             var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
             var delt = Math.Atan(senoheps / (Math.Cos(nab)));
             var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

             longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
             latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
         }

         //Deserijalicaja .xml fajla Geographic
         private NetworkModel Deserialize(string path)
         {
             NetworkModel networkModel = null;
             XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetworkModel));

             try
             {
                 StreamReader reader = new StreamReader(path);
                 networkModel = (NetworkModel)xmlSerializer.Deserialize(reader);
                 reader.Close();
             }
             catch (Exception e)
             {
                 throw new Exception("greska u deserijalizaciji", e);
             }

             return networkModel;
         } */

        #region Rotacije X,Y,Z
        private void rotateX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotateX(e.NewValue);
        }

        private void rotationY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotateY(e.NewValue);
        }

        private void rotationZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotateZ(e.NewValue);
        }

        public void RotateX(double angle)
        {
            rotX.Angle = angle;
        }

        public void RotateY(double angle)
        {
            rotY.Angle = angle;
        }

        public void RotateZ(double angle)
        {
            rotZ.Angle = angle;
        }
        #endregion Rotacije X,Y,Z
    }
}