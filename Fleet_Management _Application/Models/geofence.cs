using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management__Application.Models;

[Table("geofences")]
public partial class Geofence
{
    [Key]
    [Column("geofenceid")]
    public long Geofenceid { get; set; }

    [Column("geofencetype", TypeName = "character varying")]
    public string? Geofencetype { get; set; }

    [Column("addeddate")]
    public long? Addeddate { get; set; }

    [Column("strockcolor", TypeName = "character varying")]
    public string? Strockcolor { get; set; }

    [Column("strockopacity")]
    public float? Strockopacity { get; set; }

    [Column("strockweight")]
    public float? Strockweight { get; set; }

    [Column("fillcolor", TypeName = "character varying")]
    public string? Fillcolor { get; set; }

    [Column("fillopacity")]
    public float? Fillopacity { get; set; }

    [InverseProperty("Geofence")]
    public virtual ICollection<Circlegeofence> Circlegeofences { get; set; } = new List<Circlegeofence>();

    [InverseProperty("Geofence")]
    public virtual ICollection<Polygongeofence> Polygongeofences { get; set; } = new List<Polygongeofence>();

    [InverseProperty("Geofence")]
    public virtual ICollection<Rectanglegeofence> Rectanglegeofences { get; set; } = new List<Rectanglegeofence>();
}
