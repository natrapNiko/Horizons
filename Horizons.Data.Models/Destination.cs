﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Data.Models
{
    public class Destination
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string PublisherId { get; set; } = null!;

        public virtual IdentityUser Publisher { get; set; } = null!;

        public DateTime PublishedOn { get; set; }

        public int TerrainId { get; set; }

        public virtual Terrain Terrain { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserDestination> UsersDestinations { get; set; } = new HashSet<UserDestination>();
    }   
}
