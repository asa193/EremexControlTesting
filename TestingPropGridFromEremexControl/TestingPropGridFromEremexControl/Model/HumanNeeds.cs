using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPropGridFromEremexControl.Model {
    public partial class HumanNeeds:ObservableObject {
        [ObservableProperty]
        float sleepNeed;

        [ObservableProperty]
        float sunNeed;


    }
}
