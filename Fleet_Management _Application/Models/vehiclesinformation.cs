using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management__Application.Models;

[Table("vehiclesinformations")]
public partial class Vehiclesinformation
{
    [Column("vehicleid")]
    public long Vehicleid { get; set; }

    [Column("driverid")]
    public long Driverid { get; set; }

    [Column("vehiclemake", TypeName = "character varying")]
    public string? Vehiclemake { get; set; }

    [Column("vehiclemodel", TypeName = "character varying")]
    public string? Vehiclemodel { get; set; }

    [Column("purchasedate")]
    public long? Purchasedate { get; set; }

    [Key]
    [Column("id")]
    public long Id { get; set; }

    [ForeignKey("Driverid")]
    [InverseProperty("Vehiclesinformations")]
    public virtual Driver Driver { get; set; } = null!;

    [ForeignKey("Vehicleid")]
    [InverseProperty("Vehiclesinformations")]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
