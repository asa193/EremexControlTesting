using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPropGridFromEremexControl.ViewModels.CustomProperties {
    public class MyTypeDescriptionProvider: TypeDescriptionProvider {

        public MyTypeDescriptionProvider()
    : base(TypeDescriptor.GetProvider(typeof(object))) { }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type type, object o) {
            ICustomTypeDescriptor baseDescriptor = base.GetTypeDescriptor(type, o);
            return new MyTypeDescriptor(baseDescriptor);
        }
    }
}
