using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.Objects.Terrains
{
    public abstract class TerrainBase
    {
        public int Slope { get; set; }
        public int Length { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public int Hills { get; set; }
    }
}
