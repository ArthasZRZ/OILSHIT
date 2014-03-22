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
using System.Data;
using Visifire.Charts;
using Com.StellmanGreene.CSVReader;

namespace WpfRibbonApplication1
{
    /// <summary>
    /// WorkingDataImporterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WorkingDataImporterWindow : Window
    {
        public WorkingDataImporterWindow()
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
            dataSeries.LegendText = "X坐标";

            DataPoint point;

            System.Windows.Forms.OpenFileDialog opfile = new System.Windows.Forms.OpenFileDialog();
            opfile.Filter = "CSV Files (*.csv)|*.csv";
            opfile.RestoreDirectory = true;
            if (opfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = opfile.FileName;
                DataTable dbtable = CSVReader.ReadCSVFile(filename, true);
                DataRow[] dbrow = dbtable.Select();
                foreach (DataRow dr in dbrow)
                {
                    point = new DataPoint();
                    point.YValue = System.Convert.ToDouble(dr[1]);
                    point.XValue = System.Convert.ToDouble(dr[0]);
                    dataSeries.DataPoints.Add(point);
                }
                chart_1.Series.Add(dataSeries);

                //ShowImportedData.ItemsSource = dbtable.AsDataView();
            }
        }
    }
}
