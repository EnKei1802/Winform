using ExcerciseTwo.Utilities;

namespace ExcerciseTwo.Models
{
    public class Product
    {
        public int productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int categoryId { get; set; }
        public DateTime createdDate { get; set; }
        public ProductType type { get; set; }
        public int quantity { get; set; }
        public string photo { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }

        public override bool Equals(object? obj)
        {
            // If the passed object is null
            if(obj == null)
            {
                return false;
            }
            if(!(obj is Product))
            {
                return false;
            }
            return (this.productId == ((Product)obj).productId)
                && (this.name == ((Product)obj).name)
                && (this.price == ((Product)obj).price)
                && (this.categoryId == ((Product)obj).categoryId)
                && (this.createdDate == ((Product)obj).createdDate)
                && (this.type == ((Product)obj).type)
                && (this.quantity == ((Product)obj).quantity)
                && (this.photo == ((Product)obj).photo)
                && (this.description == ((Product)obj).description)
                && (this.isActive == ((Product)obj).isActive);
            
        }

        //Overriding the GetHashCode method
        //GetHashCode method generates hashcode for the current object
        public override int GetHashCode()
        {
            //Performing BIT wise OR Operation on the generated hashcode values
            //If the corresponding bits are different, it gives 1.
            //If the corresponding bits are same, it gives 0.
            return    productId.GetHashCode() 
                    ^ name.GetHashCode() 
                    ^ price.GetHashCode() 
                    ^ categoryId.GetHashCode() 
                    ^ createdDate.GetHashCode() 
                    ^ type.GetHashCode() 
                    ^ quantity.GetHashCode() 
                    ^ photo.GetHashCode() 
                    ^ description.GetHashCode();
        }
    }

}
