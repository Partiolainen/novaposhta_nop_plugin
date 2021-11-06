using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class Dimensions : BaseEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        
        public Dimensions SetDimensionsByProduct(Product product)
        {
            Width = (int)(product.Width * 100);
            Height = (int)(product.Height * 100);
            Length = (int)(product.Length * 100);

            return this;
        }
        
        public int[] ToArray()
        {
            return new[] { Width, Length, Height };
        }
        
        public static bool operator > (Dimensions dimensions1, Dimensions dimensions2)
        {
            var dim1 = dimensions1.ToArray();
            var dim2 = dimensions2.ToArray();

            foreach (var value in dim1)
            {
                if (dim1[0] > dim2[0] && dim1[1] > dim2[1] && dim1[2] > dim2[2])
                    return true;

                ShiftBackArray(dim1);
            }

            return false;
        }
        
        public static bool operator >= (Dimensions dimensions1, Dimensions dimensions2)
        {
            var dim1 = dimensions1.ToArray();
            var dim2 = dimensions2.ToArray();

            foreach (var value in dim1)
            {
                if (dim1[0] >= dim2[0] && dim1[1] >= dim2[1] && dim1[2] >= dim2[2])
                    return true;

                ShiftBackArray(dim1);
            }

            return false;
        }
        
        public static bool operator <= (Dimensions dimensions1, Dimensions dimensions2)
        {
            var dim1 = dimensions1.ToArray();
            var dim2 = dimensions2.ToArray();

            foreach (var value in dim1)
            {
                if (dim1[0] <= dim2[0] && dim1[1] <= dim2[1] && dim1[2] <= dim2[2])
                    return true;

                ShiftBackArray(dim1);
            }

            return false;
        }
        
        public static bool operator < (Dimensions dimensions1, Dimensions dimensions2)
        {
            var dim1 = dimensions1.ToArray();
            var dim2 = dimensions2.ToArray();

            foreach (var value in dim1)
            {
                if (dim1[0] < dim2[0] && dim1[1] < dim2[1] && dim1[2] < dim2[2])
                    return true;

                ShiftBackArray(dim1);
            }

            return false;
        }
        
        private static void ShiftBackArray(int[] array)
        {
            if (array.Length <= 1) return;

            var first = array[0];
            
            for (var i = 1; i < array.Length; i++)
            {
                array[i - 1] = array[i];
            }

            array[^1] = first;
        }
    }
}