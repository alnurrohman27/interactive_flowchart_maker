using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleChart.Shapes
{
    class TextLabel : PuzzleObject
    {
        private Brush brush;
        private Font font;

        public TextLabel()
        {
            this.brush = new SolidBrush(Color.Black);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            FontFamily fontFamily = new FontFamily("Arial");
            font = new Font(
               fontFamily,
               16,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
        }

        public override bool Add(PuzzleObject obj)
        {
            throw new NotImplementedException();
        }

        public override bool Intersect(int xTest, int yTest)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(PuzzleObject obj)
        {
            throw new NotImplementedException();
        }

        public override void RenderOnEditingView()
        {
            throw new NotImplementedException();
        }

        public override void RenderOnPreview()
        {
            throw new NotImplementedException();
        }

        public override void RenderOnStaticView()
        {
            throw new NotImplementedException();
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            throw new NotImplementedException();
        }
    }
}
