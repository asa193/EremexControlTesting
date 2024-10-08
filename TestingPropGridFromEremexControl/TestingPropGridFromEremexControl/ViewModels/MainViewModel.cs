﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        bool useObservableObject;

        List<ManWithObser> listOfObser = new List<ManWithObser>();
        List<ManWithoutObser> listOfWithoutObser = new List<ManWithoutObser>();

        const int countOfObjects = 2;

        public MainViewModel() {
            for (int i = 0; i < countOfObjects; i++) {
                listOfObser.Add(new ManWithObser() { HaveName = true, Name = "With obs" });
                listOfWithoutObser.Add(new ManWithoutObser() { HaveName = true, Name = "Without obs" });
            }
            listOfObser[0].MainBrainNeed =new HumanNeeds() { SunNeed = 0, SleepNeed = 8 };
            listOfWithoutObser[0].MainBrainNeed = new HumanNeeds() { SunNeed = 0, SleepNeed = 8 };
            OnUseObservableObjectChanged(useObservableObject);

            //Поток чтобы точно проверить что меняются свойства, не зависимо Observable или нет.
            Thread b = new Thread(Timer);
            b.IsBackground = true;
            b.Start();
        }

        partial void OnUseObservableObjectChanged(bool value) {
            object[] newSelectedObjects;
            if (value)
                newSelectedObjects = listOfObser.ToArray();
            else
                newSelectedObjects = listOfWithoutObser.ToArray();

            SelectedObjects = null;
            MyRowSource = GetMyRowSource(newSelectedObjects);
            SelectedObjects = newSelectedObjects;
        }

        partial void OnTestingNameChanged(string value) {
            switch (SelectedObjects?[0]) {
                case ManWithObser man:
                    man.Name = value;
                    break;
                case ManWithoutObser manWithout:
                    manWithout.Name = value;
                    break;
            }
        }

        private void Timer() {
            while (true) {
                switch (SelectedObjects?[0]) {
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

                var Allowed = (p.Attributes[typeof(AllowedByProp)] as AllowedByProp);

                if (Allowed == null)
                    list.Add(new DefaultRowViewModel(null, null) { FieldName = p.Name, Caption = DisplayName, ReadOnly = false });
                else
                    list.Add(new DefaultRowViewModel(firstObject as ObservableObject, myProperties.Find(p => p.Name == Allowed.PropName)) { FieldName = p.Name, Caption = DisplayName });
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
