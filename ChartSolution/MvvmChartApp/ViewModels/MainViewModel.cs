using Caliburn.Micro;
using System;
using System.Windows.Shapes;

namespace MvvmChartApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        public void LoadLineChart()
        {
            ActivateItem(new LineChartViewModel());
        }

        public void LoadGaugeChart()
        {
            ActivateItem(new GaugeChartViewModel());
        }

        public void ExitProgram()
        {
            Environment.Exit(0); // 0은 오류없이 끝내기
        }

    }
}
