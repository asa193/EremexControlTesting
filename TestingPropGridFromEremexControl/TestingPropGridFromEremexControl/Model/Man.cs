using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPropGridFromEremexControl.ViewModels.CustomProperties;

namespace TestingPropGridFromEremexControl.Model {

    public interface IForNotify {
        void ForceOnPropChange(string  propertyName);
    }

    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    partial class ManWithObser : ObservableObject, IForNotify {
        public ManWithObser() {
            mainBrainNeed = new HumanNeeds() { SleepNeed = 8, SunNeed = 2 };
            secondBrainNeed= new HumanNeeds() { SleepNeed=9, SunNeed=10 };
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
        [property:AttributeForAddExpand]
        [property:Category("MainBrain")]
        public HumanNeeds mainBrainNeed;

        [ObservableProperty]
        [property: AttributeForAddExpand]
        [property: Category("SecondBrain")]
        public HumanNeeds secondBrainNeed;

        public void ForceOnPropChange(string propertyName) {
            OnPropertyChanged(propertyName);
        }
    }

    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    public class ManWithoutObser {

        [AllowedByProp(Inverse = false, PropName = nameof(HaveName))]
        [DisplayName("Имя")]
        public string Name { get; set; }
        public bool HaveName { get; set; }

        public string EmptyBox { get; set; }

        [AttributeForAddExpand]
        [Category("MainBrainNeed")]
        public HumanNeeds MainBrainNeed { get; set; } = new HumanNeeds() { SleepNeed=8, SunNeed=2 };

        [AttributeForAddExpand]
        [Category("SecondBrain")]
        public HumanNeeds SecondBrainNeed { get; set; } = new HumanNeeds() { SleepNeed = 2, SunNeed = 4 };
    }


    public class AllowedByProp : Attribute {
        public required string PropName { get; set; }
        public required bool Inverse { get; set; }
    }

    public class AttributeForAddExpand :Attribute{
        
    }
}
