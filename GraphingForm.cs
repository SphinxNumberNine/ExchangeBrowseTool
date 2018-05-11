using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace ExchangeOnePassIdxGUI
{
    public partial class GraphingForm : Form
    {
        public GraphingForm()
        {
            InitializeComponent();
        }

        public GraphingForm(List<List<Tuple<String, int>>> values, DateTime beginningTime, DateTime endTime)
        {

            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            var model = new PlotModel { Title = "Archiving Graph" };
            model.Axes.Add(new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, TickStyle = OxyPlot.Axes.TickStyle.Outside, Position = AxisPosition.Left});
            //model.Axes.Add(new DateTimeAxis(beginningTime, endTime, null, null, DateTimeIntervalType.Auto) { Position = AxisPosition.Bottom });
            List<LineSeries> series = new List<LineSeries>();

            LineSeries ls = new LineSeries();
            LineSeries ls1 = new LineSeries();
            LineSeries ls2 = new LineSeries();
            LineSeries ls3 = new LineSeries();
            LineSeries ls4 = new LineSeries();
            LineSeries ls5 = new LineSeries();
            LineSeries ls6 = new LineSeries();
            LineSeries ls7 = new LineSeries();
            series.Add(ls);
            series.Add(ls1);
            series.Add(ls2);
            series.Add(ls3);
            series.Add(ls4);
            series.Add(ls5);
            series.Add(ls6);
            series.Add(ls7);

            Tuple<String, int>[][] arrays = values.Select(a => a.ToArray()).ToArray();

            for (int x = 0; x < arrays[0].Length; x++ ) //vertical loop
            {
                DateTime dt = DateTime.ParseExact(arrays[0][x].Item1, "yyyy-mm-dd hh:mm:ss ", null);
                for (int y = 0; y < arrays.Length; y++) //horizontal loop
                {
                    series[y].Points.Add(new DataPoint());
                }
            }
        }
    }
}
