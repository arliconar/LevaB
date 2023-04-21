using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACadSharp.Entities;
using ACadSharp.IO;
using ACadSharp.Tables;
using ACadSharp;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlTypes;
using Leva.Properties;

namespace Leva
{
    
    public class CAM
    {
        enum TypeXMLRead
        {
            None,
            Settings,
            Rising,
            High,
            Falling,
            Low
        }
        enum SubTypeXMLRead
        {
            None,
            DS,
            DE,
            P,
            GRADS,
            H,
            OFF,
            STEP
        }
        public const double pi = Math.PI;
        public Stage low = new Stage(Stage.Type.Low);
        public Stage rising = new Stage(Stage.Type.Rising);
        public Stage high = new Stage(Stage.Type.High);
        public Stage falling = new Stage(Stage.Type.Falling);
        public double step = 0.01;
        public double h = 0;
        public double off = 0;
        public double[] det = { pi / 2, pi, 3 * pi / 2 };
        public CAM(double det1 = pi / 2, double det2 = pi, double det3 = 3 * pi / 2, double h = 1, double off = 0.01, double step = 0.01)
        {
            
            this.h = h;
            this.off = off;
            this.step = step;
           
            setStages();
        }
        public void loadCam()
        {
            TypeXMLRead type = TypeXMLRead.None;
            SubTypeXMLRead stype = SubTypeXMLRead.None;

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "CamB files | *.cmb";
            open.DefaultExt = "cmb";
            if (open.ShowDialog() == DialogResult.Cancel)
                   return;

              low = new Stage(Stage.Type.Low);
              rising = new Stage(Stage.Type.Rising);
              high = new Stage(Stage.Type.High);
              falling = new Stage(Stage.Type.Falling);
            XmlTextReader reader = new XmlTextReader(open.FileName);
           
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element)
                {

                    switch (reader.Name)
                    {
                        case "SETTINGS": // The node is an element.
                            type = TypeXMLRead.Settings;

                            break;
                        case "Rising": // The node is an element.
                            type = TypeXMLRead.Rising;
                            break;
                        case "High": // The node is an element.
                            type = TypeXMLRead.High;
                            break;
                        case "Falling": // The node is an element.
                            type = TypeXMLRead.Falling;
                            break;
                        case "Low": // The node is an element.
                            type = TypeXMLRead.Low;
                            break;
                        case "GRADS":
                            stype = SubTypeXMLRead.GRADS;
                            break;
                        case "H":
                            stype = SubTypeXMLRead.H;
                            break;
                        case "OFF":
                            stype = SubTypeXMLRead.OFF;
                            break;
                        case "STEP":
                            stype = SubTypeXMLRead.STEP;
                            break;
                        case "DS":
                            stype = SubTypeXMLRead.DS;
                            break;
                        case "DE":
                            stype = SubTypeXMLRead.DE;
                            break;
                        case "P":
                            stype = SubTypeXMLRead.P;
                            break;
                    }
                }
                if(reader.NodeType==XmlNodeType.Text)
                {
                    switch(stype)
                    {
                       case SubTypeXMLRead.GRADS:
                            
                       break;

                        case SubTypeXMLRead.DS:
                            setDs(type, Double.Parse(reader.Value));
                        break;
                        
                        case SubTypeXMLRead.DE:
                            setDe(type, Double.Parse(reader.Value));
                        break;
                        
                        case SubTypeXMLRead.P:
                            SetPointsXml(type, reader.Value.ToString());
                        break;
                        case SubTypeXMLRead.H:
                            this.h = Double.Parse(reader.Value);
                        break;
                        case SubTypeXMLRead.STEP:
                            this.step = Double.Parse(reader.Value);
                        break;
                        case SubTypeXMLRead.OFF:
                            this.off = Double.Parse(reader.Value);
                        break;
                        
                    }
                }
                
            }

        }
        private void SetPointsXml(TypeXMLRead t,string s)
        {
            switch(t)
            {
                case TypeXMLRead.Rising:
                    this.rising.Pointsg.Add(St2PF(s));
                    break;
                case TypeXMLRead.Falling:
                    this.falling.Pointsg.Add(St2PF(s));


                    break;
                
            }
        }
        private PointF St2PF(string s)
        {
            string[] s2 = s.Split(',');
            return new PointF(float.Parse(s2[0]), float.Parse(s2[1]));
        }
        private void setDs(TypeXMLRead t, double ds)
        {
            switch(t)
            {
                case TypeXMLRead.Rising:
                    this.rising.degStart = ds;
                break;
                case TypeXMLRead.High:
                    this.high.degStart = ds;
                    break;
                case TypeXMLRead.Falling:
                    this.falling.degStart = ds;
                    break;
                case TypeXMLRead.Low:
                    this.low.degStart = ds;
                    break;
            }
        }
        private void setDe(TypeXMLRead t, double de)
        {
            switch (t)
            {
                case TypeXMLRead.Rising:
                    this.rising.degEnd = de;
                    break;
                case TypeXMLRead.High:
                    this.high.degEnd = de;
                    break;
                case TypeXMLRead.Falling:
                    this.falling.degEnd = de;
                    break;
                case TypeXMLRead.Low:
                    this.low.degEnd = de;
                    break;
            }
        }
        public void SaveCam()
        {

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CamB files | *.cmb";
            save.DefaultExt = "cmb";
            if(save.ShowDialog()==DialogResult.Cancel)
            {
                return;
            }

            using (XmlWriter writer = XmlWriter.Create(save.FileName))
            {
                writer.WriteStartElement("CAM");
                writer.WriteStartElement("SETTINGS");
                writer.WriteElementString("GRADS", "Deg");
                writer.WriteElementString("H", $"{this.h}");
                writer.WriteElementString("OFF", $"{this.off}");
                writer.WriteElementString("STEP", $"{this.step}");
                writer.WriteEndElement();

                //Rising
                writer.WriteStartElement("Rising");
                writer.WriteElementString("DS", $"{rising.degStart}");
                writer.WriteElementString("DE", $"{rising.degEnd}");
                foreach(PointF point in rising.Pointsg)
                {
                    writer.WriteElementString("P", $"{point.X},{point.Y}");
                }
                writer.WriteEndElement();
                //Stop High
                writer.WriteStartElement("High");
                writer.WriteElementString("DS", $"{high.degStart}");
                writer.WriteElementString("DE", $"{high.degEnd}");
                writer.WriteEndElement();

                //Falling
                writer.WriteStartElement("Falling");
                writer.WriteElementString("DS", $"{falling.degStart}");
                writer.WriteElementString("DE", $"{falling.degEnd}");

                foreach (PointF point in falling.Pointsg)
                {
                    writer.WriteElementString("P", $"{point.X},{point.Y}");
                }
                writer.WriteEndElement();
                //Stop Low
                writer.WriteStartElement("LOW");
                writer.WriteElementString("DS", $"{low.degStart}");
                writer.WriteElementString("DE", $"{low.degEnd}");
                writer.WriteEndElement();


                writer.WriteEndElement();

                writer.Flush();
            }

        }
        public List<PointF> getprofile()
        {
            
            List<PointF> points = new List<PointF>();
            points.AddRange(rising.pointspos);
            points.AddRange(high.pointspos);
            points.AddRange(falling.pointspos);
            points.AddRange(low.pointspos);
            return points;
        }

        public void setstep(double step)
        {
            this.step = step;
            rising.step = step;
            falling.step = step;
            low.step = step;
            high.step = step;

        }
        public void createdCad()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "AutoCad Drawing | *.dwg";
            save.DefaultExt = "dwg";
            if(save.ShowDialog()==DialogResult.Cancel)
            {
                return;
            }
            CadDocument doc = new CadDocument();
            List<PointF> points = getprofile();
            foreach(PointF point in points)
            {
                

                ACadSharp.Entities.Point pt = new ACadSharp.Entities.Point
                {
                    Location = new CSMath.XYZ(GetX(point),GetY(point), 0)
                };
                doc.Entities.Add(pt);
            }
            for(int i=1; i<points.Count; i++)
            {
                Line line = new Line
                {
                    StartPoint = new CSMath.XYZ(GetX(points[i-1]), GetY(points[i-1]), 0),
                    EndPoint = new CSMath.XYZ(GetX(points[i]), GetY(points[i]), 0)
                };
                doc.Entities.Add(line);
            }

            Line linef = new Line
            {
                StartPoint = new CSMath.XYZ(GetX(points.First()), GetY(points.First()), 0),
                EndPoint = new CSMath.XYZ(GetX(points.Last()), GetY(points.Last()), 0)
            };
            doc.Entities.Add(linef);
            String output = save.FileName;
            using (DxfWriter writer = new DxfWriter(output, doc, false))
            {
                writer.Write();
            }
        }
        private double GetX(PointF point)
        {
            double rad = deg2rad(point.X);
            return point.Y * Math.Cos(rad);
        }

        private double GetY(PointF point)
        {
            double rad = deg2rad(point.X);
            return point.Y * Math.Sin(rad);
        }
        public void setStages()
        {
            low = new Stage(Stage.Type.Low);
            rising = new Stage(Stage.Type.Rising);
            high = new Stage(Stage.Type.High);
            falling = new Stage(Stage.Type.Falling);
        }

        public void setPoints(Stage.Type type,List<System.Drawing.Point> points)
        {
            switch(type)
            {
                case Stage.Type.Low:
                    low.setPoints(points);
                    break;
                case Stage.Type.Rising:
                    rising.setPoints(points);
                    break;
                case Stage.Type.High:
                    high.setPoints(points);
                    break;
                case Stage.Type.Falling:
                    falling.setPoints(points);
                    break;
            }
        }
        public void calculate(Stage.Type type)
        {
            switch (type)
            {
                case Stage.Type.Low:
             //       low.calculate();
                    break;
                case Stage.Type.Rising:
                    rising.calculate();
                    break;
                case Stage.Type.High:
             //       high.calculate();
                    break;
                case Stage.Type.Falling:
            //        falling.calculate();
                    break;
            }
        }
        public static double deg2rad(double deg)
        {
            return deg * pi / 180;
        }
        public static double rad2deg(double rad)
        {
            return rad*180/pi;
        }
        public void Writepointslog(Stage.Type type)
        {/*
            using (StreamWriter writer = new StreamWriter("C:\\cam\\log.txt"))
            {
                List<PointD> points = new List<PointD>();
                switch (type)
                {
                    case Stage.Type.Rising:
                        writer.WriteLine("Rising");
                        points = rising.points;
                        break;
                    case Stage.Type.High:
                        writer.WriteLine("High");
                        points = high.points;
                        break;
                    case Stage.Type.Low:
                        writer.WriteLine("Low");
                        points = low.points;
                        break;
                    case Stage.Type.Falling:
                        writer.WriteLine("Falling");
                        points = falling.points;
                        break;
                }
                int i = 0;
                foreach(PointD point in points)
                {
                    
                    writer.WriteLine(point.ToString());
                    i++;
                }
                writer.Close();
            }*/
        }
    }

}
