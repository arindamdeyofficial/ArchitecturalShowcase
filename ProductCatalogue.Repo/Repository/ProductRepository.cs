using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Repository.EfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace ProductCatalogue.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private CancellationToken _cnclToken;
        private int _noOfRows = 0;
        public ProductRepository(IMapper mapper)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            _cnclToken = cts.Token;
            _mapper = mapper;
        }        
        public async Task<bool> AddProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();            
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            dbCon.Products.Add(_mapper.Map<Product>(prd));
            _noOfRows = await dbCon.SaveChangesAsync(true, _cnclToken);
            return _noOfRows > 0;
        }
        public async Task<bool> DeleteProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            Product productToDelete = dbCon.Products.Where(i => i.Title.Equals(_mapper.Map<Product>(prd).Title)).FirstOrDefault();
            int noOfRows = 0;
            if (productToDelete != null)
            {
                dbCon.Products.Remove(productToDelete);
                noOfRows = await dbCon.SaveChangesAsync(true, _cnclToken);
            }            
            return noOfRows > 0;
        }
        public async Task<ProductSearchResponse> SearchProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            var result = await dbCon.Products.Where(i=> i.Title.Equals(_mapper.Map<Product>(prd).Title)).ToListAsync();
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
        public async Task<bool> UpdateProduct(ProductContract prd)
        {
            ProductContext dbCon = new ProductContext();
            //Automapper performance issue will occur in case of 1000+ mapping. Here it should have no performance issue
            Product productToUpdate = await dbCon.Products.Where(i => i.Title.Equals(_mapper.Map<Product>(prd).Title)).FirstOrDefaultAsync();
            int noOfRows = 0;
            if (productToUpdate != null)
            {
                productToUpdate.Quantity = prd.Quantity;
                productToUpdate.Title = prd.Title;
                productToUpdate.TotalCost = prd.TotalCost;
                productToUpdate.Cost = prd.Cost;
                dbCon.Products.Update(productToUpdate);
                noOfRows = await dbCon.SaveChangesAsync(true, _cnclToken);
            }
            return noOfRows > 0;
        }
        public async Task<bool> AddProductDapper(ProductContract prd)
        {
            int affectedRows = 0;
            string sqlCustomerInsert = @"INSERT INTO [dbo].[Products]
           ([Title]
           ,[BusinessName]
           ,[Cost]
           ,[Quantity]
           ,[TotalCost])
     VALUES
           (@Title
           ,@BusinessName
           ,@Cost
           ,@Quantity
           ,@TotalCost)";


            using (var connection = new SqlConnection(
                ))
            {
                affectedRows = await connection.ExecuteAsync(sqlCustomerInsert, new 
                { 
                    Title = "TestProductDapper",
                    BusinessName = "TestProductDapper",
                    Cost = 12,
                    Quantity = 3,
                    TotalCost = 36
                });
            }
            return affectedRows > 0;
        }
    }
}
