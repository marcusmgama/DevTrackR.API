using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrackR.API.Entities
{
    public class PackageUpdate
    {
        public PackageUpdate(string status, int packageId)
        {
            PackageId = packageId;
            Status = status;
            UpdatedAt = DateTime.Now;
        }

        public int Id { get; private set; }
        public int PackageId { get; private set; }
        public string Status { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}