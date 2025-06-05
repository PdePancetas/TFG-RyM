using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRCars.Models
{
    public class RequestDTO
    {
       
        public Request Request { get; set; }
        public String notes { get; set; }

        public RequestDTO(Request request, string notes)
        {
            this.Request = request;
            this.notes = notes;
        }

    }
}
