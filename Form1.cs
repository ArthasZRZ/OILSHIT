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
        public int UsingVirtualHeater;
        public int Using3DTower;
        public double Width;
        public double Height;

        public FormParas()
        {
            RotateAngle = 0;
            UsingEdges = 1;
            UsingVirtualHeater = 0;
            Using3DTower = 1;

            Width = 300.0;
            Height = 300.0;
        }
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

        private vtkActor BasicVTKBuilder()
        {
            // Create a simple sphere. A pipeline is created.
            vtkPoints points = vtkPoints.New();
            vtkCellArray polys = vtkCellArray.New();
            vtkFloatArray scalars = vtkFloatArray.New();
            vtkLookupTable Luk = vtkLookupTable.New();
            int pointsNum = 0;
            MainWindow.WorkSpaceInstance.TowerModelInstance.VTKDrawModel(ref points, ref polys, ref scalars, ref pointsNum);

            Luk.SetNumberOfColors(6);
            Luk.SetTableValue(0, 0, 1, 0, 1); //middle: 
            Luk.SetTableValue(1, 0, 0, 1, 1); //border
            Luk.SetTableValue(2, 1, 0, 0, 1); //insider
            Luk.SetTableValue(3, 0.854902, 0.647059, 0.12549, 1);
            Luk.SetTableValue(4, 1, 0, 0, 1); //dont know
            Luk.SetTableValue(5, 1, 0, 0, 1);
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

            // The actor links the data pipeline to the rendering subsystem
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(0.388, 0.388, 0.388);

            return actor;
        }

        private vtkActor2D VirtualHeaterVTKBuilder()
        {
            vtkActor2D actor = vtkActor2D.New();

            vtkPoints pointSource = vtkPoints.New();
            vtkStringArray labels = vtkStringArray.New();
            vtkCellArray verts = vtkCellArray.New();

            MainWindow.WorkSpaceInstance.TowerModelInstance.VTKLabelGetter(ref pointSource, ref labels, ref verts);

            //MessageBox.Show(labels.ToString());
            vtkPolyData polyData = vtkPolyData.New();
            polyData.SetPoints(pointSource);
            polyData.SetVerts(verts);
            polyData.GetPointData().AddArray(labels);

            vtkTextProperty textProp = vtkTextProperty.New();
            textProp.SetFontSize(12);
            textProp.SetColor(1.0, 1.0, 0.5);
            textProp.SetFontFamilyToArial();

            vtkPointSetToLabelHierarchy hie = vtkPointSetToLabelHierarchy.New();
            hie.SetInput(polyData);
            hie.SetMaximumDepth(15);
            hie.SetLabelArrayName("111");
            hie.SetTargetLabelCount(100);
            hie.SetTextProperty(textProp);

            vtkLabelPlacementMapper labelMapper = new vtkLabelPlacementMapper();
            labelMapper.SetInputConnection(hie.GetOutputPort());
            
            vtkFreeTypeLabelRenderStrategy strategy = new vtkFreeTypeLabelRenderStrategy();
            labelMapper.SetRenderStrategy(strategy);
            labelMapper.UseDepthBufferOn();
            labelMapper.SetShapeToNone();
            labelMapper.SetStyleToOutline();
            
            //labelMapper.UseUnicodeStringsOff();

            actor.SetMapper(labelMapper);  
            return actor;
        }

        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            
            // Create components of the rendering subsystem
            //
            vtkRenderer ren1 = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renWin = renderWindowControl1.RenderWindow;

            // Add the actors to the renderer, set the window size
            //
            if (paras.Using3DTower == 1)
            {
                vtkActor actor1 = BasicVTKBuilder();
                ren1.AddViewProp(actor1);
            }

            if (paras.UsingVirtualHeater == 1)
            {
                //MessageBox.Show("!");
                vtkActor2D actor2 = VirtualHeaterVTKBuilder();
                ren1.AddActor(actor2);
            }
           
            //ren1.AddViewProp(actor2);
            renWin.SetSize((int)paras.Height, (int)paras.Width);
            renWin.Render();
            vtkCamera camera = ren1.GetActiveCamera();
            camera.Zoom(1.5);
        }
    }
}
