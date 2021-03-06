﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable 1591

namespace QA.Core.Performance
{
    public class OperationCancelledException : Exception
    {
        public OperationCancelledException(string msg) : base(msg) { }
        public OperationCancelledException(string msg, Exception inner) : base(msg, inner) { }
    }
}
