using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using TestingPropGridFromEremexControl.Model;

namespace TestingPropGridFromEremexControl.ViewModels {
    partial class MainViewModel : ViewModelBase {
        [ObservableProperty]
        string testingName;

        [ObservableProperty]
        string currentName;

        [ObservableProperty]
        bool currentReadOnly;

        [ObservableProperty]
        object[] selectedObjects = [];

        public MainViewModel() {
            List<object> list = new List<object>();
            while (list.Count < 1000) {
                list.Add(new ManWithObser() { HaveName = true, Name = "With obs" });
                //list.Add(new ManWithoutObser() { HaveName = true, Name = "Without obs" });
            }
            
            SelectedObjects = list.ToArray();

            //Поток чтобы точно проверить что меняются свойства, не зависимо Observable или нет.
            Thread b = new Thread(Timer);
            b.IsBackground = true;
            b.Start();
        }

        partial void OnTestingNameChanged(string value) {
            switch (SelectedObjects[0]) {
                case ManWithObser man:
                    man.Name = value;
                    break;
                case ManWithoutObser manWithout:
                    manWithout.Name = value;
                    break;
            }
        }

        private void Timer() {
            while(true) {
                switch (SelectedObjects[0]) {
                    case ManWithObser man:
                        CurrentName = man.Name;
                        CurrentReadOnly = man.HaveName;
                        break;
                    case ManWithoutObser manWithout:
                        CurrentName = manWithout.Name;
                        CurrentReadOnly = manWithout.HaveName;
                        break;
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        public IEnumerable MyRowSource { get => GetMyRowSource(); }

        public IEnumerable GetMyRowSource() {
            if (SelectedObjects.Length == 0)
                return new List<object>();

            var firstObject = SelectedObjects.First();
            List<PropertyInfo> myProperties = [.. firstObject.GetType().GetProperties()];

            foreach (var o in SelectedObjects.Skip(1)) {
                var p = o.GetType().GetProperties();
                myProperties = myProperties.IntersectBy(p.Select(n => n.Name), x => x.Name).ToList();
            }

            var list = new List<object>();
            foreach (var p in myProperties) {
                string? DisplayName = p.GetCustomAttributes(typeof(DisplayNameAttribute)).Cast<DisplayNameAttribute>().FirstOrDefault()?.DisplayName;
                if (DisplayName == null)
                    DisplayName = p.Name;

                var Allowed = p.GetCustomAttributes(typeof(AllowedByProp)).Cast<AllowedByProp>().FirstOrDefault();

                if(Allowed == null)
                    list.Add(new DefaultRowViewModel(null, null) { FieldName = p.Name, Caption = DisplayName, ReadOnly = false });
                else
                    list.Add(new DefaultRowViewModel(firstObject as ObservableObject, myProperties.Find(p => p.Name == Allowed.PropName)) 
                    { FieldName = p.Name, Caption = DisplayName });
            }
            return list;
        }
    }

    public class CategoryRowViewModel {
        // The category's display name.
        public string Caption { get; set; }

        // Child row View Models that will be rendered as child rows.
        public IEnumerable Items { get; set; }
    }

    public class DefaultRowViewModel : ObservableObject {

        public DefaultRowViewModel(ObservableObject? model, PropertyInfo? propForAllow) {
            _model = model;
            _propForAllow = propForAllow;
            if(model !=null)
                model.PropertyChanged += OnPropertyChanged;
            ReadOnly = !AllowProp();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == _propForAllow?.Name)
                ReadOnly = !AllowProp();
        }

        private bool AllowProp() {
            if(_model == null) return true;
            if (_propForAllow?.GetValue(_model) is bool b)
                return b;
            else
                return true;
        }

        private ObservableObject? _model;

        private PropertyInfo? _propForAllow;

        // The path to a target object's property.
        public required string FieldName { get; set; }

        // The property's display name.
        public required string Caption { get; set; }

        private bool enabled;
        public bool ReadOnly {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }
    }
}
