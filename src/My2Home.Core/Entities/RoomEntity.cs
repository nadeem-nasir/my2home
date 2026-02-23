using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Room")]
    public class RoomEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }
        public string  RoomNumber {get;set;}
        public string RoomFloorNumber { get;set;}       
        public string RoomRemarks { get;set;}
        public int RoomRent {get;set;}
        public int RoomNoOfBeds { get; set; }
        public int RoomRentPerBed { get; set; }
        public int RoomHostelId { get; set; }



        [NotMapped]
        public int TotalRows { get; set; }


    }
}

////(Private, Dorm)
//Number of beds {get;set;}
//Bathroom(shared, Ensuite)
//Price(Per person or per bed or per room, per month, per night)
//Room facilities