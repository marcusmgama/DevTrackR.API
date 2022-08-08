using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrackR.API.Models
{
    public class AddPackageInputModel
    {
        public string Title { get; private set; }
        public decimal Weight { get; private set; }
        
    }
}