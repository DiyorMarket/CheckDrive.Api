﻿using System;

namespace CheckDrive.DTOs.MechanicAcceptance
{
    public class MechanicAcceptanceForCreateDto
    {
        public bool IsAccepted { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }
        public int MechanicHandoverId { get; set; }
    }
}
