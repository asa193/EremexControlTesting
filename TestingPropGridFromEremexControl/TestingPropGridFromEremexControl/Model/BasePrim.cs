using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPropGridFromEremexControl.Model.SmallData;

namespace TestingPropGridFromEremexControl.Model {
    partial class BasePrim : ObservableObject {
        [ObservableProperty]
        string name="base";
    }
}
