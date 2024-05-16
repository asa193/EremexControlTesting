﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPropGridFromEremexControl.Model.SmallData {
    partial class LaserModeSettings : ObservableObject {
        [ObservableProperty]
        int power;
    }
}
