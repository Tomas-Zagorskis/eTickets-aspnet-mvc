using System.ComponentModel.DataAnnotations;

namespace eTickets.Models {
    public class OrderItem {
        [Key]
        public int Id { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }

        public int MovieId { get; set; } // MovieId as foreign key for Movie
        public virtual Movie Movie { get; set; }
    }
}
