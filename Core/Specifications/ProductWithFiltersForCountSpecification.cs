using Core.Entities;

namespace Core.Specifications;

public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
         : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains
            (productParams.Search)) && 
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && // || is or else operator, does expression to right in else case also
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
    {
    }
}
