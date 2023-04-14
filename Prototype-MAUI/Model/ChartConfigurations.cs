using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Model
{
    public class ChartConfiguration
    {
        public float XMax { get; set; }
        public SolidColorPaint LegendBackgroundPaint { get; set; }
        public LabelVisual Title { get; internal set; }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public SolidColorPaint LegendTextPaint { get; set; }
    }
}
