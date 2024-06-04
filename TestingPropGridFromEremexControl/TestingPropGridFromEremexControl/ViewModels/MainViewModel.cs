using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
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

        public ObservableRangeCollection<object> Nodes { get; set; } = [];

        [ObservableProperty]
        private ObservableRangeCollection<object> _selectedNodes = [];

        public MainViewModel() {
            List<object> list = new List<object>();
            while (list.Count < 10) {
                list.Add(new ManWithObser() { HaveName = true, Name = "With obs" });
                //list.Add(new ManWithoutObser() { HaveName = true, Name = "Without obs" });
            }

            Nodes.AddRange(list.ToArray());

            SelectedNodes.CollectionChanged += SelectedNodes_CollectionChanged;
        }

        private void SelectedNodes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
        }

        public void SelectAllWithNewCol() {
            SelectedNodes = new ObservableRangeCollection<object>(Nodes);
        }

        public void SelectAllWithAddRange() {

            SelectedNodes.Clear();
            SelectedNodes.AddRange(Nodes);
        }
    }

}
