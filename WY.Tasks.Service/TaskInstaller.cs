using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace WY.Tasks.Service {
    [RunInstaller(true)]
    public partial class TaskInstaller : System.Configuration.Install.Installer {
        public TaskInstaller() {
            InitializeComponent();
        }
    }
}
