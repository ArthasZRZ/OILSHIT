using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Visifire.Charts;


namespace WpfRibbonApplication1.Views
{
    /// <summary>
    /// WorkingStatusWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WorkingStatusWindow : Window
    {
        public WorkingStatusWindow()
        {
            InitializeComponent();
            ShowLine();
        }

        public void ShowLine()
        {
            Title title = new Title();
            title.Text = "This is chart 1";
            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Spline;
            dataSeries.LegendText = "xx";
    
            int y = 0;
            DataPoint point;
            for (int time = 0; time < 50; time+=5)
            {
                point = new DataPoint();
                point.YValue = (-2.36*(time/3600)*(time/3600)+161*time/9000-40.2)*(y+6.529)/35.976+2.36*(time/3600)*(time/3600)+49*time/9000+107.2;
                point.XValue = time;
                dataSeries.DataPoints.Add(point);
            }
            StatusChart_1.Series.Add(dataSeries);

            DataSeries dataseries2 = new DataSeries();
            dataseries2.RenderAs = RenderAs.Spline;
            dataseries2.Legend = "yy";
            for (int time = 0; time < 50; time ++)
            {
                point = new DataPoint();
                point.YValue = (-2.36 * (time / 3600) * (time / 3600) - 49 * time / 9000 + 295.8) * (y + 6.529) / 35.976 + 2.36 * (time / 3600)*(time / 3600) + 49 * time / 9000 + 107.2;
                point.XValue = time;
                dataseries2.DataPoints.Add(point);
            }
            StatusChart_1.Series.Add(dataseries2);
        }
    }
}
