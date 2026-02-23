using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class RoomViewModel: BaseViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomFloorNumber { get; set; }
        [MaxLength(1000)]
        public string RoomRemarks { get; set; }
        public int RoomRent { get; set; }
        public int RoomNoOfBeds { get; set; }
        public int RoomRentPerBed { get; set; }
        public int RoomHostelId { get; set; }
        public int NumberOfRoomesToCreate { get; set; }

    }
}
