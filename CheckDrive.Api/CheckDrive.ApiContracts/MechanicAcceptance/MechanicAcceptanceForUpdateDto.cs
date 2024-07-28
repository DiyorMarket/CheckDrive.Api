﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.MechanicAcceptance
{
    public class MechanicAcceptanceForUpdateDto
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Yakuniy masofani kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Yakuniy masofa manfiy bo'lishi mumkin emas")]
        public double Distance { get; set; }
        public double RemainingFuel { get; set; }

        public int MechanicId { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
    }
}
