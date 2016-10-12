using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.ViewModels
{

    public class ViewModelCommunication : MvxMessage
    {
        public ViewModelCommunication(object sender, string msg) : base(sender)
        {
            Msg = msg;

        }

        public string Msg { get; private set; }
    }

}
