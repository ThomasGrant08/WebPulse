﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebPulse2023.Models;

namespace WebPulse2023.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<WebPulse2023.Models.Website> Website { get; set; } = default!;
		public DbSet<WebPulse2023.Models.WebPing> WebPing { get; set; } = default!;
		public DbSet<WebPulse2023.Models.PingStatistic> PingStatistic { get; set; } = default!;
		public DbSet<WebPulse2023.Models.Group> Group { get; set; } = default!;
		public DbSet<WebPulse2023.Models.Role> Role { get; set; } = default!;
	}
}