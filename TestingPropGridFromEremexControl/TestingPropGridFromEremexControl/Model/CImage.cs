using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPropGridFromEremexControl.Model.SmallData;

namespace TestingPropGridFromEremexControl.Model {
    partial class CImage : BasePrim {
        LaserModeSettings _fillingModes;
        FillSettings _fillSettings;

        [ObservableProperty]
        int lpmPerPixel;

    }
}
