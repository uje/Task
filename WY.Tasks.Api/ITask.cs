﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace WY.Tasks.Api {
    public interface ITask:IPlugin {
        void Execute();
    }
}
