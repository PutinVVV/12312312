
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Shyrigin_tamograma
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public int Clamp(int val, int min, int max)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }
        Color TransferFunction(short val)
        {
            int min = 0;
            int max = 2000;
            int newVal = Clamp((val - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }
        class Bin
        {
            public static int X, Y, Z;
            public static short[] array;
            public Bin() { }
            public void readBIN(string path)
            {
                if (File.Exists(path))
                {
                    BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
                    X = reader.ReadInt32();
                    Y = reader.ReadInt32();
                    Z = reader.ReadInt32();
                    int arraySize = X * Y * Z;
                    array = new short[arraySize];
                    for (int i = 0; i < arraySize; i++)
                        array[i] = reader.ReadInt16();
                }
            }
        }
        class View
        {
            public void SetupView(int wid, int hei)
            {
                GL.ShadeModel(ShadingModel.Smooth);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
                GL.Viewport(0, 0, wid, hei);
            }
            public void DrawQuads(int layerNumber)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Begin(BeginMode.Quads);
                for (int x_coord = 0; x_coord < Bin.X - 1; x_coord++)
                    for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
                    {
                        short value;
                        value = Bin.array[x_coord + y_coord * Bin.X + layerNumber * Bin.X * Bin.Y];
                        short val1 = TransferFunction(value);
                        GL.Color3(TransferFunction(value));

                    }
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

    }

}

