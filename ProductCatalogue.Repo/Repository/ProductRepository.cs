using AutoMapper;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Repository.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogue.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        public ProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public bool AddProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            dbCon.Products.Add(_mapper.Map<Product>(prd));
            int noOfRows = dbCon.SaveChanges();
            return noOfRows > 0;
        }
        public bool DeleteProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            Product productToDelete = dbCon.Products.Where(i => i.Title.Equals(_mapper.Map<Product>(prd).Title)).FirstOrDefault();
            int noOfRows = 0;
            if (productToDelete != null)
            {
                dbCon.Products.Remove(productToDelete);
                noOfRows = dbCon.SaveChanges();
            }            
            return noOfRows > 0;
        }
        public ProductSearchResponse SearchProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            var result = dbCon.Products.Where(i=> i.Title.Equals(_mapper.Map<Product>(prd).Title)).ToList();
            ProductSearchResponse res = new ProductSearchResponse
            {
                Success = false
            };
            if (result != null)
            {
                res.Success = true;
                res.Products = _mapper.Map<List<ProductContract>>(result);
            }
            return res;
        }
        public bool UpdateProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            Product productToUpdate = dbCon.Products.Where(i => i.Title.Equals(_mapper.Map<Product>(prd).Title)).FirstOrDefault();
            int noOfRows = 0;
            if (productToUpdate != null)
            {
                productToUpdate.Quantity = prd.Quantity;
                productToUpdate.Title = prd.Title;
                productToUpdate.TotalCost = prd.TotalCost;
                productToUpdate.Cost = prd.Cost;
                dbCon.Products.Update(productToUpdate);
                noOfRows = dbCon.SaveChanges();
            }
            return noOfRows > 0;
        }
    }
}
