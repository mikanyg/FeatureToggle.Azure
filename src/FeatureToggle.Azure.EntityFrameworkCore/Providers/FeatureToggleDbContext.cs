using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureToggle.Azure.Providers
{
    public class FeatureToggleDbContext : DbContext
    {
        DbSet<BooleanFeatureToggleEntity> BooleanToggles { get; set; }
        DbSet<DateTimeFeatureToggleEntity> DateTimeToggles { get; set; }
    }    
}
