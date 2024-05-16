using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPropGridFromEremexControl.Model;

namespace TestingPropGridFromEremexControl.ViewModels.CustomProperties {
    public class MyPropertyDescriptor : PropertyDescriptor {
        PropertyDescriptor _p;
        List<MyPropertyDescriptor> _pathToProperty;
        string _name;

        public MyPropertyDescriptor(PropertyDescriptor p, List<MyPropertyDescriptor> pathToProperty) : base(p) {
            _p = p;
            _pathToProperty = pathToProperty;
            _name = "";
            foreach (var prop in _pathToProperty) {
                _name += prop.Name+".";
            }
            _name += _p.Name;
        }

        public override string Name => _name;

        public override Type ComponentType => _p.ComponentType;

        public override bool IsReadOnly => _p.IsReadOnly;

        public override Type PropertyType => _p.PropertyType;

        public override bool CanResetValue(object component) {
            return _p.CanResetValue(component);
        }

        public override object? GetValue(object? component) {
            foreach(var p in _pathToProperty) {
                component = p.GetValue(component);
            }
            return _p.GetValue(component);
        }

        public override void ResetValue(object component) {
            _p.ResetValue(component);
        }

        public override void SetValue(object? component, object? value) {
            var objectForSetValue = component;
            foreach (var p in _pathToProperty) {
                objectForSetValue = p.GetValue(component);
            }
            _p.SetValue(objectForSetValue, value);
            //if (component is IForNotify notify)
            //    notify.ForceOnPropChange(Name);
        }

        public override bool ShouldSerializeValue(object component) {
            return _p.ShouldSerializeValue(component);
        }
    }
}
