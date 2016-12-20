using PuzzleChart.Api.Interfaces;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PuzzleChart.Commands
{
    class ExportCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;
        public int height;
        public int width;
        public int imageWidth;
        public int imageHeight;
        public Bitmap bmp;

        public ExportCommand(ICanvas canvas, IEditor editor, int width, int height, int imageWidth, int imageHeight, Bitmap bmp)
        {
            this.canvas = canvas;
            this.editor = editor;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.width = width;
            this.height = height;
            this.bmp = bmp;
        }

        public void Execute()
        {
            Debug.WriteLine("Export File is selected");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Portable Network Graphic (*.png)|*.png";
            saveFileDialog1.Title = "Export an Document";
            saveFileDialog1.ShowDialog();
            string filename = saveFileDialog1.FileName;

            int dx = 32;
            int dy = 100;
            Bitmap whole_form = bmp;
            int wid = imageWidth;
            int hgt = imageHeight;
            Bitmap bm = new Bitmap(wid, hgt);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(whole_form, 0, 0,
                    new Rectangle(dx, dy, wid, hgt),
                    GraphicsUnit.Pixel);
            }

            if (filename != "")
            {
                bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png); //save location and type
            }
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}
