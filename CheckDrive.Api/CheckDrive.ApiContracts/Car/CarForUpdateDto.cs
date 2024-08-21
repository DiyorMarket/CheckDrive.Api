﻿using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.Car
{
    public class CarForUpdateDto
    {
        [Required(ErrorMessage = "ID kiritish majburiy")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Modelni kiritish majburiy")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Rangni kiritish majburiy")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Raqamni kiritish majburiy")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Avtomobil bosib o'tgan masofasini kiritish majburiy")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "Avtomobilni 1 yillik bosib o'tish meyorini kiritish majburiy")]
        [Range(0, int.MaxValue, ErrorMessage = "Avtomobilni 1 yillik bosib o'tish meyori manfiy bo'lishi mumkin emas")]
        public int OneYearMediumDistance { get; set; }

        [Required(ErrorMessage = "O'rtacha yoqilg'i sarfini kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "O'rtacha yoqilg'i sarfi manfiy bo'lishi mumkin emas")]
        public double MeduimFuelConsumption { get; set; }

        [Required(ErrorMessage = "Yoqilg'i baki sig'imini kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Yoqilg'i baki sig'imi manfiy bo'lishi mumkin emas")]
        public double FuelTankCapacity { get; set; }

        [Required(ErrorMessage = "Yoqilg'i hajmini kiritish majburiy")]
        [Range(0, double.MaxValue, ErrorMessage = "Yoqilg'i hajmi manfiy bo'lishi mumkin emas")]
        public double RemainingFuel { get; set; }

        [Required(ErrorMessage = "Ishlab chiqarilgan yilni kiritish majburiy")]
        public int ManufacturedYear { get; set; }
    }
}
