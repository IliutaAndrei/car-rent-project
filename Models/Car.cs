using CarRent.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRent.Models;
public class Car
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Marca este obligatorie.")]
    [Display(Name = "Marcă")]
    public string Make { get; set; }

    [Required(ErrorMessage = "Modelul este obligatoriu.")]
    [Display(Name = "Model")]
    public string Model { get; set; }

    [Required(ErrorMessage = "Anul Fabricației este obligatoriu.")]
    [Display(Name = "Anul Fabricației")]
    public int YearOfFabrication { get; set; }

    [Display(Name = "Tip Combustibil")]
    public FuelType FuelType { get; set; }

    [Display(Name = "Tip Transmisie")]
    public TransmissionType TransmissionType { get; set; }

    [Display(Name = "Disponibilă")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "Prețul pe zi este obligatoriu.")]
    [Display(Name = "Preț pe Zi")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Prețul trebuie să fie mai mare decât 0.")]
    public decimal PricePerDay { get; set; }
}