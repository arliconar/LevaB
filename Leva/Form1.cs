using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Leva
{
    public partial class Form1 : Form
    {
        public CAM Cam1 = new CAM();
        double pi = CAM.pi;
        List<PointF> puntosris = new List<PointF>();
        List<PointF> puntosfall = new List<PointF>();
        Form2 profilewindow = new Form2();
        public Form1()
        {
            InitializeComponent();
            List<PointF> puntosf = new List<PointF>();
            Cam1 = new CAM(pi / 2, pi, 3 * pi / 2, 3, 0.5, 0.01);

            puntosris.Add(new PointF(0, 0));
            puntosris.Add(new PointF((float)9.9, (float)0.4));
            puntosris.Add(new PointF((float)16.65, (float)1.4));
            puntosris.Add(new PointF((float)21.15, (float)3.7));
            puntosris.Add(new PointF((float)27.9, (float)6.6));
            puntosris.Add(new PointF((float)37.75, (float)8.1));
            puntosris.Add(new PointF((float)45, (float)10));

            puntosfall.Add(new PointF(0, 1));
            puntosfall.Add(new PointF((float)0.22, (float)0.75));
            puntosfall.Add(new PointF((float)0.37, (float)0.62));
            puntosfall.Add(new PointF((float)0.47, (float)0.47));
            puntosfall.Add(new PointF((float)0.62, (float)0.37));
            puntosfall.Add(new PointF((float)0.75, (float)0.22));
            puntosfall.Add(new PointF((float)1, (float)0));


            Cam1.rising.Pointsg = puntosris;
            Cam1.falling.Pointsg = puntosfall;
            Cam1.rising.seth(5, (float)0.5);
            Cam1.falling.seth(5, (float)0.5);
            Cam1.rising.scaleaxis((float)0, (float)Convert.ToDouble(deris.Text));
            //Cam1.falling.scaleaxis((float)180, (float)270);
            profilewindow.Show();
                
            updatePoints();
            updateRising();
            updateFalling();
            resgraph();
            this.udpdateCam();

        }

        private void updatelowandhigh()
        {
            Cam1.high.calculate();

        }
        private void updateCampro()
        {
             ChartPolar.Series["Profile"].Points.Clear();
             profilewindow.ChartPolar.Series["Profile"].Points.Clear();

            List<PointF> profile = Cam1.getprofile();

            foreach (PointF points in profile)
            {
                
                ChartPolar.Series["Profile"].Points.AddXY(points.X,points.Y);
                profilewindow.ChartPolar.Series["Profile"].Points.AddXY(points.X, points.Y); ;
            }
            
        }
        private void updatePoints()
        {
            ChartRising.Series["Points"].Points.Clear();
            foreach (PointF punto in puntosris)
            {
                ChartRising.Series["Points"].Points.AddXY(punto.X, punto.Y);
                ChartRising.Series["Points"].Points.Last().Label = $"{punto.X},{punto.Y}";
                ChartRising.Series["Points"].Points.Last().MarkerSize = 20;
            }
            ChartFalling.Series["Points"].Points.Clear();
            foreach (PointF punto in puntosfall)
            {
                ChartFalling.Series["Points"].Points.AddXY(punto.X, punto.Y);
                ChartFalling.Series["Points"].Points.Last().Label = $"{punto.X},{punto.Y}";
                ChartFalling.Series["Points"].Points.Last().MarkerSize = 20;
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void updateRising()
        {
            Cam1.rising.calculate();
            ChartRising.Series["Rising"].Points.Clear();
            ChartRisingVel.Series["VelRising"].Points.Clear();
            ChartRisingAce.Series["Acel"].Points.Clear();
            ChartRisingJerk.Series["Jerk"].Points.Clear();

            foreach (PointF punto in Cam1.rising.pointspos)
            {
                ChartRising.Series["Rising"].Points.AddXY(punto.X, punto.Y);

            }
            foreach (PointF punto in Cam1.rising.pointsvel)
            {
                ChartRisingVel.Series["VelRising"].Points.AddXY(punto.X, punto.Y);

            }
            
            foreach (PointF punto in Cam1.rising.pointsacel)
            {
                ChartRisingAce.Series["Acel"].Points.AddXY(punto.X, punto.Y);

            }
            foreach (PointF punto in Cam1.rising.pointsjerk)
            {
                ChartRisingJerk.Series["Jerk"].Points.AddXY(punto.X, punto.Y);

            }
        }
        private void updateFalling()
        {
            Cam1.falling.calculate();
            
            ChartFalling.Series["Falling"].Points.Clear();

            ChartFallingVel.Series["VelRising"].Points.Clear();
            ChartFallingAce.Series["Acel"].Points.Clear();
            ChartFallingJerk.Series["Jerk"].Points.Clear();
            
            foreach (PointF punto in Cam1.falling.pointspos)
            {
                ChartFalling.Series["Falling"].Points.AddXY(punto.X, punto.Y);

            }
            
            foreach (PointF punto in Cam1.falling.pointsvel)
            {
                ChartFallingVel.Series["VelRising"].Points.AddXY(punto.X, punto.Y);

            }
            foreach (PointF punto in Cam1.falling.pointsacel)
            {
                ChartFallingAce.Series["Acel"].Points.AddXY(punto.X, punto.Y);

            }
            foreach (PointF punto in Cam1.falling.pointsjerk)
            {
                ChartFallingJerk.Series["Jerk"].Points.AddXY(punto.X, punto.Y);

            }
            updateCampro();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ChartRising_Click(object sender, EventArgs e)
        {
            
            
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resgraph();
        }
        private void resgraph()
        {
            Panelsup.Size = new Size(Panelsup.Width, this.Size.Height / 2);
            panelupfall.Size = new Size(panelupfall.Width, this.Size.Height / 2);
            ChartRising.Size = new Size(this.Width / 2, ChartRising.Height);
            ChartRisingAce.Size = new Size(this.Width / 2, ChartRisingAce.Height);
            ChartFalling.Size = new Size(this.Width / 2, ChartFalling.Height);
            ChartFallingAce.Size = new Size(this.Width / 2, ChartFallingAce.Height);
        }

        private void acelgroup_Enter(object sender, EventArgs e)
        {

        }

        private void ChartRising_MouseMove_1(object sender, MouseEventArgs e)
        {

            System.Windows.Forms.DataVisualization.Charting.HitTestResult hit = ChartRising.HitTest(e.X, e.Y, ChartElementType.DataPoint);

            if (hit.ChartElementType == ChartElementType.DataPoint && hit.Series == ChartRising.Series["Points"] && e.Button == MouseButtons.Left)
            {
                double PX = ChartRising.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                double PY = ChartRising.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                int index = hit.PointIndex;
                if (index == 0 || index == ChartRising.Series["Points"].Points.Count - 1)
                    return;

                if (PY > ChartRising.ChartAreas[0].AxisY.Maximum)
                    PY = ChartRising.ChartAreas[0].AxisY.Maximum;
                if (PY < 0)
                    PY = 0;
                ChartRising.Series["Points"].Points[index].XValue = PX;
                ChartRising.Series["Points"].Points[index].YValues[0] = PY;
                ChartRising.Update();
                String label= String.Format("{0:0.##},{1:0.##}",PX,PY);
                ChartRising.Series["Points"].Points[index].Label = label;
                Cam1.rising.Pointsg[index] = new PointF((float)PX, (float)PY);
                updateRising();
                updateCampro();
            }
        }

       
        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void ChartFalling_Move(object sender, EventArgs e)
        {

        }

        private void ChartFalling_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.HitTestResult hit = ChartFalling.HitTest(e.X, e.Y, ChartElementType.DataPoint);

            if (hit.ChartElementType == ChartElementType.DataPoint && hit.Series == ChartFalling.Series["Points"] && e.Button == MouseButtons.Left)
            {
                double PX = ChartFalling.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                double PY = ChartFalling.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                int index = hit.PointIndex;
                if (index == 0 || index == ChartFalling.Series["Points"].Points.Count - 1)
                    return;

                if (PY > ChartFalling.ChartAreas[0].AxisY.Maximum)
                    PY = ChartFalling.ChartAreas[0].AxisY.Maximum;
                if (PY < 0)
                    PY = 0;
                ChartFalling.Series["Points"].Points[index].XValue = PX;
                ChartFalling.Series["Points"].Points[index].YValues[0] = PY;
                ChartFalling.Update();
                ChartFalling.Series["Points"].Points[index].Label = $"{(int)PX},{(int)PY}";
                Cam1.falling.Pointsg[index] = new PointF((float)PX, (float)PY);
                updateFalling();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Cam1.rising.seth((float) Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));
            Cam1.falling.seth((float)Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));
            updatePoints();
            updateFalling();
            updateRising();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            udpdateCam();
        }
        private void udpdateCam()
        {
            Cam1.rising.scaleaxis((float)0, (float)Convert.ToDouble(deris.Text));
            Cam1.high.scaleaxis((float)Convert.ToDouble(deris.Text), (float)Convert.ToDouble(dehi.Text));
            Cam1.falling.scaleaxis((float)Convert.ToDouble(dehi.Text), (float)Convert.ToDouble(defa.Text));
            Cam1.low.scaleaxis((float)Convert.ToDouble(defa.Text), (float)360);
            Cam1.setstep(Convert.ToDouble(steptext.Text));
            Cam1.rising.seth((float)Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));
            Cam1.falling.seth((float)Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));
            Cam1.high.seth((float)Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));
            Cam1.low.seth((float)Convert.ToDouble(htext.Text), (float)Convert.ToDouble(offtext.Text));

            updatePoints();
            updateFalling();
            updateRising();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void ChartRising_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            lhigh.Text = deris.Text + " to";
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            lfall.Text = dehi.Text + " to";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            llow.Text = defa.Text + " to";
        }

        private void Config_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Cam1.createdCad();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                profilewindow.Show();
            }
            else
            {
                profilewindow.Hide();
            }
            externalWindowToolStripMenuItem.Checked=checkBox1.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profilewindow.Close();
            this.Close();

        }

        private void sAVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam1.SaveCam();
        }

        private void penToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam1.loadCam();
            udpdateCam();
            htext.Text = Cam1.h.ToString();
            steptext.Text = Cam1.step.ToString();
            offtext.Text = Cam1.off.ToString();
            deris.Text = Cam1.rising.degEnd.ToString();
            lhigh.Text = Cam1.rising.degEnd.ToString()+" to";
            dehi.Text = Cam1.high.degEnd.ToString();
            lfall.Text = Cam1.high.degEnd.ToString()+" to";
            defa.Text = Cam1.falling.degEnd.ToString();
            llow.Text = Cam1.falling.degEnd.ToString()+" to";
            delo.Text = Cam1.low.degEnd.ToString();


        }

        private void saveDWGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam1.createdCad();

        }

        private void externalWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (externalWindowToolStripMenuItem.Checked)
            {
                profilewindow.Show();
            }
            else
            {
                profilewindow.Hide();
            }
            checkBox1.Checked = externalWindowToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(AboutBox1 about=new AboutBox1())
            {
                about.ShowDialog();
            }
        }
    }
} 
