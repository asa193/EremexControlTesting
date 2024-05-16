using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using TestingPropGridFromEremexControl.Model;
using System.Collections.Generic;
using System.Collections;
using TestingPropGridFromEremexControl.ViewModels.CustomProperties;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        [ObservableProperty]
        bool showImage;

        [ObservableProperty]
        bool showRectangle;

        [ObservableProperty]
        bool showElipse;

        List<BasePrim> _primList = new List<BasePrim> {
            new CRectangle(),new CRectangle(),
            new CElipse(),new CElipse(),
            new CImage(), new CImage()};

        const int countOfObjects = 2;

        public MainViewModel() {
        }

        partial void OnShowRectangleChanged(bool value) {
            UpdateSelectedPrims();
        }

        partial void OnShowElipseChanged(bool value) {
            UpdateSelectedPrims();
        }

        partial void OnShowImageChanged(bool value) {
            UpdateSelectedPrims();
        }

        private void UpdateSelectedPrims() {
            List<BasePrim> newSelectedObjects =new List<BasePrim>();
            foreach(var prim in _primList) {
                if (prim is CImage && ShowImage) {
                    newSelectedObjects.Add(prim);
                }else if(prim is CRectangle && ShowRectangle) {
                    newSelectedObjects.Add(prim);
                }else if(prim is CElipse && ShowElipse) { 
                    newSelectedObjects.Add(prim);}
            }

            SelectedObjects = null;
            MyRowSource = GetMyRowSource(newSelectedObjects);
            SelectedObjects = newSelectedObjects.ToArray();
        }

        [ObservableProperty]
        public IEnumerable myRowSource;

        public IEnumerable GetMyRowSource(IEnumerable<object> SelectedObjects) {
            if (SelectedObjects.Count() < 1)
                return null;

            var firstObject = SelectedObjects.First();


            List<PropertyDescriptor> myProperties = new List<PropertyDescriptor>();
            myProperties.AddRange(TypeDescriptor.GetProperties(firstObject).Cast<PropertyDescriptor>());

            foreach (var o in SelectedObjects.Skip(1)) {
                var p = TypeDescriptor.GetProperties(firstObject).Cast<PropertyDescriptor>();
                myProperties = myProperties.IntersectBy(p.Select(n => n.Name), x => x.Name).ToList();
            }

            var list = new List<object>();
            foreach (var p in myProperties) {
                string? DisplayName = (p.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute)?.DisplayName;
                if (DisplayName == null)
                    DisplayName = p.Name;
                list.Add(new DefaultRowViewModel(null, null) { FieldName = p.Name, Caption = DisplayName, ReadOnly = false });
            }
            return list;
        }
    }

    public class HumanNeedsRowViewModel {
        // The category's display name.
        public string FieldName { get; set; }

    }

    public class CategoryRowViewModel {
        // The category's display name.
        public string Caption { get; set; }

        // Child row View Models that will be rendered as child rows.
        public IEnumerable Items { get; set; }
    }

    public class DefaultRowViewModel : ObservableObject {

        public DefaultRowViewModel(ObservableObject? model, PropertyDescriptor? propForAllow) {
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

        private PropertyDescriptor? _propForAllow;


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
