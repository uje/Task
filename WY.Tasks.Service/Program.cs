using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WY.Tasks.Service {
    class Program {
        static void Main(string[] args) {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { 
				new TaskService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
