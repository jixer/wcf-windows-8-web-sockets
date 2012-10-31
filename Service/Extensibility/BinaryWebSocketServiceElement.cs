using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Extensibility
{
    public class BinaryWebSocketServiceElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(BinaryWebSocketService); }
        }

        protected override object CreateBehavior()
        {
            return new BinaryWebSocketService();
        }
    }
}
