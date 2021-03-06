﻿using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPGMiner.Handler
{
    public class FPGTreeNode
    {
        public Skill Skill { get; set; }

        public List<FPGTreeNode> ChildNodes { get; set; }

        public FPGTreeNode ParentNode { get; set; }

        public int FrequencyCount { get; set; }
    }
}
