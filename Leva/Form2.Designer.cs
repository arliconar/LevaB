namespace Leva
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ChartPolar = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPolar)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartPolar
            // 
            chartArea3.Name = "ChartArea1";
            this.ChartPolar.ChartAreas.Add(chartArea3);
            this.ChartPolar.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.ChartPolar.Legends.Add(legend3);
            this.ChartPolar.Location = new System.Drawing.Point(0, 0);
            this.ChartPolar.Name = "ChartPolar";
            series3.BorderWidth = 5;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series3.Legend = "Legend1";
            series3.Name = "Profile";
            this.ChartPolar.Series.Add(series3);
            this.ChartPolar.Size = new System.Drawing.Size(800, 450);
            this.ChartPolar.TabIndex = 1;
            this.ChartPolar.Text = "chart1";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.ChartPolar);
            this.Name = "Form2";
            this.Text = "Profile";
            ((System.ComponentModel.ISupportInitialize)(this.ChartPolar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart ChartPolar;
    }
}