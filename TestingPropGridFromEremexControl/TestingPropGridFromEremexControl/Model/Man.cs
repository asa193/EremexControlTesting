using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPropGridFromEremexControl.Model {
    partial class ManWithObser : ObservableObject {
        public ManWithObser() {
            mainBrainNeed = new HumanNeeds() { SleepNeed = 8, SunNeed = 2 };
        }

        [ObservableProperty]
        [property:AllowedByProp(Inverse =false, PropName =nameof(HaveName))]
        [property:DisplayName("Имя")]
        string name;

        [ObservableProperty]
        bool haveName;

        [ObservableProperty]
        public string emptyBox;

        [ObservableProperty]
        public HumanNeeds mainBrainNeed;
    }

    public class ManWithoutObser {
        [AllowedByProp(Inverse = false, PropName = nameof(HaveName))]
        [DisplayName("Имя")]
        public string Name { get; set; }
        public bool HaveName { get; set; }

        public string EmptyBox { get; set; }
    }


    public class AllowedByProp : Attribute {
        public required string PropName { get; set; }
        public required bool Inverse { get; set; }
    }
}
