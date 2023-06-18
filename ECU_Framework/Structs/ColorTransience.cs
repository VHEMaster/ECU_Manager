using ECU_Framework.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    public class ColorTransience
    {
        private float fColorTransMin = 0.0f;
        private float fColorTransMax = 1.0f;
        private Color cDefaultColor;

        List<Color> lcColors;
        List<float> lfValues;
        object mutex = new object();

        public Color[] GetColors()
        {
            return lcColors.ToArray();
        }

        public ColorTransience(float rangeFrom, float rangeTo, Color colorDefault)
        {
            fColorTransMin = rangeFrom;
            fColorTransMax = rangeTo;
            cDefaultColor = colorDefault;
            lcColors = new List<Color>();
            lfValues = new List<float>();
        }

        public void Add(Color color, float value)
        {
            lock (mutex)
            {
                lcColors.Add(color);
                lfValues.Add(value);
            }
        }

        public Color Get(float value)
        {
            Color color = new Color();

            int r, g, b;
            Interpolation interpolation;
            Color[] colors = new Color[2];

            if(lcColors.Count == 0)
            {
                color = cDefaultColor;
            }
            else if (value <= fColorTransMin)
            {
                color = lcColors.First();
            }
            else if (value >= fColorTransMax)
            {
                color = lcColors.Last();
            }
            else
            {
                lock (mutex)
                {
                    interpolation = new Interpolation(value, lfValues.ToArray(), lfValues.Count);
                    colors[0] = lcColors[interpolation.indexes[0]];
                    colors[1] = lcColors[interpolation.indexes[1]];
                }

                r = (int)((colors[1].R - colors[0].R) * interpolation.mult + colors[0].R);
                g = (int)((colors[1].G - colors[0].G) * interpolation.mult + colors[0].G);
                b = (int)((colors[1].B - colors[0].B) * interpolation.mult + colors[0].B);

                color = Color.FromArgb(r, g, b);
            }

            return color;
        }
    }
}
