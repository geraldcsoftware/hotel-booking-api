using HotelBooking.Api.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Models;

[EntityTypeConfiguration(typeof(FeatureDescriptionConfiguration))]
public class FeatureDescription
{
    public string? Id { get; set; }
    public string? HotelId { get; set; }
    public string? FeatureName { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
}