using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class ContinentalDbContext : DbContext
    {
        public ContinentalDbContext()
        {
            
        }

        public ContinentalDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Conentinal;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContinentalDbContext).Assembly);
        }

        #region DBSet
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<AmenityRoomDetail> AmenityRoomDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<RoomBookingDetail> RoomBookingDetails { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<RoomDetail> RoomDetails { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceOrderDetail> ServiceOrderDetails { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherDetail> VouchersDetails { get; set; }
        #endregion
    }
}
