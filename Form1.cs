using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;

namespace WpfRibbonApplication1
{
    public class FormParas
    {
        public int RotateAngle;
        public int UsingEdges;
    }
    public partial class Form1 : Form
    {
        FormParas paras = null;
        public Form1(FormParas paras)
        {
            this.paras = new FormParas();
            this.paras = paras;
            InitializeComponent();
        }

        //Parameters for building a model
        

        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            // Create a simple sphere. A pipeline is created.
            vtkPoints points = vtkPoints.New();
            vtkCellArray polys = vtkCellArray.New();
            vtkFloatArray scalars = vtkFloatArray.New();
            vtkLookupTable Luk = vtkLookupTable.New();
            int pointsNum = 0;
            MainWindow.WorkSpaceInstance.TowerModelInstance.VTKDrawModel(ref points, ref polys, ref scalars, ref pointsNum);

            Luk.SetNumberOfColors(10);
            Luk.SetTableValue(0, 0, 0, 0, 1);  //Black
            Luk.SetTableValue(1, 0.9804, 0.5020, 0.4471, 1); // Salmon
            Luk.SetTableValue(2, 0.7400, 0.9900, 0.7900, 1); // Mint
            Luk.SetTableValue(3, 0.9608, 0.8706, 0.7020, 1); // Wheat
            Luk.SetTableValue(4, 0.2000, 0.6300, 0.7900, 1); // Peacock
            Luk.SetTableValue(5, 0.5300, 0.1500, 0.3400, 1); // Raspberry
            Luk.SetTableValue(6, 1.0000, 0.4900, 0.2500, 1); // Flesh
            Luk.SetTableValue(7, 0.8900, 0.8100, 0.3400, 1); // Banana
            Luk.SetTableValue(8, 1.0000, 0.3882, 0.2784, 1); // Tomato
            Luk.SetTableValue(9, 0.9020, 0.9020, 0.9804, 1); // Lavender
            Luk.Build();
            vtkPolyData profile = vtkPolyData.New();
           
            profile.SetPoints(points);
            profile.SetPolys(polys);
            profile.GetPointData().SetScalars(scalars);

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();

            if (paras.RotateAngle == 0)
            {
                if (paras.UsingEdges == 1)
                {
                    vtkExtractEdges extractEdges = vtkExtractEdges.New();
                    extractEdges.SetInput(profile);
                    mapper.SetInputConnection(extractEdges.GetOutputPort());
                }
                else
                {
                    mapper.SetInput(profile);
                }
            }
            else
            {
                vtkRotationalExtrusionFilter refilter = vtkRotationalExtrusionFilter.New();
                refilter.SetInput(profile);
                refilter.SetResolution(10);
                refilter.SetAngle(paras.RotateAngle);
                refilter.SetTranslation(0);
                refilter.SetDeltaRadius(0);

                if (paras.UsingEdges == 1)
                {
                    vtkExtractEdges extractEdges = vtkExtractEdges.New();
                    extractEdges.SetInputConnection(refilter.GetOutputPort());
                    mapper.SetInputConnection(extractEdges.GetOutputPort());
                }
                else
                {
                    mapper.SetInputConnection(refilter.GetOutputPort());
                }        
            }
            //MessageBox.Show(polys.GetSize().ToString());
            mapper.SetScalarRange(0, 10);
            mapper.SetLookupTable(Luk);

            //Get extract edges to form a grid
            vtkExtractEdges extractGrids = vtkExtractEdges.New();
            extractGrids.SetInput(profile);

            vtkPolyDataMapper mapperGrid = vtkPolyDataMapper.New();
            mapperGrid.SetInputConnection(extractGrids.GetOutputPort());

            // The actor links the data pipeline to the rendering subsystem
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(0.388, 0.388, 0.388);

            vtkActor actor2 = vtkActor.New();
            actor2.SetMapper(mapperGrid);
            actor2.GetProperty().SetColor(0.388, 0.388, 0.388);
              
            // Create components of the rendering subsystem
            //
            vtkRenderer ren1 = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renWin = renderWindowControl1.RenderWindow;

            // Add the actors to the renderer, set the window size
            //
            
            ren1.AddViewProp(actor);
            //ren1.AddViewProp(actor2);
            renWin.SetSize(250, 250);
            renWin.Render();
            vtkCamera camera = ren1.GetActiveCamera();
            camera.Zoom(1.5);
        }
    }
}
