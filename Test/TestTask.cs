using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test {
    public class TestTask:WY.Tasks.Api.Task {
        public override void Execute() {
            AppSettings["test"] = "haha";
        }
    }
}
