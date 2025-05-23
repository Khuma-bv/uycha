using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uycha.Models
{
    // Property types available
    public enum PropertyType
    {
        LandHouse,      // House
        Apartment,      // Apartment
        Land,           // Land plot
        Commercial      // Commercial property
    }

    // Listing types
    public enum ListingType
    {
        Sale,   // For sale
        Rent    // For rent
    }

    // Rental duration options
    public enum RentDuration
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    // Core Property entity class
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string City { get; set; } // City where the property is located

        [Required]
        public string Address { get; set; } // Property address

        // Map points
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Required]
        public string Description { get; set; } // Description text

        [Required]
        public int Price { get; set; } // Price in local currency

        [Required]
        public int Area { get; set; } // Property area in square meters

        [Required]
        public int Rooms { get; set; } // Number of rooms

        [Required]
        public int Floor { get; set; } // Floor number

        [Required]
        public string PhoneNumber { get; set; } // Contact phone number

        [Required]
        public string TelegramLink { get; set; } // Telegram contact

        [Required(ErrorMessage = "Image is required!")]
        public string ImagePaths { get; set; } // Semicolon-separated image paths

        public DateTime PostedDate { get; set; } = DateTime.UtcNow; // Date of posting


        public bool IsApproved { get; set; } // Admin approval status

        [Required]
        public string UserId { get; set; } // ID of the user who posted

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } // Navigation to Identity user

        [Required]
        public ListingType ListingType { get; set; } // Type of listing: Sale or Rent

        [Required]
        public PropertyType PropertyType { get; set; } // Property category

        public RentDuration? RentDuration { get; set; } // Duration for rental listings

        // Additional property features
        public bool HasBalcony { get; set; }
        public bool HasElevator { get; set; }
        public bool HasFurniture { get; set; }
        public bool HasHeating { get; set; }
        public bool HasParking { get; set; }
        public bool IsNegotiable { get; set; }
        public bool IsPremium { get; set; }
    }

    // Optional: Extended info for commercial properties
    public class CommercialDetails
    {
        [Key]
        public int Id { get; set; }

        public int ParkingSpaces { get; set; } // Number of parking spaces
        public bool HasConferenceRoom { get; set; } // Conference room availability
        public bool HasWarehouse { get; set; } // Warehouse availability
        public bool SuitableForOffice { get; set; } // Office-suitability

        [ForeignKey("PropertyId")]
        public int PropertyId { get; set; } // Link to base property
        public Property Property { get; set; } // Navigation to base property
    }
}
