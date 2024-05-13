using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPropGridFromEremexControl.Model;

namespace TestingPropGridFromEremexControl.ViewModels.CustomProperties {
    public class MyTypeDescriptor: CustomTypeDescriptor {

        public MyTypeDescriptor(ICustomTypeDescriptor original) : base(original) { }

        public override PropertyDescriptorCollection GetProperties() => GetProperties(Array.Empty<Attribute>());

        public override PropertyDescriptorCollection GetProperties(Attribute[]? attributes) {
            var def = base.GetProperties(attributes);
            List<PropertyDescriptor> NewDescr = new List<PropertyDescriptor>();

            foreach (PropertyDescriptor des in def) {
                var newDes = new MyPropertyDescriptor(des,new List<MyPropertyDescriptor>());
                var descr = des.Attributes[typeof(AttributeForAddExpand)] as AttributeForAddExpand;
                if (descr != null) {
                    var a = des?.GetChildProperties(attributes);
                    foreach(PropertyDescriptor p in a) {
                        NewDescr.Add(new MyPropertyDescriptor(p, new List<MyPropertyDescriptor>() { newDes }) );
                    }
                }
                else
                    NewDescr.Add(newDes);
            }
            return new PropertyDescriptorCollection(NewDescr.ToArray());
        }
    }
}
