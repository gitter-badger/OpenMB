﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public class AIMeshIndexData
    {
        public List<int> VertexNumber { get; set; }

        public AIMeshIndexData()
        {
            VertexNumber = new List<int>();
        }
    }
}